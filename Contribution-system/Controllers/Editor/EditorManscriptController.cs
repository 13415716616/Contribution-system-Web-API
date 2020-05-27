using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using Contribution_system_Commond.Page;
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
            var list = sqlConnect.ManuscriptState.Where(b => b.Manuscript_State.Equals("等待编辑审查"))
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
                        Time = state.Manuscript_Time,
                        ManuscriptColumn_ID = mans.ManuscriptColumn_ID,
                        Manuscript_Status=state.Manuscript_State

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
                        Manuscript_Status=mans.Manuscript_Status
                    }
                );
            sw.Stop();
            TimeSpan ts2 = sw.Elapsed;
            Console.WriteLine("Stopwatch总共花费{0}ms.", ts2.TotalMilliseconds);
            return Ok(list);
        }
      
        // 完成初审
        [HttpPost("CompleteFirstContribution")]
        [Authorize]
        public IActionResult CompleteFirstContribution([FromBody] Review manuscript)
        {
            var state = sqlConnect.ManuscriptState.FirstOrDefault(b => b.Manuscript_ID == manuscript.Manuscript_ID);
            state.Manuscript_State = "等待专家审查";
            state.Expert_ID = manuscript.Filed_ID;
            EditorReview review = new EditorReview();
            review.Editor_ID= User.FindFirst(ClaimTypes.Name)?.Value;
            review.Editor_Type = "初审编辑";
            review.Editor_Opinion = manuscript.ContentText;
            review.Manuscript_ID = manuscript.Manuscript_ID;
            review.Review_Time = DateTime.Now.ToString();
            sqlConnect.Update(state);
            sqlConnect.EditorReview.Add(review);
            sqlConnect.SaveChanges();
            var info=sqlConnect.Manuscript.FirstOrDefault(b => b.Manuscript_ID == manuscript.Manuscript_ID);
            MessageApi.SystemMessage("【系统消息】",info.Author_ID, 
                "你的稿件已通过编辑初审", "<h3><span style='font - weight: bold; '>作者你好：</span></h3><h3>&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; <span style='font - weight: bold; '>您的稿件" + info.Manuscript_Title + "目前已经通过编辑的初步审查，正在移交专家审查，请耐心等待。" +
                "</span></h3><p>&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;&nbsp;&nbsp;稿件标题：" + info.Manuscript_Title + 
                "<br></p><p>&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;&nbsp;&nbsp;投递时间：" + info.Time + 
                "<br></p><p>&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;&nbsp;&nbsp;编辑初审意见：" + review.Editor_Opinion+ "<br></p><h3>&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;" +
                "<span style='font - weight: bold; '>稿件审查进度我们将以信息的方式通知您，请注意查看。</span></h3>");
            return Ok();
        }

        [HttpGet("GetEndManuscript")]
        [Authorize]
        public IActionResult GetEndManuscript()
        {
            var id= User.FindFirst(ClaimTypes.Name)?.Value;
            var info = sqlConnect.ManuscriptState.Where(b => b.Manuscript_State=="等待主编审查"||b.Manuscript_State == "等待专家审查" || b.Manuscript_State == "采纳稿件" || b.Manuscript_State == "稿件退回")
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
                        Time = state.Manuscript_Time,
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

        // 获取所有复审稿件
        [HttpGet("GetSecondEdiotrManuscript")]
        public IActionResult GetSecondEdiotrManuscript()
        {
            var id= User.FindFirst(ClaimTypes.Name)?.Value;          
            var manuscripts = sqlConnect.Manuscript.Where(b =>b.Manuscript_Status =="等待编辑复审").ToList();
            return Ok(manuscripts);
        }

        [HttpPost("CompleteSecondEdiotrManuscript")]
        public IActionResult CompleteSecondEdiotrManuscript([FromBody] Review manuscript)
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
            var state=sqlConnect.ManuscriptState.FirstOrDefault(b => b.Manuscript_ID == id);
            model.Manuscript_ID = info.Manuscript_ID;
            model.Manuscript_Name = info.Manuscript_Title;
            model.ManuscriptColumn_ID = sqlConnect.ManuscriptColumn.FirstOrDefault(b => b.ManuscriptColumn_ID == info.ManuscriptColumn_ID).ManuscriptColumn_Name;
            model.Author_ID = state.Author_ID;
            model.Time = state.Manuscript_Time;
            return Ok(model);
        }

        [HttpGet("GetCompleteManuscrit")]
        public IActionResult GetCompleteManuscrit()
        {
            var id = User.FindFirst(ClaimTypes.Name)?.Value;
            var info = sqlConnect.ManuscriptState.Where(b=> b.Manuscript_State == "采纳稿件").Join(
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
            return Ok(info);
        }

        //获取专家领域信息
        [HttpGet("GetAllExpertFiled")]
        public IActionResult GetAllExpertFiled()
        {
            var info=sqlConnect.ExpertFiled;
            return Ok(info);
        }

        [HttpGet("GetExpertFiled")]
        public IActionResult GetExpertFiled(int id)
        {
            var info = sqlConnect.Expert.Where(b => b.Expert_Filed == id);
            return Ok(info);
        }
    }
}