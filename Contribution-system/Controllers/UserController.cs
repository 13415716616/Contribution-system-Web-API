using Contribution_system_Commond;
using Contribution_system_Models.Models;
using Contribution_system_Models.WebModel;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace Contribution_system.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        SqlConnect sqlConnect;

        public UserController(SqlConnect sqlConnect)
        {
            this.sqlConnect = sqlConnect;
        }

        /// <summary>
        /// 用户注册Web API，Post方法
        /// </summary>
        /// <param name="author"></param>
        /// <returns></returns>
        [HttpPost("Register")]
        public IActionResult Register([FromBody] Author author)
        {
            try
            {
                sqlConnect.Authors.Add(author);
                sqlConnect.SaveChanges();
                return Ok();
            }catch(Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("注册账号出错：");
                Console.WriteLine(e);
                Console.ForegroundColor = ConsoleColor.White;
                return BadRequest();
            }
        }

        /// <summary>
        /// 首次验证登录名和密码，返回Token
        /// </summary>
        /// <param name="loginInfo"></param>
        /// <returns></returns>
        [HttpPost("Login")]
        public IActionResult Login([FromBody] LoginInfo loginInfo)
        {
            try
            {
                var info = sqlConnect.Authors.FirstOrDefault(b => b.Author_ID.Equals(loginInfo.username));
                if (UserCommond.GetMD5Hash(info.Author_Password).Equals(loginInfo.password))
                {
                    var token = UserCommond.SetToken(loginInfo.username);
                    return Ok(token);
                }
            }catch(Exception e)
            {
                Console.WriteLine(e);
            }
            return BadRequest();
        }

        [HttpGet]
        public string Login()
        {
            string text = System.IO.File.ReadAllText(InfoPath.AuthorRouterInfo);
            return text;
        }

        [HttpGet("info")]
        public string getinfo()
        {
            UserRoleInfo userRole = new UserRoleInfo();
            userRole.id = "4291d7da9005377ec9aec4a71ea837f";
            userRole.name = "天野远子";
            userRole.avatar = "/avatar2.jpg";
            Console.WriteLine(InfoPath.ModelsPath);
            userRole.role =JObject.Parse(System.IO.File.ReadAllText(InfoPath.AuthorRole));
            var s= JsonConvert.SerializeObject(userRole);
            return JsonConvert.SerializeObject(userRole);
        }

    }
}