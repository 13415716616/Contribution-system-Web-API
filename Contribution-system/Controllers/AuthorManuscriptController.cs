using Contribution_system_Commond.Page;
using Contribution_system_Models;
using Contribution_system_Models.Models;
using Contribution_system_Models.WebModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;

namespace Contribution_system.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorManuscriptController : ControllerBase
    {
        SqlConnect sqlConnect;

        public AuthorManuscriptController(SqlConnect sqlConnect)
        {
            this.sqlConnect = sqlConnect;
        }


        //获取栏目信息
        [HttpGet("GetManuscriptColumn")]
        public IActionResult GetManuscriptColumn()
        {
            var info = sqlConnect.ManuscriptColumn.ToList();
            return Ok(info);
        }

        //添加新的稿件信息
        [HttpPost]
        [Authorize]
        public IActionResult AddManuscript([FromBody] Manuscript manuscript)
        {
            string author_id = manuscript.Author_ID = User.FindFirst(ClaimTypes.Name)?.Value;
            var id = AuthorManuscriptPageAPI.AddNewManuscript(manuscript, author_id);
            if (id != 0)
            {
                return Ok(id);
            }
            else
                return BadRequest();
        }

        //上传主要的稿件
        [HttpPost("uploadmain")]
        [Authorize]
        public IActionResult UploadDataMain([FromForm] IFormFile file)
        {
            var a = int.Parse(Request.Headers["ManuscriptID"]);
            var userid = User.FindFirst(ClaimTypes.Name)?.Value;
            if (AuthorManuscriptPageAPI.AddMainManuscriptUpload(file, a))
                return Ok();
            else
                return BadRequest();
        }


        [HttpPost("AddManuscriptAuthor")]
        [Authorize]
        public IActionResult AddManuscriptAuthor([FromBody] Contribution_system_Models.Models.ManuscriptAuthor author)
        {
            sqlConnect.ManuscriptAuthor.Add(author);
            sqlConnect.SaveChanges();
            return Ok();
        }

        [HttpGet("GetManuscript")]
        public IActionResult GetManuscript(int id)
        {
            var info = sqlConnect.Manuscript.FirstOrDefault(b => b.Manuscript_ID == id);
            return Ok(info);
        }

        // 获取草稿箱稿件的信息
        [HttpGet("ManuscriptToDrafts")]
        public IActionResult GetManscriptInfo()
        {
            string author_id = User.FindFirst(ClaimTypes.Name)?.Value;
            var info = sqlConnect.Manuscript.Where(b => b.Author_ID.Equals(author_id) && b.Manuscript_Status.Equals("稿件编辑中"));
            return Ok(info);
        }
        //更改草稿箱中的稿件信息
        //[HttpGet("ManuscriptTos")]
        //[Authorize]
        //public IActionResult ManuscriptTos()
        //{
        //    try { 
        //        var userid = User.FindFirst(ClaimTypes.Name)?.Value;
        //        var manuscripts = sqlConnect.Manuscript.Where(b => b.Author_ID.Equals(userid)).ToList();
        //        return Ok(manuscripts);
        //    }catch(Exception e)
        //    {
        //        Console.Write(e);
        //        return BadRequest();
        //    }
        //}



        [HttpGet("GetFile")]
        public IActionResult GetFile(int id)
        {
            var info = sqlConnect.ManuscriptReview.FirstOrDefault(b => b.ManuscriptReview_ID == id);
            var provider = new FileExtensionContentTypeProvider();
            var file = InfoPath.ModelsPath + info.ManuscriptReview_MainDataPath;
            var fileName = Path.GetFileName(file);
            var ext = Path.GetExtension(fileName);
            var stream = System.IO.File.OpenRead(file);
            var contentType = provider.Mappings[ext];
            return File(stream, contentType, fileName);
        }

        [HttpGet("GetCompleteManuscrit")]
        public IActionResult GetCompleteManuscrit()
        {
            var info = sqlConnect.CompleteManuscript.ToList();
            return Ok(info);
        }

        [HttpPost("CompleteManuscript")]
        public IActionResult CompleteManuscript([FromBody] Manuscript manuscript)
        {
            SqlConnect sqlConnect = new SqlConnect();
            var info = sqlConnect.Manuscript.FirstOrDefault(b => b.Manuscript_ID == manuscript.Manuscript_ID);
            info.Manuscript_Status = "等待编辑审查";
            sqlConnect.Update(manuscript);
            sqlConnect.SaveChanges();
            return Ok();
        }

        [HttpGet("DeleteMansuscriptDrafts")]
        public IActionResult DeleteMansuscriptDrafts(int id)
        {
            var minfo = sqlConnect.Manuscript.FirstOrDefault(b => b.Manuscript_ID == id);
            sqlConnect.Remove(minfo);
            sqlConnect.SaveChanges();
            var author = sqlConnect.ManuscriptAuthor.Where(b => b.Manuscript_ID == id).ToList();
            sqlConnect.Remove(author);
            sqlConnect.SaveChanges();
            return Ok();
        }
    }
}
