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
    public class ManuscriptController : ControllerBase
    {
        SqlConnect sqlConnect;

        public ManuscriptController(SqlConnect sqlConnect)
        {
            this.sqlConnect = sqlConnect;
        }

        //添加新的稿件信息
        [HttpPost]
        [Authorize]
        public IActionResult AddManuscript(Manuscript manuscript)
        {
            string author_id=manuscript.Author_ID = User.FindFirst(ClaimTypes.Name)?.Value;
            if (AuthorManuscriptPageAPI.AddNewManuscript(manuscript, author_id))
            {
                return Ok();
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
            if (AuthorManuscriptPageAPI.AddMainManuscriptUpload(file, a, userid))
                return Ok();
            else
                return BadRequest();
        }

        [HttpPost("uploadother")]
        [Authorize]
        public IActionResult UploadDataOther([FromForm] IFormFile file)
        {
            var a = int.Parse(Request.Headers["ManuscriptID"]);
            var manuscript = sqlConnect.Manuscript.FirstOrDefault(b => b.Manuscript_ID == a);
            var userid = User.FindFirst(ClaimTypes.Name)?.Value;
            if (AuthorManuscriptPageAPI.AddOtherManuscriptUpload(file, a, userid))
                return Ok();
            else
                return BadRequest();
        }

        //给稿件添加作者信息
        [HttpPost("AddAuthor")]
        public IActionResult AddaAuthor([FromBody] Contribution_system_Models.WebModel.ManuscriptAuthor author)
        {
            if (AuthorManuscriptPageAPI.AddManuscriptAuthor(author))
                return Ok();
            else return BadRequest();
        }

        /// <summary>
        /// 获取草稿箱稿件的信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetManscriptInfo(int id)
        {
            var info = sqlConnect.Manuscript.FirstOrDefault(b => b.Manuscript_ID.Equals(id));
            return Ok(info);
        }

        [HttpGet("ManuscriptToDrafts")]
        [Authorize]
        public IActionResult ManuscriptToDrafts()
        {
            var userid = User.FindFirst(ClaimTypes.Name)?.Value;
            List<Manuscript> manuscripts = sqlConnect.Manuscript.Where(b=>b.Author_ID.Equals(userid)).ToList();
            return Ok(manuscripts);
        }

        /// <summary>
        /// 删除草稿箱中的稿件
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("DeleteMansuscriptDrafts")]
        public IActionResult DeleteMansuscriptDrafts(int id)
        {
            try
            {
                var a=sqlConnect.Manuscript.First(sss => sss.Manuscript_ID.Equals(id));
                sqlConnect.Manuscript.Remove(a);
                sqlConnect.SaveChanges();
                return Ok();
            }catch(Exception e)
            {
                Console.WriteLine(e);
                return BadRequest();
            }
        }

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

        [HttpGet("CompleteDratfs")]
        public IActionResult CompleteDratfs(int id){
            var info = sqlConnect.Manuscript.FirstOrDefault(b => b.Manuscript_ID == id);
            ManuscriptReview review = new ManuscriptReview();
            review.ManuscriptReview_Title = info.Manuscript_Title;
            review.ManuscriptReview_Etitle = info.Manuscript_Etitle;
            review.ManuscriptReview_Keyword = info.Manuscript_Keyword;
            review.ManuscriptReview_Reference = info.Manuscript_Reference;
            review.ManuscriptReview_Abstract = info.Manuscript_Abstract;
            review.ManuscriptReview_Text = info.Manuscript_Text;
            review.ManuscriptReview_MainDataPath = info.Manuscript_MainDataPath;
            review.ManuscriptReview_OtherDataPath = info.Manuscript_OtherDataPath;
            review.Author_name = info.Author_name;
            review.Author_Phone = info.Author_Phone;
            review.Author_sex = info.Author_sex;
            review.Author_Address = info.Author_Address;
            review.Author_dec = info.Author_dec;
            review.Author_ID = info.Author_ID;
            review.ManuscriptReview_Status = "等待编辑审查";
            review.ManuscriptReview_Time = DateTime.Now.ToString();
            sqlConnect.ManuscriptReview.Add(review);
            sqlConnect.Manuscript.Remove(info);
            sqlConnect.SaveChanges();
            return Ok();
        }
    }
}