using DALContracts;
using ImageMarkingServiceContracs;
using ImakeMarkingDTO;
using System;
using System.Collections.Generic;

namespace SharingServiceImpl
{
    public class SharingService: ISharingService
    {
        IDAL _dal;
        private IUserService _userService;
        public SharingService(IDAL dal, IUserService userService)
        {
            _dal = dal;
            _dal.ConnectionStr = "Server = RACHELMATLIS\\SQLEXPRESS; Database = PictureAppDB; Trusted_Connection = True;";
            _userService = userService;
        }

        public BoolResponseDTO ShareOnDocument(ShareDocDTO share)
        {
            BoolResponseDTO retval = new BoolResponseDTO();
            retval.Success = false;

            if (_userService.isRegisterdUser(share.UserID) && isUserOwnTheDoc(share))
            {
                if(_userService.isRegisterdUser(share.SharedUserID))
                {
                    retval.Success = CreateShare(share);
                }
            }

            return retval;
        }


        public BoolResponseDTO RemoveShareOnDocument(ShareDocDTO share)
        {
            BoolResponseDTO retval = new BoolResponseDTO();
            retval.Success = false;

            if (_userService.isRegisterdUser(share.UserID) && isUserOwnTheDoc(share))
            {
                var paramDocID = _dal.CreateParameter("docID", share.DocumentID);
                var dataset = _dal.ExecuteQuery("RemoveShare", paramDocID);
                if (!(isDocShared(share)))
                    retval.Success = true;
            }

            return retval;
        }

        public GetSharedDocumentsResponseDTO GetSharedDocuments(string userID)
        {
            var retval = new GetSharedDocumentsResponseDTO();
            retval.SharedDocsList = new List<DocumentDTO>();
            var paramUserID = _dal.CreateParameter("userID", userID);
            var dataset = _dal.ExecuteQuery("GetUserSharedDocuments", paramUserID);
            int tblSize = dataset.Tables[0].Rows.Count;
            for (int i = 0; i < tblSize; i++)
            {
                var docID = (string)dataset.Tables[0].Rows[i]["docID"];
                retval.SharedDocsList.Add(getDocument(docID));
            }
           
            return retval;
        }

        public DocumentDTO getDocument(string docID)
        {
            var retval = new DocumentDTO();
            var paramDocId = _dal.CreateParameter("docID", docID);
            var dataset = _dal.ExecuteQuery("GetDocument", paramDocId);
            retval.UserId = (string)dataset.Tables[0].Rows[0]["owner"];
            retval.ImgUrl = (string)dataset.Tables[0].Rows[0]["imageURL"];
            retval.DocumentName = (string)dataset.Tables[0].Rows[0]["documentName"];
            retval.DocumentId = docID;
            return retval;
        }

        private bool CreateShare(ShareDocDTO share)
        {
            var retval = false;
            var paramDocID = _dal.CreateParameter("docID", share.DocumentID);
            var paramUserID = _dal.CreateParameter("userID", share.SharedUserID);
            var dataset = _dal.ExecuteQuery("CreateShare", paramDocID, paramUserID);
            if (isUserSharedOnDoc(share))
                retval = true;
            return retval;
        }
        private bool isUserOwnTheDoc(ShareDocDTO share)
        {
            var retval = true;
            var paramDocID = _dal.CreateParameter("docID", share.DocumentID);
            var paramUserID = _dal.CreateParameter("userID", share.UserID);
            var dataset = _dal.ExecuteQuery("GetUserDocument", paramDocID, paramUserID);
            if (dataset.Tables[0].Rows.Count == 0)
                retval = false;
            return retval;
        }

        private bool isUserSharedOnDoc(ShareDocDTO share)
        {
            var retval = true;
            var paramDocID = _dal.CreateParameter("docID", share.DocumentID);
            var paramUserID = _dal.CreateParameter("userID", share.SharedUserID);
            var dataset = _dal.ExecuteQuery("GetUserAndSharedDocument", paramDocID, paramUserID);
            if (dataset.Tables[0].Rows.Count == 0)
                retval = false;
            return retval;
        }

        private bool isDocShared(ShareDocDTO share)
        {
            var retval = true;
            var paramDocID = _dal.CreateParameter("docID", share.DocumentID);
            var dataset = _dal.ExecuteQuery("GetSharedDoc", paramDocID);
            if (dataset.Tables[0].Rows.Count == 0)
                retval = false;
            return retval;
        }


    }
}
