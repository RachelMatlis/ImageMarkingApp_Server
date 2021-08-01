using ImakeMarkingDTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageMarkingServiceContracs
{
    public interface IUserService
    {
        public BoolResponseDTO CrateUser(UserDTO user);
        public BoolResponseDTO Login(string userID);
        public BoolResponseDTO UnSubscribe(string userID);
        public bool isRegisterdUser(string userID);
    }
}
