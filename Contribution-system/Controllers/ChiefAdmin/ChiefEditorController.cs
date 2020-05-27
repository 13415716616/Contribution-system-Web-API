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


        //获取稿件
        [HttpGet("GetChiefEditorManuscript")]
        public IActionResult GetChiefEditorManuscript()
        {
            SqlConnect sqlConnect = new SqlConnect();
            var info = sqlConnect.ManuscriptState.Where(b => b.Manuscript_State.Equals("等待主编审查"))
                .Join(
                    sqlConnect.Manuscript,
                    state => state.Manuscript_ID,
                    mans => mans.Manuscript_ID,
                    (state, mans) => new ShowManuscript
                    {
                        Manuscript_ID = mans.Manuscript_ID,
                        Manuscript_Title = mans.Manuscript_Title,
                        Author_Name = state.Author_ID,
                        Manuscript_Keyword = mans.Manuscript_Keyword,
                        Time = mans.Time,
                        ManuscriptColumn_ID = mans.ManuscriptColumn_ID,
                        Manuscript_Status = state.Manuscript_State

                    })
                .Join(
                    sqlConnect.ManuscriptColumn,
                    mans => mans.ManuscriptColumn_ID,
                    col => col.ManuscriptColumn_ID,
                    (mans, col) => new ShowManuscript
                    {
                        Manuscript_ID = mans.Manuscript_ID,
                        Manuscript_Title = mans.Manuscript_Title,
                        Author_Name = mans.Author_Name,
                        Manuscript_Keyword = mans.Manuscript_Keyword,
                        Time = mans.Time,
                        ManuscriptColumn = col.ManuscriptColumn_Name,
                        Manuscript_Status = mans.Manuscript_Status
                    }
                );
            return Ok(info);
        }

        [HttpGet("GetExpertReviewInfo")]
        public IActionResult GetExpertReviewInfo(int id)
        {
            var info = sqlConnect.ExpertReview.FirstOrDefault(b => b.Manuscript_ID == id);
            return Ok(info);
        }

        [HttpGet("GetEditorReviewInfo")]
        public IActionResult GetEditorReviewInfo(int id)
        {
            var info = sqlConnect.EditorReview.FirstOrDefault(b => b.Manuscript_ID == id);
            return Ok(info);
        }

        [HttpPost("CompleteManuscript")]
        public IActionResult CompleteManuscript([FromBody] EditorReview review)
        {
            review.Editor_Type = "主编终审";
            review.Review_Time = DateTime.Now.ToString();
            review.Editor_ID= User.FindFirst(ClaimTypes.Name)?.Value;
            var info = sqlConnect.ManuscriptState.FirstOrDefault(b => b.Manuscript_ID == review.Manuscript_ID);
            info.Manuscript_State = "采纳稿件";
            info.Manuscript_Result = "稿件通过";
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
            var m_info = sqlConnect.ManuscriptState.Where(b => b.Manuscript_State.Equals("采纳稿件"))
                .Join(
                    sqlConnect.Manuscript,
                    state => state.Manuscript_ID,
                    mans => mans.Manuscript_ID,
                    (state, mans) => new ShowManuscript
                    {
                        Manuscript_ID = mans.Manuscript_ID,
                        Manuscript_Title = mans.Manuscript_Title,
                        Author_Name = state.Author_ID,
                        Manuscript_Keyword = mans.Manuscript_Keyword,
                        Time = mans.Time,
                        ManuscriptColumn_ID = mans.ManuscriptColumn_ID,
                        Manuscript_Status = state.Manuscript_State

                    })
                .Join(
                    sqlConnect.Layout,
                    mans => mans.Manuscript_ID,
                    col => col.Manuscript_ID,
                    (mans, col) => new
                    {
                        Manuscript_ID = mans.Manuscript_ID,
                        Manuscript_Title = mans.Manuscript_Title,
                        Author_Name = mans.Author_Name,
                        Manuscript_Keyword = mans.Manuscript_Keyword,
                        Time = mans.Time,
                        Avtor = col.Layout_Image,
                        Manuscript_Status = mans.Manuscript_Status
                    }
                );
            return Ok(m_info);
        }
    }
}