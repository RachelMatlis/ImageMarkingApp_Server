using ImageMarkingServiceContracs;
using ImakeMarkingDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageMarkingApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // POST api/<UserController>
        [HttpPost("Register")]
        public BoolResponseDTO Register([FromBody] UserDTO user)
        {
            var retval = _userService.CrateUser(user);
            return retval;
        }

        [HttpPost("Login")]
        public BoolResponseDTO Login([FromBody] UserDTO user)
        {
            var retval = _userService.Login(user.UserID);
            return retval;
        }

        [HttpPost("UnSubscribe")]
        public BoolResponseDTO UnSubscribe([FromBody] UserDTO user)
        {
            var retval = _userService.UnSubscribe(user.UserID);
            return retval;
        }
    }
}
