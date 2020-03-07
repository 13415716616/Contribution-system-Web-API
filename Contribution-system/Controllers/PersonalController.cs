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
        [HttpPost("UpdateImg")]
        public IActionResult UpdateImg([FromForm] IFormFile file)
        {
            try { 
               return Ok();
            }catch(Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}