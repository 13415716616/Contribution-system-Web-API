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
            var info= sqlConnect.Manuscript.Where(b => b.Manuscript_Status.Equals("等待专家审查")).ToList();
            return Ok(info);
        }

        //获取稿件信息
        [HttpGet("GetReviewManuscriptID")]
        public IActionResult GetReviewManuscriptID(int id)
        {
            SqlConnect sqlConnect = new SqlConnect();
            var info = sqlConnect.Manuscript.FirstOrDefault(b => b.Manuscript_ID==id);
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
            var info = sqlConnect.Manuscript.FirstOrDefault(b => b.Manuscript_ID == review.Manuscript_ID);
            info.Manuscript_Status = "等待编辑复审";
            sqlConnect.Update(review);
            sqlConnect.SaveChanges();
            return Ok();
        }

        [HttpGet("ShowCompleteManuscript")]
        //获取已处理的稿件信息
        public IActionResult ShowCompleteManuscript()
        {
            var id = User.FindFirst(ClaimTypes.Name)?.Value;
            SqlConnect sqlConnect = new SqlConnect();
            var info= sqlConnect.ExpertReview.Where(b => b.Expert_ID ==id ).ToList();           
            List<ShowExpertReview> list = new List<ShowExpertReview>();
            foreach(var i in info)
            {
                ShowExpertReview show = new ShowExpertReview();
                show.EditorReview_ID = i.ExpertReview_ID;
                show.Manuscript_ID = i.Manuscript_ID;
                var a = sqlConnect.Manuscript.FirstOrDefault(b => b.Manuscript_ID == i.Manuscript_ID);
                show.Manuscript_Title = a.Manuscript_Title;
                show.Review_Time = i.Review_Time;
                show.Author_ID = a.Author_ID;
                show.Manuscript_Keyword = a.Manuscript_Keyword;
                list.Add(show);
            }
            return Ok(list);
        }
    }
}