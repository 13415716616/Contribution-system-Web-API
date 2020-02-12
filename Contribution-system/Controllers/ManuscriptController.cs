using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Contribution_system_Models.Models;
using Microsoft.AspNetCore.Authorization;
using static Contribution_system_Models.WebModel.ManuscriptModel;
using System.Security.Claims;
using Contribution_system_Models.WebModel;
using System.IO;
using Microsoft.Extensions.Primitives;

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

        [HttpPost]
        [Authorize]
        public IActionResult AddManuscript(Manuscript manuscript)
        {
            try { 
                manuscript.Manuscript_Status = ManuscriptMode.WriteInfo;
                manuscript.Author_ID = User.FindFirst(ClaimTypes.Name)?.Value;
                sqlConnect.Manuscript.Add(manuscript);
                sqlConnect.SaveChanges();
                return Ok(manuscript.Manuscript_ID);
            }catch(Exception e)
            {
                Console.WriteLine(e);
                return BadRequest();
            }
        }

        [HttpPost("uploadmain")]
        [Authorize]
        public IActionResult UploadDataMain([FromForm] IFormFile file)
        {
            //StringValues value1;
            //Request.Headers.TryGetValue("ManuscriptID", out value1);
            try { 
                var a =int.Parse(Request.Headers["ManuscriptID"]);
                var manuscript = sqlConnect.Manuscript.FirstOrDefault(b => b.Manuscript_ID==a);
                var userid = User.FindFirst(ClaimTypes.Name)?.Value;
                var fpath = InfoPath.ModelsPath + "wwwroot\\File\\Manuscript\\" + userid;           
                if (!Directory.Exists(fpath))
                {
                    Directory.CreateDirectory(fpath);
                }
                var spath=fpath+ "\\" + a;
                if (!Directory.Exists(spath))
                {
                    Directory.CreateDirectory(spath);
                }
                var mpath = spath + "\\主要稿件\\";
                if (!Directory.Exists(mpath))
                {
                    Directory.CreateDirectory(mpath);
                }
                FileStream stream = new FileStream(InfoPath.ModelsPath+"wwwroot\\File\\Manuscript\\" + userid + "\\"+a+ "\\主要稿件\\" + file.FileName, FileMode.Create);
                file.CopyTo(stream);
                manuscript.Manuscript_MainDataPath = "wwwroot\\File\\Manuscript\\" + userid + "\\" + a + "\\主要稿件\\" + file.FileName;
                manuscript.Manuscript_Status = ManuscriptMode.UploadFile;
                sqlConnect.Update(manuscript);
                sqlConnect.SaveChanges();
                return Ok();
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
            return BadRequest();
        }

        [HttpPost("uploadother")]
        [Authorize]
        public IActionResult UploadDataOther([FromForm] IFormFile file)
        {
            //StringValues value1;
            //Request.Headers.TryGetValue("ManuscriptID", out value1);
            try
            {
                var a = int.Parse(Request.Headers["ManuscriptID"]);
                var manuscript = sqlConnect.Manuscript.FirstOrDefault(b => b.Manuscript_ID == a);
                var userid = User.FindFirst(ClaimTypes.Name)?.Value;
                var fpath = InfoPath.ModelsPath + "wwwroot\\File\\Manuscript\\" + userid;
                if (!Directory.Exists(fpath))
                {
                    Directory.CreateDirectory(fpath);
                }
                var spath = fpath + "\\" + a;
                if (!Directory.Exists(spath))
                {
                    Directory.CreateDirectory(spath);
                }
                var mpath = spath + "\\其他资料\\";
                if (!Directory.Exists(mpath))
                {
                    Directory.CreateDirectory(mpath);
                }
                FileStream stream = new FileStream(InfoPath.ModelsPath + "wwwroot\\File\\Manuscript\\" + userid + "\\" + a + "\\其他资料\\" + file.FileName, FileMode.Create);
                file.CopyTo(stream);
                manuscript.Manuscript_OtherDataPath = "wwwroot\\File\\Manuscript\\" + userid + "\\" + a + "\\其他资料\\" + file.FileName;
                manuscript.Manuscript_Status = ManuscriptMode.UploadFile;
                sqlConnect.Update(manuscript);
                sqlConnect.SaveChanges();
                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return BadRequest();
        }

        [HttpPost("AddAuthor")]
        public IActionResult AddaAuthor([FromBody] Contribution_system_Models.WebModel.ManuscriptAuthor author)
        {
            try
            {
                var info = sqlConnect.Manuscript.FirstOrDefault(b => b.Manuscript_ID.Equals(author.Manscript_ID));
                info.Author_name = author.Author_name;
                info.Author_sex = author.Author_sex;
                info.Author_Phone = author.Author_Phone;
                info.Author_Address = author.Author_Address;
                info.Author_dec = author.Author_dec;
                info.Manuscript_Status = ManuscriptMode.Complete;
                sqlConnect.Update(info);
                sqlConnect.SaveChanges();
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
                sqlConnect.ManuscriptReview.Add(review);
                sqlConnect.SaveChanges();
                return Ok();
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
            return BadRequest();
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
    }
}