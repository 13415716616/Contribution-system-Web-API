using Contribution_system_Commond.Page;
using Contribution_system_Models;
using Contribution_system_Models.Models;
using Contribution_system_Models.WebModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;

namespace Contribution_system.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorManuscriptController : ControllerBase
    {
        SqlConnect sqlConnect;

        public AuthorManuscriptController(SqlConnect sqlConnect)
        {
            this.sqlConnect = sqlConnect;
        }


        //获取栏目信息
        [HttpGet("GetManuscriptColumn")]
        public IActionResult GetManuscriptColumn()
        {
            var info = sqlConnect.ManuscriptColumn.ToList();
            return Ok(info);
        }


        //添加新的稿件信息
        [HttpPost]
        [Authorize]
        public IActionResult AddManuscript([FromBody] Manuscript manuscript)
        {
            string author_id = manuscript.Author_ID = User.FindFirst(ClaimTypes.Name)?.Value;
            var id = AuthorManuscriptPageAPI.AddNewManuscript(manuscript, author_id);
            if (id != 0)
            {
                return Ok(id);
            }
            else
                return BadRequest();
        }


        //上传主要的稿件
        [HttpPost("uploadmain")]
        [Authorize]
        public IActionResult UploadDataMain([FromForm] IFormFile file)
        {
            var a = int.Parse(Request.Headers["ManuscriptID"]);
            var userid = User.FindFirst(ClaimTypes.Name)?.Value;
            if (AuthorManuscriptPageAPI.AddMainManuscriptUpload(file, a))
                return Ok();
            else
                return BadRequest();
        }

        //上传次要的稿件
        [HttpPost("uploadother")]
        [Authorize]
        public IActionResult UploadDataOther([FromForm] IFormFile file)
        {
            var a = int.Parse(Request.Headers["ManuscriptID"]);
            var userid = User.FindFirst(ClaimTypes.Name)?.Value;
            if (AuthorManuscriptPageAPI.AddOtherManuscriptUpload(file, a))
                return Ok();
            else
                return BadRequest();
        }


        //添加作者信息
        [HttpPost("AddManuscriptAuthor")]
        [Authorize]
        public IActionResult AddManuscriptAuthor([FromBody] Contribution_system_Models.Models.ManuscriptAuthor author)
        {
            sqlConnect.ManuscriptAuthor.Add(author);
            sqlConnect.SaveChanges();
            return Ok();
        }

        //获取稿件信息
        [HttpGet("GetManuscript")]
        public IActionResult GetManuscript(int id)
        {
            var info = sqlConnect.Manuscript.FirstOrDefault(b => b.Manuscript_ID == id);
            return Ok(info);
        }

        // 获取草稿箱稿件的信息
        [HttpGet("ManuscriptToDrafts")]
        public IActionResult GetManscriptInfo()
        {
            string author_id = User.FindFirst(ClaimTypes.Name)?.Value;
            var info = sqlConnect.ManuscriptState.Where(b => b.Author_ID.Equals(author_id) && b.Manuscript_State.Equals("稿件编辑中")).Join(
                sqlConnect.Manuscript, man => man.Manuscript_ID, state => state.Manuscript_ID,(man,state)=>new { state }).Join(
                sqlConnect.ManuscriptColumn,man=>man.state.ManuscriptColumn_ID,col=> col.ManuscriptColumn_ID,(man,col)=>new { 
                    Manuscript_ID=man.state.Manuscript_ID,
                    Manuscript_Name=man.state.Manuscript_Title,
                    Manuscript_KeyWork=man.state.Manuscript_Keyword,
                    ManuscriptColumn_ID=col.ManuscriptColumn_Name
                });
            return Ok(info);
        }

        //更新的稿件信息
        [HttpPost("UpadteManuscript")]
        [Authorize]
        public IActionResult UpadteManuscript([FromBody] Manuscript manuscript)
        {
            var Info = sqlConnect.Manuscript.FirstOrDefault(b => b.Manuscript_ID.Equals(manuscript.Manuscript_ID));
            Info.ManuscriptColumn_ID = manuscript.ManuscriptColumn_ID;
            Info.Manuscript_Title = manuscript.Manuscript_Title;
            Info.Manuscript_Abstract = manuscript.Manuscript_Abstract;
            Info.Manuscript_Content = manuscript.Manuscript_Content;
            Info.Manuscript_EAbstract = manuscript.Manuscript_EAbstract;
            Info.Manuscript_EKeyword = manuscript.Manuscript_EKeyword;
            Info.Manuscript_Etitle = manuscript.Manuscript_Etitle;
            Info.Manuscript_Keyword = manuscript.Manuscript_Keyword;
            Info.Manuscript_Reference = manuscript.Manuscript_Reference;
            sqlConnect.Update(Info);
            sqlConnect.SaveChanges();
            return Ok();
        }

        //更改草稿箱中的稿件信息
        //[HttpGet("ManuscriptTos")]
        //[Authorize]
        //public IActionResult ManuscriptTos()
        //{
        //    try { 
        //        var userid = User.FindFirst(ClaimTypes.Name)?.Value;
        //        var manuscripts = sqlConnect.Manuscript.Where(b => b.Author_ID.Equals(userid)).ToList();
        //        return Ok(manuscripts);
        //    }catch(Exception e)
        //    {
        //        Console.Write(e);
        //        return BadRequest();
        //    }
        //}



        [HttpGet("GetFile")]
        public IActionResult GetFile(int id)
        {
            var info = sqlConnect.ManuscriptFile.FirstOrDefault(b => b.Manuscript_ID == id);
            var provider = new FileExtensionContentTypeProvider();
            var file = InfoPath.ModelsPath + info.ManuscriptFile_Path;
            var fileName = Path.GetFileName(file);
            var ext = Path.GetExtension(fileName);
            var stream = System.IO.File.OpenRead(file);
            var contentType = provider.Mappings[ext];
            return File(stream, contentType, fileName);
        }

        //获取所以通过的稿件
        [HttpGet("GetCompleteManuscrit")]
        public IActionResult GetCompleteManuscrit()
        {
            var id = User.FindFirst(ClaimTypes.Name)?.Value;
            var info = sqlConnect.ManuscriptState.Where(b => b.Author_ID == id && b.Manuscript_State == "采纳稿件")
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
            //List<CompleteManuscript> list = new List<CompleteManuscript>();
            //foreach(var i in info)
            //{
            //    CompleteManuscript a = new CompleteManuscript();
            //    a.avtor = sqlConnect.Layout.FirstOrDefault(b => b.Manuscript_ID == i.Manuscript_ID).Layout_Image;
            //    a.Titile = i.Manuscript_Title;
            //    a.KeyWord = i.Manuscript_Keyword;
            //    a.Time = i.Time;
            //    list.Add(a);
            //}
            return Ok(info);
        }

        [HttpPost("CompleteManuscript")]
        public IActionResult CompleteManuscript([FromBody] Manuscript manuscript)
        {
            SqlConnect sqlConnect = new SqlConnect();
            var id = User.FindFirst(ClaimTypes.Name)?.Value;
            var state = sqlConnect.ManuscriptState.FirstOrDefault(b => b.Manuscript_ID == manuscript.Manuscript_ID);
            var info = sqlConnect.Manuscript.FirstOrDefault(b => b.Manuscript_ID == manuscript.Manuscript_ID);
            state.Manuscript_State = "等待编辑审查";
            state.Manuscript_Time = DateTime.Now.ToString();
            MessageApi.SystemMessage("【系统消息】", id, "你的稿件《" + info.Manuscript_Title + "》投递完成", "<h3><span style='font - weight: bold; '>作者你好：</span></h3><h3>&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; <span style='font - weight: bold; '>您的稿件" + info.Manuscript_Title + "投递成功，正在等待编辑初审，请耐心等待。</span></h3><p>&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;&nbsp;&nbsp;稿件标题：" + info.Manuscript_Title + "<br></p><p>&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;&nbsp;&nbsp;投递时间：" + info.Time + "<br></p><h3>&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; <span style='font - weight: bold; '>稿件审查进度我们将以信息的方式通知您，请注意查看。</span></h3>");
            sqlConnect.Update(state);
            sqlConnect.SaveChanges();
            return Ok();
        }

        [HttpGet("CompleteManuscriptID")]
        public IActionResult CompleteManuscriptID(int id)
        {
            SqlConnect sqlConnect = new SqlConnect();
            var info = sqlConnect.Manuscript.FirstOrDefault(b => b.Manuscript_ID == id);
            var state= sqlConnect.ManuscriptState.FirstOrDefault(b => b.Manuscript_ID == id);
            state.Manuscript_State = "等待编辑审查";
            var uid = User.FindFirst(ClaimTypes.Name)?.Value;
            MessageApi.SystemMessage("【系统消息】", uid, "你的稿件投递完成", "<h3><span style='font - weight: bold; '>作者你好：</span></h3><h3>&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; <span style='font - weight: bold; '>您的稿件"+info.Manuscript_Title+"投递成功，正在等待编辑初审，请耐心等待。</span></h3><p>&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;&nbsp;&nbsp;稿件标题："+info.Manuscript_Title+"<br></p><p>&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;&nbsp;&nbsp;投递时间："+info.Time+"<br></p><h3>&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; <span style='font - weight: bold; '>稿件审查进度我们将以信息的方式通知您，请注意查看。</span></h3>");
            sqlConnect.Update(state);
            sqlConnect.SaveChanges();
            return Ok();
        }

        [HttpGet("DeleteMansuscriptDrafts")]
        public IActionResult DeleteMansuscriptDrafts(int id)
        {
            try { 
                var minfo = sqlConnect.Manuscript.FirstOrDefault(b => b.Manuscript_ID == id);
                sqlConnect.Remove(minfo);
                sqlConnect.SaveChanges();
                var author = sqlConnect.ManuscriptAuthor.Where(b => b.Manuscript_ID == id).ToList();
                sqlConnect.Remove(author);
                sqlConnect.SaveChanges();
                var state = sqlConnect.ManuscriptState.FirstOrDefault(b => b.Manuscript_ID == id);
                sqlConnect.Remove(state);
                sqlConnect.SaveChanges();
                return Ok();
            }
            catch (Exception e)
            {
                return Ok();
            }
        }

        [HttpGet("GetReviewManuscript")]
        public IActionResult GetReviewManuscript()
        {
            var id= User.FindFirst(ClaimTypes.Name)?.Value;
            var info = sqlConnect.ManuscriptState.Where(b => b.Author_ID == id && b.Manuscript_State != "稿件编辑中"&&b.Manuscript_State != "采纳稿件" && b.Manuscript_State != "稿件退回")
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
                   });
            return Ok(info);
        }

        [HttpGet("GetManuscriptID")]
        public IActionResult GetManuscriptID(int id)
        {
            var info = sqlConnect.Manuscript.FirstOrDefault(b => b.Manuscript_ID == id);
            return Ok(info);
        }

        [HttpGet("GetManuscriptStateID")]
        public IActionResult GetManuscriptStateID(int id)
        {
            var info = sqlConnect.ManuscriptState.FirstOrDefault(b => b.Manuscript_ID == id);
            return Ok(info);
        }

        [HttpGet("GetReturnManuscript")]
        public IActionResult GetReturnManuscript()
        {
            var id = User.FindFirst(ClaimTypes.Name)?.Value;
            var info = sqlConnect.ManuscriptState.Where(b => b.Author_ID == id && b.Manuscript_Result =="稿件退回")
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
                   });
            return Ok(info);
        }

        [HttpGet("GetReviewReturn")]
        public IActionResult GetReviewReturn(int id)
        {
            var info = sqlConnect.EditorReview.FirstOrDefault(b => b.Manuscript_ID==id);
            return Ok(info);
        }
    }
}
