using DALContracts;
using ImageMarkingServiceContracs;
using ImakeMarkingDTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace DocumentServiceImpl
{
    public class DocumentsService : IDocument
    {
        IDAL _dal;
        private IUserService _userService;
        private string _serverPath;
        private string _pathFrorClient;
        public DocumentsService(IDAL dal, IUserService userService)
        {
            _dal = dal;
            _dal.ConnectionStr = "Server = RACHELMATLIS\\SQLEXPRESS; Database = PictureAppDB; Trusted_Connection = True;";
            _userService = userService;
            _serverPath = @"C:\Users\RachelMatlis\source\repos\fsfinalclient_2\ImageMarkingSystem\ImageMarkingApp\src\assets\img\";
            _pathFrorClient = "assets/img/";
        }

        public CreateDocResponseDTO CreateDocument(DocumentDTO doc)
        {
            CreateDocResponseDTO retval = new CreateDocResponseDTO();
            retval.Success = true;
            retval.DocumentId = "0";

            if (_userService.isRegisterdUser(doc.UserId))
            {
                retval.DocumentId = insertDocToDB(doc);
            }

            if (retval.DocumentId != "0")
            {
                UploadFile(doc);

            }
            else
            {
                retval.Success = false;
            }
            return retval;
        }

        private void UploadFile(DocumentDTO doc)
        {
            string fileName = doc.DocumentName + ".jpg";
            if (doc.File != null)
            {
                byte[] bytes = Convert.FromBase64String(doc.File.Split(',')[1]);

                using (BinaryWriter writer = new BinaryWriter(System.IO.File.Open(_serverPath + fileName, FileMode.Create)))
                {
                    writer.Write(bytes);

                }
            }
        }

        public GetDocumentsResponseDTO GetAllDocuments(DocumentRequstDTO doc)
        {
            int i = 0;
            GetDocumentsResponseDTO retval = new GetDocumentsResponseDTO();
            retval.docList = new List<DocumentDTO>();
            var paramUserID = _dal.CreateParameter("userID", doc.UserId);
            var dataset = _dal.ExecuteQuery("GetUserDocs", paramUserID);
            foreach (DataTable table in dataset.Tables)
            {
                foreach (DataRow row in table.Rows)
                {
                    DocumentDTO docModel = new DocumentDTO();
                    docModel.DocumentId = (string)dataset.Tables[0].Rows[i]["docID"];
                    doc.DocumentId = docModel.DocumentId;
                    docModel = GetDocument(doc).Document;
                    retval.docList.Add(docModel);
                    i++;
                }
            }

            return retval;
        }

        public GetDocumentResponseDTO GetDocument(DocumentRequstDTO doc)
        {
            GetDocumentResponseDTO retval = new GetDocumentResponseDTO();
            retval.Document = new DocumentDTO();
            retval.Success = false;

            if (isDocExistInDB(doc.DocumentId) && (isUserOwnTheDoc(doc) || isUserSharedOnDocument(doc.DocumentId, doc.UserId)))
            {
                var paramDocId = _dal.CreateParameter("docID", doc.DocumentId);
                var dataset = _dal.ExecuteQuery("GetDocument", paramDocId);
                retval.Document.UserId = (string)dataset.Tables[0].Rows[0]["owner"];
                retval.Document.ImgUrl = (string)dataset.Tables[0].Rows[0]["imageURL"];
                retval.Document.DocumentName = (string)dataset.Tables[0].Rows[0]["documentName"];
                retval.Document.DocumentId = doc.DocumentId;
                retval.Success = true;
            }

            return retval;
        }

        private bool isUserSharedOnDocument(string docID, string userID)
        {
            var retval = false;
            var paramUserID = _dal.CreateParameter("userID", userID);
            var paramDocID = _dal.CreateParameter("docID", docID);
            var datset = _dal.ExecuteQuery("GetUserAndSharedDocument", paramUserID, paramDocID);

            if (datset.Tables[0].Rows.Count != 0)
            {
                retval = true;
            }

            return retval;
        }

        public BoolResponseDTO RemoveDocument(DocumentRequstDTO doc)
        {
            var retval = new BoolResponseDTO();
            retval.Success = false;
            if (isDocExistInDB(doc.DocumentId) && isUserOwnTheDoc(doc))
            {
                RemoveFile(doc);
                var paramDocId = _dal.CreateParameter("docID", doc.DocumentId);
                var datset = _dal.ExecuteQuery("RemoveDocument", paramDocId);
                if (!isDocExistInDB(doc.DocumentId))
                {
                    retval.Success = true;
                    if (isDocHasMarkers(doc.DocumentId))
                    {
                        removeDocFromDocumentMarkers(doc.DocumentId);
                    }
                    if (isDocIsShared(doc.DocumentId))
                    {
                        removeDocFromSharedDocs(doc.DocumentId);
                    }
                }

            }

            return retval;
        }

        private void RemoveFile(DocumentRequstDTO doc)
        {
            DocumentDTO document = GetDocument(doc).Document;
            string fileName = document.DocumentName + ".jpg";
            if (File.Exists(Path.Combine(_serverPath, fileName)))
            {
                File.Delete(Path.Combine(_serverPath, fileName));
            }
        }

        private string createDocumentID()
        {
            Random rnd = new Random();
            return (rnd.Next(1000, 100000000)).ToString();
        }

        private bool isDocExistInDB(string docId)
        {
            var retval = false;
            var paramDocId = _dal.CreateParameter("docID", docId);
            var dataset = _dal.ExecuteQuery("GetDocument", paramDocId);
            if (dataset.Tables[0].Rows.Count != 0)
                retval = true;
            return retval;
        }

        private string insertDocToDB(DocumentDTO doc)
        {
            string docId = createDocumentID();
            var paramDocId = _dal.CreateParameter("docID", docId);
            var paramUserId = _dal.CreateParameter("owner", doc.UserId);
            var paramDocName = _dal.CreateParameter("documentName", doc.DocumentName);
            doc.ImgUrl = _pathFrorClient + doc.DocumentName + ".jpg";
            var paramImgUrl = _dal.CreateParameter("imageURL", doc.ImgUrl);

            try
            {
                _dal.ExecuteNonQuery("CreateDocument", paramUserId, paramImgUrl, paramDocName, paramDocId);
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627) //Violation of primary key
                {
                    insertDocToDB(doc);
                }
                else
                {
                    docId = "0";
                }
            }

            return docId;
        }

        private void removeDocFromSharedDocs(string docID)
        {
            var paramDocID = _dal.CreateParameter("docID", docID);
            _dal.ExecuteNonQuery("RemoveShare", paramDocID);
        }
        private bool isDocIsShared(string docID)
        {
            var retval = true;
            var paramDocID = _dal.CreateParameter("docID", docID);
            var dataset = _dal.ExecuteQuery("GetSharedDoc", paramDocID);
            if (dataset.Tables[0].Rows.Count == 0)
                retval = false;
            return retval;

        }

        private void removeDocFromDocumentMarkers(string docID)
        {
            var paramDocID = _dal.CreateParameter("docID", docID);
            _dal.ExecuteNonQuery("RemoveDocFromDocumentMarkers", paramDocID);
        }

        private bool isDocHasMarkers(string docID)
        {
            var retval = true;
            var paramDocID = _dal.CreateParameter("docID", docID);
            var dataset = _dal.ExecuteQuery("GetDocFromDocumentMarkers", paramDocID);
            if (dataset.Tables[0].Rows.Count == 0)
                retval = false;
            return retval;

        }

        private bool isUserOwnTheDoc(DocumentRequstDTO doc)
        {
            var retval = true;
            var paramDocID = _dal.CreateParameter("docID", doc.DocumentId);
            var paramUserID = _dal.CreateParameter("userID", doc.UserId);
            var dataset = _dal.ExecuteQuery("GetUserDocument", paramDocID, paramUserID);
            if (dataset.Tables[0].Rows.Count == 0)
                retval = false;
            return retval;
        }
    }
}
