using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using Contribution_system_Models;
using Contribution_system_Models.Models;
using Contribution_system_Models.WebModel;
using Microsoft.AspNetCore.Authorization;
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

        // 需要编辑初审的稿件信息
        [HttpGet]
        public IActionResult FindManuscript()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var id = User.FindFirst(ClaimTypes.Name)?.Value;
            //List<Manuscript> list= sqlConnect.Manuscript.Where(b => b.Manuscript_Status.Equals("等待编辑审查")).ToList();
            var list = sqlConnect.Manuscript.Where(b => b.Manuscript_Status.Equals("等待编辑审查"))
                .Join(
                    sqlConnect.ManuscriptColumn,
                    manus => manus.ManuscriptColumn_ID,
                    col => col.ManuscriptColumn_ID,
                    (mans, col) =>new ShowManuscript
                    {
                        Manuscript_ID = mans.Manuscript_ID,
                        Manuscript_Title = mans.Manuscript_Title,
                        ManuscriptColumn = col.ManuscriptColumn_Name,
                        Manuscript_Keyword = mans.Manuscript_Keyword,
                        Manuscript_Status = mans.Manuscript_Status,
                        Author_Name=mans.Author_ID,
                        Time = mans.Time,
                    });
            sw.Stop();
            TimeSpan ts2 = sw.Elapsed;
            Console.WriteLine("Stopwatch总共花费{0}ms.", ts2.TotalMilliseconds);
            return Ok(list);
        }
      

        [HttpPost("CompleteFirstContribution")]
        [Authorize]
        public IActionResult CompleteFirstContribution([FromBody] FirstReview manuscript)
        {
            var info = sqlConnect.Manuscript.FirstOrDefault(b => b.Manuscript_ID == manuscript.Manuscript_ID);
            info.Manuscript_Status = "等待专家审查";
            EditorReview review = new EditorReview();
            review.Editor_ID= User.FindFirst(ClaimTypes.Name)?.Value;
            review.Editor_Type = "初审编辑";
            review.Editor_Opinion = manuscript.ContentText;
            review.Manuscript_ID = manuscript.Manuscript_ID;
            review.Review_Time = DateTime.Now.ToString();
            sqlConnect.Update(info);
            sqlConnect.EditorReview.Add(review);
            sqlConnect.SaveChanges();
            return Ok();
        }

        [Authorize]
        [HttpPost("SentComment")]
        public IActionResult SentComment([FromBody] CommentInfo commentinfo)
        {
            commentinfo.role = User.FindFirst(ClaimTypes.Role)?.Value;
            commentinfo.time = DateTime.Now.ToString();
            var id= User.FindFirst(ClaimTypes.Name)?.Value; 
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
                bool test = commentinfo.role.Equals("ChiefEditor");
                if (commentinfo.role.Equals("ChiefEditor"))
                {
                    info.ManuscriptReview_Status = "主编审查中";
                    info.ChiefEditor_ID = id;
                }
                if(commentinfo.role.Equals("Editor"))
                {
                    info.ManuscriptReview_Status = "编辑审查中";
                    info.Editor_ID = id;
                }
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

        [HttpGet("GetEndManuscript")]
        [Authorize]
        public IActionResult GetEndManuscript()
        {
            var id= User.FindFirst(ClaimTypes.Name)?.Value;
            var info = sqlConnect.Manuscript.Where(b => b.Manuscript_Status=="等待主编审查"||b.Manuscript_Status=="等待专家审查")
                .Join(
                    sqlConnect.ManuscriptColumn,
                    manus => manus.ManuscriptColumn_ID,
                    col => col.ManuscriptColumn_ID,
                    (mans, col) => new ShowManuscript
                    {
                        Manuscript_ID = mans.Manuscript_ID,
                        Manuscript_Title = mans.Manuscript_Title,
                        ManuscriptColumn = col.ManuscriptColumn_Name,
                        Manuscript_Keyword = mans.Manuscript_Keyword,
                        Manuscript_Status = mans.Manuscript_Status,
                        Author_Name = mans.Author_ID,
                        Time = mans.Time,
                    });
            return Ok(info);
        }

        [HttpGet("GetManuscript")]
        public IActionResult GetManuscript(int id)
        {
            var info = sqlConnect.ManuscriptReview.FirstOrDefault(a => a.ManuscriptReview_ID == id);
            return Ok();
        }

        // 获取所有初审稿件
        [HttpGet("GetEdiotrManuscript")]
        public IActionResult GetEdiotrManuscript()
        {
            var id= User.FindFirst(ClaimTypes.Name)?.Value;
            List<ManuscriptReview> manuscripts = new List<ManuscriptReview>();
            manuscripts = sqlConnect.ManuscriptReview.Where(b => b.Editor_ID == id&&b.ManuscriptReview_Status=="编辑审查中").ToList(); 
            return Ok(manuscripts);
        }

        // 获取所有复审稿件
        [HttpGet("GetSecondEdiotrManuscript")]
        public IActionResult GetSecondEdiotrManuscript()
        {
            var id= User.FindFirst(ClaimTypes.Name)?.Value;          
            var manuscripts = sqlConnect.Manuscript.Where(b =>b.Manuscript_Status =="等待编辑复审").ToList();
            return Ok(manuscripts);
        }

        [HttpPost("CompleteSecondEdiotrManuscript")]
        public IActionResult CompleteSecondEdiotrManuscript([FromBody] FirstReview manuscript)
        {
            var info = sqlConnect.Manuscript.FirstOrDefault(b => b.Manuscript_ID == manuscript.Manuscript_ID);
            info.Manuscript_Status = "等待主编审查";
            EditorReview review = new EditorReview();
            review.Editor_ID = User.FindFirst(ClaimTypes.Name)?.Value;
            review.Editor_Type = "复审编辑";
            review.Editor_Opinion = manuscript.ContentText;
            review.Manuscript_ID = manuscript.Manuscript_ID;
            review.Review_Time = DateTime.Now.ToString();
            sqlConnect.Update(info);
            sqlConnect.EditorReview.Add(review);
            sqlConnect.SaveChanges();
            return Ok();
        }

        // 获取稿件作者信息
        [HttpGet("GetManuscriptAuthor")]
        public IActionResult GetManuscriptAuthor(int id)
        {
            var author = sqlConnect.ManuscriptAuthor.Where(b => b.Manuscript_ID == 2).ToList();
            return Ok(author);
        }

        [HttpGet("ReviewFirstManuscript")]
        public IActionResult ReviewFirstManuscript(int id)
        {
            ReviewManuscriptModel model = new ReviewManuscriptModel();
            var info = sqlConnect.Manuscript.FirstOrDefault(b => b.Manuscript_ID == id);
            model.Manuscript_ID = info.Manuscript_ID;
            model.Manuscript_Name = info.Manuscript_Title;
            model.ManuscriptColumn_ID = sqlConnect.ManuscriptColumn.FirstOrDefault(b => b.ManuscriptColumn_ID == info.ManuscriptColumn_ID).ManuscriptColumn_Name;
            model.Author_ID = info.Author_ID;
            model.Time = info.Time;
            return Ok(model);
        }
    }
}