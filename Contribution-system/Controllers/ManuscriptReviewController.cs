using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using Contribution_system_Models;
using Contribution_system_Models.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Contribution_system.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManuscriptReviewController : ControllerBase
    {
        SqlConnect sqlConnect;

        public ManuscriptReviewController(SqlConnect _sqlConnect)
        {
            sqlConnect = _sqlConnect;
        }

        //获取用户所有投稿的稿件
        [HttpGet]
        public IActionResult GetAllManuscript()
        {
            var id = User.FindFirst(ClaimTypes.Name)?.Value;
            List<ManuscriptReview> reviews = new List<ManuscriptReview>();
            reviews = sqlConnect.ManuscriptReview.Where(b => b.Author_ID.Equals(id)).ToList();
            return Ok(reviews);
        } 
        
        [HttpGet("ShowManuscriptReviews")]
        public IActionResult GetAllManuscript(int id)
        {
            ManuscriptReview review = new ManuscriptReview();
            review = sqlConnect.ManuscriptReview.FirstOrDefault(a => a.ManuscriptReview_ID.Equals(id));
            return Ok(review);
        }
    }
}