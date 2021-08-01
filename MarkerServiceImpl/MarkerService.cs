using DALContracts;
using ImageMarkingServiceContracs;
using ImakeMarkingDTO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace MarkerServiceImpl
{
    public class MarkerService : IMarkerService
    {
        private IDAL _dal;
        private IUserService _userService;
        public MarkerService(IDAL dal, IUserService userService)
        {
            _dal = dal;
            _dal.ConnectionStr = "Server = RACHELMATLIS\\SQLEXPRESS; Database = PictureAppDB; Trusted_Connection = True;";
            _userService = userService;
        }

        public CreateMarkerResponseDTO CreateMarker(MarkerDTO marker)
        {
            var retval = new CreateMarkerResponseDTO();
            retval.Success = true;
            retval.MarkerID = "0";

            if (isDocumentExist(marker.DocID) && _userService.isRegisterdUser(marker.UserID))
            {
                //if the owner of the document sent the marker *or* user is shared on document
                if (markerUserIDEqualsDocUserID(marker) || isUserSharedOnDocument(marker.DocID, marker.UserID))
                {
                    retval.MarkerID = insertMarkerToDB(marker);
                }
            }

            if (retval.MarkerID == "0")
                retval.Success = false;

            return retval;
        }

        public BoolResponseDTO RemoveMarker(string markerID)
        {
            var retval = new BoolResponseDTO();
            retval.Success = false;
            if (isMarkerExistInDB(markerID))
            {
                var paramMarkerID = _dal.CreateParameter("markerID", markerID);
                var datset = _dal.ExecuteQuery("RemoveMarker", paramMarkerID);
                if (!isMarkerExistInDB(markerID))
                    retval.Success = true;
            }

            return retval;
        }

        public BoolResponseDTO UpdateMarker(MarkerDTO marker)
        {
            var retval = new BoolResponseDTO();
            retval.Success = false;
            if (marker.MarkerID != null && isMarkerExistInDB(marker.MarkerID))
            {
                if (RemoveMarker(marker.MarkerID).Success == true)
                {
                    retval.Success = ((insertMarkerToDB(marker) != "0") ? true : false);
                }

            }

            return retval;
        }

        public GetMarkersResponseDTO GetMarkers(string documentID)
        {
            var retval = new GetMarkersResponseDTO();
            retval.Marker = new List<MarkerDTO>();
            var paramDocumentID = _dal.CreateParameter("docID", documentID);
            var datset = _dal.ExecuteQuery("GetMarkers", paramDocumentID);
            int tblSize = datset.Tables[0].Rows.Count;
            for (int i = 0; i < tblSize; i++)
            {
                var marker = new MarkerDTO();
                marker.DocID = documentID;
                marker.MarkerID = (string)datset.Tables[0].Rows[i]["markerID"];
                marker.MarkerType = (string)datset.Tables[0].Rows[i]["markerType"];
                marker.MarkerLocation = (string)datset.Tables[0].Rows[i]["markerLocation"];
                marker.MarkerBackColor = (string)datset.Tables[0].Rows[i]["backColor"];
                marker.UserID = (string)datset.Tables[0].Rows[i]["userID"];
                retval.Marker.Add(marker);
            }

            return retval;

        }

        private string createMarkerID()
        {
            Random rnd = new Random();
            return (rnd.Next(1000, 100000000)).ToString();
        }

        private bool isMarkerExistInDB(string markerId)
        {
            var retval = true;
            var paramMarkerID = _dal.CreateParameter("markerID", markerId);
            var datset = _dal.ExecuteQuery("GetMarker", paramMarkerID);

            if (datset.Tables[0].Rows.Count == 0)
                retval = false;

            return retval;
        }

        private bool isDocumentExist(string docID)
        {
            var retval = true;
            var paramDocID = _dal.CreateParameter("docID", docID);
            var datset = _dal.ExecuteQuery("GetDocument", paramDocID);

            if (datset.Tables[0].Rows.Count == 0)
                retval = false;
            return retval;
        }

        private string getDocumentUserID(string docID)
        {
            var paramDocID = _dal.CreateParameter("docID", docID);
            var dataset = _dal.ExecuteQuery("GetDocument", paramDocID);
            string owner = (string)dataset.Tables[0].Rows[0]["owner"];
            return owner;
        }

        private string insertMarkerToDB(MarkerDTO marker)
        {
            string markerID = createMarkerID();
            var paramMarkerID = _dal.CreateParameter("markerID", markerID);
            var paramDocID = _dal.CreateParameter("docID", marker.DocID);
            var paramMarkerType = _dal.CreateParameter("markerType", marker.MarkerType);
            var paramMarkerLocation = _dal.CreateParameter("markerLocation", marker.MarkerLocation);
            var paramMarkerBackColor = _dal.CreateParameter("backColor", marker.MarkerBackColor);
            var paramUserID = _dal.CreateParameter("userID", marker.UserID);

            try
            {
                _dal.ExecuteNonQuery("CreateMarker", paramUserID, paramDocID, paramMarkerID, paramMarkerType,
                    paramMarkerLocation, paramMarkerBackColor);
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627) //Violation of primary key
                {
                    insertMarkerToDB(marker);
                }
                else
                {
                    markerID = "0";
                }
            }

            return markerID;
        }

        private bool markerUserIDEqualsDocUserID(MarkerDTO marker)
        {
            string markerUserID = marker.UserID.Trim().ToLower();
            string documentUserID = getDocumentUserID(marker.DocID).Trim().ToLower();
            return String.Equals(markerUserID, documentUserID);
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

    }
}
