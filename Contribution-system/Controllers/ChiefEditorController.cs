using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Contribution_system_Models;
using Contribution_system_Models.Models;
using Contribution_system_Models.WebModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Contribution_system.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChiefEditorController : ControllerBase
    {
        SqlConnect sqlConnect;

        public ChiefEditorController(SqlConnect sqlConnect)
        {
            this.sqlConnect = sqlConnect;
        }       

        [HttpGet("GetReviewManuscript")]
        public IActionResult GetReviewManuscript()
        {
            var id= User.FindFirst(ClaimTypes.Name)?.Value;
            var info = sqlConnect.ManuscriptReview.Where(b => b.ChiefEditor_ID == id && b.ManuscriptReview_Status == "主编审查中");
            return Ok(info);
        }

        [HttpGet("GetChiefEditorManuscript")]
        public IActionResult GetChiefEditorManuscript()
        {
            List<Manuscript> manuscripts = sqlConnect.Manuscript.Where(b => b.Manuscript_Status == "等待主编审查").ToList();
            return Ok(manuscripts);
        }

        [HttpGet("GetExpertReviewInfo")]
        public IActionResult GetExpertReviewInfo(int id)
        {
            var info = sqlConnect.ExpertReview.FirstOrDefault(b => b.Manuscript_ID == id);
            return Ok(info);
        }

        [HttpPost("CompleteManuscript")]
        public IActionResult CompleteManuscript([FromBody] EditorReview review)
        {
            review.Editor_Type = "主编终审";
            review.Review_Time = DateTime.Now.ToString();
            review.Editor_ID= User.FindFirst(ClaimTypes.Name)?.Value;
            var info = sqlConnect.Manuscript.FirstOrDefault(b => b.Manuscript_ID == review.Manuscript_ID);
            info.Manuscript_Status = "采纳稿件";
            Layout layout = new Layout();
            layout.Manuscript_ID = review.Manuscript_ID;
            layout.Layout_Image = "/Layout/Manuscript/timg.jpg";
            sqlConnect.Layout.Add(layout);
            sqlConnect.Update(info);
            sqlConnect.EditorReview.Add(review);
            sqlConnect.SaveChanges();
            return Ok();
        }

        [HttpGet("GetCompleteManuscript")]
        public IActionResult GetCompleteManuscript()
        {
            var info= sqlConnect.Manuscript.Where(b => b.Manuscript_Status == "采纳稿件").ToList();
            return Ok(info);
        }

        [HttpPost("LayoutUpload")]
        public IActionResult LayoutUpload([FromForm]IFormFile file)
        {
            var a =int.Parse(Request.Headers["ManuscriptID"]);
            try
            {
                SqlConnect sqlConnect = new SqlConnect();
                var fpath = InfoPath.FilePath + "wwwroot/Layout/Manuscript/";
                if (!Directory.Exists(fpath))
                {
                    Directory.CreateDirectory(fpath);
                }
                var info = sqlConnect.Layout.FirstOrDefault(b => b.Manuscript_ID == a);
                info.Layout_Image = "/Layout/Manuscript/" + file.FileName;
                sqlConnect.Layout.Update(info);
                sqlConnect.SaveChanges();
                FileStream stream = new FileStream(InfoPath.FilePath + "wwwroot/Layout/Manuscript/" + file.FileName, FileMode.Create);
                file.CopyTo(stream);              
                return Ok(info.Layout_Image);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return Ok();
        }

        [HttpGet("GetAllCompleteInfo")]
        public IActionResult GetAllCompleteInfo()
        {
            var m_info = sqlConnect.Manuscript.Where(b => b.Manuscript_Status == "采纳稿件").ToList();
            var list = new List<CompleteModels>();
            foreach(var i in m_info)
            {
                CompleteModels layout = new CompleteModels();
                layout.avtor = sqlConnect.Layout.FirstOrDefault(b => b.Manuscript_ID == i.Manuscript_ID).Layout_Image;
                layout.TiTle = i.Manuscript_Title;
                layout.KeyWord = i.Manuscript_Keyword;
                layout.Time = i.Time;
                layout.id = i.Manuscript_ID;
                list.Add(layout);
            }
            return Ok(list);
        }
    }
}