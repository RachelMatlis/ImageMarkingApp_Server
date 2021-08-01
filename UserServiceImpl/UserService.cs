using DALContracts;
using ImageMarkingServiceContracs;
using ImakeMarkingDTO;
using System;

namespace UserServiceImpl
{
    public class UserService : IUserService
    {
        IDAL _dal;
        public UserService(IDAL dal)
        {
            _dal = dal;
            _dal.ConnectionStr = "Server = RACHELMATLIS\\SQLEXPRESS; Database = PictureAppDB; Trusted_Connection = True;";
        }
        public BoolResponseDTO CrateUser(UserDTO user)
        {
            var retval = new BoolResponseDTO();
            retval.Success = false;

            if (isRegisterdUser(user.UserID) == false)
            {
                var paramUserID = _dal.CreateParameter("userID", user.UserID);
                var paramUserName = _dal.CreateParameter("userName", user.UserName);
                _dal.ExecuteNonQuery("CreateUser", paramUserID, paramUserName);
                if (isRegisterdUser(user.UserID))
                    retval.Success = true;
            }

            return retval;
        }

        public BoolResponseDTO Login(string userID)
        {
            var retval = new BoolResponseDTO();
            retval.Success = isRegisterdUser(userID);
            return retval;
        }

        public bool isRegisterdUser(string userID)
        {
            var retval = true;
            var paramUserID = _dal.CreateParameter("userID", userID);
            var datset = _dal.ExecuteQuery("GetUser", paramUserID);

            if (datset.Tables[0].Rows.Count == 0)
                retval = false;

            return retval;
        }

        public BoolResponseDTO UnSubscribe(string userID)
        {
            var retval = new BoolResponseDTO();
            retval.Success = false;
            var paramUserID = _dal.CreateParameter("userID", userID);
            var datset = _dal.ExecuteQuery("UnSubscribeUser", paramUserID);
            if (isUserUnSubscribe(userID))
                retval.Success = true;
            return retval;
        }

        private bool isUserUnSubscribe(string userID)
        {
            var paramUserID = _dal.CreateParameter("userID", userID);
            var datset = _dal.ExecuteQuery("GetUser", paramUserID);
            return ((bool)datset.Tables[0].Rows[0]["status"] == false);
        }
    }
}

