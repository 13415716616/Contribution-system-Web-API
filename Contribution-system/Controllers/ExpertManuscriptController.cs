using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Contribution_system_Models;
using Contribution_system_Models.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Contribution_system.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpertManuscriptController : ControllerBase
    {
        [HttpGet("GetReviewManuscript")]
        public IActionResult GetReviewManuscript()
        {
            SqlConnect sqlConnect = new SqlConnect();
            var info= sqlConnect.Manuscript.Where(b => b.Manuscript_Status.Equals("专家审查中")).ToList();
            return Ok(info);
        }

        [HttpGet("GetReviewManuscriptID")]
        public IActionResult GetReviewManuscriptID(int id)
        {
            SqlConnect sqlConnect = new SqlConnect();
            var info = sqlConnect.Manuscript.FirstOrDefault(b => b.Manuscript_ID==id);
            return Ok(info);
        }

        [HttpPost("ReviewManuscript")]
        public IActionResult ReviewManuscript([FromBody] ExpertReview review)
        {
            SqlConnect sqlConnect = new SqlConnect();
            review.Expert_ID= User.FindFirst(ClaimTypes.Name)?.Value;
            review.Review_Time = DateTime.Now.ToString();
            sqlConnect.ExpertReview.Add(review);
            sqlConnect.SaveChanges();
            var info = sqlConnect.Manuscript.FirstOrDefault(b => b.Manuscript_ID == review.Manuscript_ID);
            info.Manuscript_Status = "等待主编复审";
            sqlConnect.Update(review);
            sqlConnect.SaveChanges();
            return Ok();
        }
    }
}