using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Contribution_system_Commond.Page;
using Contribution_system_Models;
using Contribution_system_Models.Models;
using Contribution_system_Models.WebModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Contribution_system.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManuscriptController : ControllerBase
    {
        // Post方式将稿件退回的API
        [HttpPost("ReturnComplete")]
        public IActionResult ReturnComplete([FromBody] Review manuscript)
        {
            SqlConnect sqlConnect = new SqlConnect();
            var state= sqlConnect.ManuscriptState.FirstOrDefault(b => b.Manuscript_ID.Equals(manuscript.Manuscript_ID));
            state.Manuscript_State = "稿件退回";
            state.Manuscript_Result = "稿件退回";
            EditorReview review = new EditorReview();
            review.Editor_ID = User.FindFirst(ClaimTypes.Name)?.Value;
            review.Editor_Type = "稿件退回";
            review.Editor_Opinion = manuscript.ContentText;
            review.Manuscript_ID = manuscript.Manuscript_ID;
            review.Review_Time = DateTime.Now.ToString();
            sqlConnect.Update(state);
            sqlConnect.EditorReview.Add(review);
            sqlConnect.SaveChanges();
            var info = sqlConnect.Manuscript.FirstOrDefault(b => b.Manuscript_ID.Equals(manuscript.Manuscript_ID));
            MessageApi.SystemMessage("管理员",info.Author_ID, "您的稿件《" + info.Manuscript_Title + "》被退回，请查看信息",
                "<h3><span style='font - weight: bold; '>作者你好：</span></h3><h3>&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; <span style='font - weight: bold; '>您的稿件" + info.Manuscript_Title + "目前未通过审核，稿件被退回" +
                "</span></h3><p>&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;&nbsp;&nbsp;稿件标题：" + info.Manuscript_Title +
                "<br></p><p>&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;&nbsp;&nbsp;投递时间：" + info.Time +
                "<br></p><p>&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;&nbsp;&nbsp;编辑退回意见：" + review.Editor_Opinion + "<br></p><h3>&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;" +
                "<span style='font - weight: bold; '>稿件审查进度我们将以信息的方式通知您，请注意查看。</span></h3>"
                );
            return Ok();
        }

        [HttpGet("GetAllReturnManuscript")]
        public IActionResult GetAllReturnManuscript()
        {
            SqlConnect sqlConnect = new SqlConnect();
            var info= sqlConnect.Manuscript.Where(b => b.Manuscript_Status.Equals("稿件退回"));
            return Ok(info);
        }
    }
}