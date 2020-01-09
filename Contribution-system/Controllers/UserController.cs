using Contribution_system_Commond;
using Contribution_system_Models.Models;
using Contribution_system_Models.WebModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Security.Claims;

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
                var editorinfo = sqlConnect.Editors.FirstOrDefault(a => a.Editor_ID.Equals("aaa111"));
                var aaa = UserCommond.GetMD5Hash(editorinfo.Editor_Password);
                if (editorinfo!=null&&UserCommond.GetMD5Hash(editorinfo.Editor_Password).Equals(loginInfo.password))
                {
                    var token = UserCommond.SetToken(loginInfo.username,"Editor");
                    return Ok(token);
                }
                var authorinfo = sqlConnect.Authors.FirstOrDefault(b => b.Author_ID.Equals(loginInfo.username));
                if (authorinfo != null && UserCommond.GetMD5Hash(authorinfo.Author_Password).Equals(loginInfo.password))
                {
                    var token = UserCommond.SetToken(loginInfo.username,"Author");
                    return Ok(token);
                }
            }catch(Exception e)
            {
                Console.WriteLine(e);
            }
            return BadRequest();
        }

        /// <summary>
        /// 获取路由
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public string Login()
        {
            var Role = User.FindFirst(ClaimTypes.Role)?.Value;
            string text="";
            if (Role.Equals("Editor"))
                text = System.IO.File.ReadAllText(InfoPath.EditorRouterinfo);
            if (Role.Equals("Author"))
                text = System.IO.File.ReadAllText(InfoPath.AuthorRouterInfo);
            return text;
        }

        /// <summary>
        /// 获取权限信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("info")]
        [Authorize]
        public string getinfo()
        {
            var userid = User.FindFirst(ClaimTypes.Name)?.Value;
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            if (role.Equals("Author"))
            {
                var info = sqlConnect.Authors.FirstOrDefault(b => b.Author_ID.Equals(userid));
                UserRoleInfo userRole = new UserRoleInfo();
                userRole.id = userid;
                userRole.name = info.Author_Name;
                userRole.avatar = "/avatar2.jpg";
                Console.WriteLine(InfoPath.ModelsPath);
                userRole.role = JObject.Parse(System.IO.File.ReadAllText(InfoPath.AuthorRole));
                var s = JsonConvert.SerializeObject(userRole);
                return JsonConvert.SerializeObject(userRole);
            }
            if (role.Equals("Editor"))
            {
                var info = sqlConnect.Editors.FirstOrDefault(b => b.Editor_ID.Equals(userid));
                UserRoleInfo userRole = new UserRoleInfo();
                userRole.id = userid;
                userRole.name = info.Editor_Name;
                userRole.avatar = "/avatar2.jpg";
                Console.WriteLine(InfoPath.ModelsPath);
                userRole.role = JObject.Parse(System.IO.File.ReadAllText(InfoPath.AuthorRole));
                var s = JsonConvert.SerializeObject(userRole);
                return JsonConvert.SerializeObject(userRole);
            }
            else
                return "";
        }

    }
}