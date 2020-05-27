using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Contribution_system_Models;
using Contribution_system_Models.Models;
using Contribution_system_Models.WebModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;

namespace Contribution_system.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpertManuscriptController : ControllerBase
    {
        //获取待审核的稿件
        [HttpGet("GetReviewManuscript")]
        public IActionResult GetReviewManuscript()
        {
            SqlConnect sqlConnect = new SqlConnect();
            var a= User.FindFirst(ClaimTypes.Name)?.Value;
            var info = sqlConnect.ManuscriptState.Where(b => b.Manuscript_State.Equals("等待专家审查")&&b.Expert_ID==a).Join(
                    sqlConnect.Manuscript,
                    manus => manus.Manuscript_ID,
                    col => col.Manuscript_ID,
                    (mans, col) => new
                    {
                        Manuscript_ID = mans.Manuscript_ID,
                        Manuscript_Title = col.Manuscript_Title,
                        Manuscript_Keyword = col.Manuscript_Keyword,
                        Manuscript_Status = mans.Manuscript_State,
                        ManuscriptColumn_ID = col.ManuscriptColumn_ID,
                        Author_Name = mans.Author_ID,
                        Time = col.Time,
                    }
                )
                .Join(sqlConnect.ManuscriptColumn,
                    manus => manus.ManuscriptColumn_ID,
                    col => col.ManuscriptColumn_ID,
                    (mans, col) => new ShowManuscript
                    {
                        Manuscript_ID = mans.Manuscript_ID,
                        Manuscript_Title = mans.Manuscript_Title,
                        ManuscriptColumn = col.ManuscriptColumn_Name,
                        Manuscript_Keyword = mans.Manuscript_Keyword,
                        Manuscript_Status = mans.Manuscript_Status,
                        Author_Name = mans.Author_Name,
                        Time = mans.Time,
                    });
            return Ok(info);
        }

        //获取稿件信息
        [HttpGet("GetReviewManuscriptID")]
        public IActionResult GetReviewManuscriptID(int id)
        {
            SqlConnect sqlConnect = new SqlConnect();
            var info = sqlConnect.Manuscript.FirstOrDefault(b => b.Manuscript_ID == id);
            ShowExpertReview review = new ShowExpertReview();
            review.Manuscript_Title = info.Manuscript_Title;
            var s = sqlConnect.ManuscriptState.FirstOrDefault(b => b.Manuscript_ID == id);
            review.Author_ID = s.Author_ID;
            review.Review_Time = s.Manuscript_Time;
            var c = sqlConnect.ManuscriptColumn.FirstOrDefault(b => b.ManuscriptColumn_ID == info.ManuscriptColumn_ID);
            review.ManuscriptColumn = c.ManuscriptColumn_Name;
            return Ok(review);
        }

        //获取稿件信息
        [HttpGet("GetManuscriptID")]
        public IActionResult GetManuscriptID(int id)
        {
            SqlConnect sqlConnect = new SqlConnect();
            var info = sqlConnect.Manuscript.FirstOrDefault(b => b.Manuscript_ID == id);
            return Ok(info);
        }

        //处理稿件信息
        [HttpPost("ReviewManuscript")]
        public IActionResult ReviewManuscript([FromBody] ExpertReview review)
        {
            SqlConnect sqlConnect = new SqlConnect();
            review.Expert_ID= User.FindFirst(ClaimTypes.Name)?.Value;
            review.Review_Time = DateTime.Now.ToString();
            sqlConnect.ExpertReview.Add(review);
            sqlConnect.SaveChanges();
            var info = sqlConnect.ManuscriptState.FirstOrDefault(b => b.Manuscript_ID == review.Manuscript_ID);
            info.Manuscript_State = "等待主编审查";
            sqlConnect.Update(info);
            sqlConnect.SaveChanges();
            return Ok();
        }

        [HttpGet("ShowCompleteManuscript")]
        //获取已处理的稿件信息
        public IActionResult ShowCompleteManuscript()
        {
            var id = User.FindFirst(ClaimTypes.Name)?.Value;
            SqlConnect sqlConnect = new SqlConnect();
            var info = sqlConnect.ExpertReview.Where(b => b.Expert_ID == id).Join(
                sqlConnect.Manuscript, er=>er.Manuscript_ID, mans=>mans.Manuscript_ID, (er, mans) =>new
                {
                    editorReview_ID=er.ExpertReview_ID,
                    Manuscript_Title =mans.Manuscript_Title,
                    manuscript_Keyword=mans.Manuscript_Keyword,
                    review_Time = er.Review_Time,
                    ManuscriptColumn_ID=mans.ManuscriptColumn_ID,
                    Manuscript_ID=er.Manuscript_ID,
                }).Join(
                sqlConnect.ManuscriptColumn, mans => mans.ManuscriptColumn_ID, col => col.ManuscriptColumn_ID, (mans, col) => new
                {
                    editorReview_ID = mans.editorReview_ID,
                    Manuscript_Title = mans.Manuscript_Title,
                    manuscript_Keyword = mans.manuscript_Keyword,
                    review_Time = mans.review_Time,
                    ManuscriptColumn = col.ManuscriptColumn_Name,
                    Manuscript_ID = mans.Manuscript_ID,
                }
            );
            return Ok(info);
        }
    }
}