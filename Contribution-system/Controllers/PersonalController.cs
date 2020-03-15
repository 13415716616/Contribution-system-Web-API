using System;
using System.Linq;
using System.Security.Claims;
using Contribution_system_Commond.Page;
using Contribution_system_Models;
using Contribution_system_Models.WebModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Contribution_system_Models.Models;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace Contribution_system.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonalController : ControllerBase
    {
        //获取作者的个人信息
        [HttpGet]
        [Authorize]
        public IActionResult GetAuthorPersonalInfo()
        {
            try
            {
                var id = User.FindFirst(ClaimTypes.Name)?.Value;
                return Ok(PersonalPageAPI.GetAuthorInfo(id));
            }
            catch
            {
                return BadRequest();
            }
        }

        //添加作者的Tags标签函数
        [HttpGet("AddAuthorTags")]
        [Authorize]
        public IActionResult AddAuthorTags(string tags)
        {
            try
            {
                var id = User.FindFirst(ClaimTypes.Name)?.Value;
                PersonalPageAPI.AddAuthorTags(tags, id);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("GetCompleteManuscript")]
        public IActionResult GetCompleteManuscript()
        {
            var id= User.FindFirst(ClaimTypes.Name)?.Value;
            var list= PersonalPageAPI.GetCompleteManuscript(id);
            return Ok(list);
        }

        //获取稿件个数
        [HttpGet("GetAuthorManuscriptNum")]
        [Authorize]
        public IActionResult GetAuthorManuscriptNum()
        {
            var id = User.FindFirst((ClaimTypes.Role))?.Value;
            return Ok(PersonalPageAPI.GetAuthorManuscriptNumNum(id));
        }
        
        //获取修改的信息
        [HttpPost("UpdateAuthorInfo")]
        public IActionResult UpdateAuthorInfo([FromBody] Author author)
        {
            try
            {
                SqlConnect sqlConnect = new SqlConnect();
                var id = User.FindFirst(ClaimTypes.Name)?.Value;
                var info = sqlConnect.Authors.FirstOrDefault(b => b.Author_ID == id);
                info.Author_Name = author.Author_Name;
                info.Author_Address = author.Author_Address;
                info.Author_Dec = author.Author_Dec;
                info.Author_Email = author.Author_Email;
                info.Author_Phone = author.Author_Phone;
                sqlConnect.Authors.Update(info);
                sqlConnect.SaveChanges();
                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest(e);
            }
        }

        //上传角色头像函数
        [HttpPost("UpdateEditorImg")]
        [Authorize]
        public IActionResult UpdateImg([FromForm] IFormFile file)
        {
            SqlConnect sqlConnect = new SqlConnect();
            var fpath = InfoPath.FilePath + "/wwwroot/File/Image/";
            if (!Directory.Exists(fpath))
            {
                Directory.CreateDirectory(fpath);
            }
            FileStream stream = new FileStream(InfoPath.FilePath + "/wwwroot/File/Image/" + file.FileName, FileMode.Create);
            file.CopyTo(stream);
            var id = User.FindFirst(ClaimTypes.Name)?.Value;
            var info = sqlConnect.Editors.FirstOrDefault(b => b.Editor_ID == id);
            info.Editor_avtor =  "/File/Image/" + file.FileName;
            sqlConnect.Update(info);
            sqlConnect.SaveChanges();
            return Ok(info.Editor_avtor);
        }

        // 获取编辑的个人信息
        [HttpGet("GetEditorPersonalInfo")]
        [Authorize]
        public IActionResult GetEditorPersonalInfo()
        {
            SqlConnect sqlConnect = new SqlConnect();
            var id = User.FindFirst(ClaimTypes.Name)?.Value;
            var info = sqlConnect.Editors.FirstOrDefault(b => b.Editor_ID == id);
            return Ok(info);
        }

        [HttpPost("UpdateEditorInfo")]
        public IActionResult UpdateEditorInfo([FromBody] Editor editor)
        {
            SqlConnect sqlConnect = new SqlConnect();
            var info= sqlConnect.Editors.FirstOrDefault(b => b.Editor_ID == editor.Editor_ID);
            info.Editor_Name = editor.Editor_Name;
            info.Editor_Sex = editor.Editor_Sex;
            info.Editor_Phone = editor.Editor_Phone;
            info.Editor_Email = editor.Editor_Email;
            info.Editor_Education = editor.Editor_Education;
            info.Editor_Dec = editor.Editor_Dec;
            sqlConnect.Update(info);
            sqlConnect.SaveChanges();
            return Ok();
        }
    }
}