using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Contribution_system_Models;
using Contribution_system_Models.Models;
using Contribution_system_Models.WebModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Contribution_system.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorPersonalController : ControllerBase
    {
        [HttpPost("UpdateAuthorImg")]
        [Authorize]
        public IActionResult UpdateAuthorImg([FromForm] IFormFile file)
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
            var info = sqlConnect.Authors.FirstOrDefault(b => b.Author_ID == id);
            info.Author_Avtor = "/File/Image/" + file.FileName;
            sqlConnect.Update(info);
            sqlConnect.SaveChanges();
            return Ok();
        }

        [HttpGet("GetAuthorInfo")]
        [Authorize]
        public IActionResult GetAuthorInfo()
        {
            SqlConnect sqlConnect = new SqlConnect();
            var id = User.FindFirst(ClaimTypes.Name)?.Value;
            var info = sqlConnect.Authors.FirstOrDefault(b => b.Author_ID == id);
            return Ok(info);
        }

        [HttpPost("UpdateAuthorInfo")]
        [Authorize]
        public IActionResult UpdateAuthorInfo([FromBody] Author author)
        {
            SqlConnect sqlConnect = new SqlConnect();
            var info = sqlConnect.Authors.FirstOrDefault(b => b.Author_ID == author.Author_ID);
            info.Author_Name = author.Author_Name;
            info.Author_Education = author.Author_Education;
            info.Author_Email = author.Author_Email;
            info.Author_Phone = author.Author_Phone;
            info.Author_Address = author.Author_Address;
            info.Author_Dec = author.Author_Dec;
            sqlConnect.Update(info);
            sqlConnect.SaveChanges();
            return Ok();
        }

        [HttpPost("SentMessage")]
        public IActionResult SentMessage([FromBody] Message message)
        { 
            var id = User.FindFirst(ClaimTypes.Name)?.Value;
            message.Message_Sender = id;
            message.Message_Time = DateTime.Now.ToString();
            SqlConnect sqlConnect = new SqlConnect();
            sqlConnect.Message.Add(message);
            sqlConnect.SaveChanges();
            return Ok();
        }

        [HttpGet("GetAllMessage")]
        public IActionResult GetAllMessage()
        {
            var id= User.FindFirst(ClaimTypes.Name)?.Value;
            SqlConnect sqlConnect = new SqlConnect();
            var info = sqlConnect.Message.Where(b => b.Message_Recipient == id);
            return Ok(info);
        }
    }
}