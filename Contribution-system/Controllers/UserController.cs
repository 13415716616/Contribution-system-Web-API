using Contribution_system_Commond.Page;
using Contribution_system_Models;
using Contribution_system_Models.Models;
using Contribution_system_Models.WebModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;

namespace Contribution_system.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        //注册用户信息
        [HttpPost("Register")]
        public IActionResult Register([FromBody] Author author)
        {
            if (UserPageApi.AddAuthor(author))
                return Ok();
            else
                return BadRequest();
        }

        //获取登录的Token
        [HttpPost("Login")]
        public IActionResult Login([FromBody] LoginInfo loginInfo)
        {
            var token = UserPageApi.GetLoginRoleToken(loginInfo);
            if(token!="")
                return Ok(token);
            else
                return BadRequest();
        }

        //获取前端所用的路由表
        [HttpGet]
        [Authorize]
        public string Login() 
        {
            var Role = User.FindFirst(ClaimTypes.Role)?.Value;
            return UserPageApi.GetLoginRoleRoutor(Role);           
        }

        //获取用户前端所需要显示的信息
        [HttpGet("info")]
        [Authorize]
        public string getinfo()
        {
            var userid = User.FindFirst(ClaimTypes.Name)?.Value;
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            return UserPageApi.GetLoginInfo(userid, role);
        }       
    }
}