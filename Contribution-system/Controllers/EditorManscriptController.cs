using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Contribution_system_Models;
using Contribution_system_Models.Models;
using Contribution_system_Models.WebModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Contribution_system.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EditorManscriptController : ControllerBase
    {
        SqlConnect sqlConnect;

        public EditorManscriptController(SqlConnect _sqlConnect)
        {
            this.sqlConnect = _sqlConnect;
        }

        [HttpGet]
        public IActionResult FindManuscript()
        {
            var id = User.FindFirst(ClaimTypes.Name)?.Value;
            List<ManuscriptReview> list= sqlConnect.ManuscriptReview.Where(b => b.Editor_ID ==null).ToList();
            return Ok(list);
        }

        [HttpGet("Show")]
        public IActionResult GetAllManuscript(int id)
        {
            ManuscriptReview review = new ManuscriptReview();
            review = sqlConnect.ManuscriptReview.FirstOrDefault(a => a.ManuscriptReview_ID.Equals(id));
            return Ok(review);
        }
       
        [HttpPost("SentComment")]
        public IActionResult SentComment([FromBody] CommentInfo commentinfo)
        {
            commentinfo.role = User.FindFirst(ClaimTypes.Role)?.Value;
            commentinfo.time = DateTime.Now.ToString();
            var info = sqlConnect.ManuscriptReview.FirstOrDefault(b => b.ManuscriptReview_ID == commentinfo.manscriptid);
            if (info.ManuscriptReview_First_Info == null|| info.ManuscriptReview_First_Info == "")
            {
                List<CommentInfo> infos = new List<CommentInfo>();
                infos.Add(commentinfo);
                var s = JsonConvert.SerializeObject(infos);
                info.ManuscriptReview_First_Info = s;
                sqlConnect.Update(info);
                sqlConnect.SaveChanges();
                return Ok();
            }
            else
            {
                List<CommentInfo> infos = JsonConvert.DeserializeObject<List<CommentInfo>>(info.ManuscriptReview_First_Info);
                infos.Add(commentinfo);
                var s = JsonConvert.SerializeObject(infos);
                info.ManuscriptReview_First_Info = s;
                sqlConnect.Update(info);
                sqlConnect.SaveChanges();
                return Ok();
            }
        }

        [HttpGet("GetManuscriptComment")]
        public IActionResult GetManuscriptComment(int id)
        {
            var jsoninfo = sqlConnect.ManuscriptReview.FirstOrDefault(b => b.ManuscriptReview_ID == id).ManuscriptReview_First_Info;
           // var info =JsonConvert.SerializeObject(jsoninfo);
            return Ok(jsoninfo);
        }

        [HttpGet("Complete")]
        public IActionResult CompleteFirstExamination(int id)
        {
            var info = sqlConnect.ManuscriptReview.FirstOrDefault(b => b.ManuscriptReview_ID == id);
            info.ManuscriptReview_Status = "等待主编审查";
            sqlConnect.Update(info);
            sqlConnect.SaveChanges();
            return Ok();
        }

        [HttpGet("GetManuscript")]
        public IActionResult GetManuscript(int id)
        {
            var info = sqlConnect.ManuscriptReview.FirstOrDefault(a => a.ManuscriptReview_ID == id);
            return Ok();
        }

        [HttpGet("GetEdiotrManuscript")]
        public IActionResult GetEdiotrManuscript()
        {
            var id= User.FindFirst(ClaimTypes.Name)?.Value;
            List<ManuscriptReview> manuscripts = new List<ManuscriptReview>();
            manuscripts = sqlConnect.ManuscriptReview.Where(b => b.Editor_ID == id).ToList();
            return Ok(manuscripts);
        }
    }
}