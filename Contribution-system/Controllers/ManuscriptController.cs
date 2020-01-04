﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Contribution_system_Models.Models;
using Microsoft.AspNetCore.Authorization;
using static Contribution_system_Models.WebModel.ManuscriptModel;
using System.Security.Claims;
using Contribution_system_Models.WebModel;
using System.IO;
using Microsoft.Extensions.Primitives;

namespace Contribution_system.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManuscriptController : ControllerBase
    {
        SqlConnect sqlConnect;

        public ManuscriptController(SqlConnect sqlConnect)
        {
            this.sqlConnect = sqlConnect;
        }

        [HttpPost]
        [Authorize]
        public IActionResult AddManuscript(Manuscript manuscript)
        {
            try { 
                manuscript.Manuscript_Status = ManuscriptMode.WriteInfo;
                manuscript.Author_ID = User.FindFirst(ClaimTypes.Name)?.Value;
                sqlConnect.Manuscript.Add(manuscript);
                sqlConnect.SaveChanges();
                return Ok(manuscript.Manuscript_ID);
            }catch(Exception e)
            {
                Console.WriteLine(e);
                return BadRequest();
            }
        }

        [HttpPost("uploadmain")]
        [Authorize]
        public IActionResult UploadDataMain([FromForm] IFormFile file)
        {
            //StringValues value1;
            //Request.Headers.TryGetValue("ManuscriptID", out value1);
            try { 
                var a =int.Parse(Request.Headers["ManuscriptID"]);
                var manuscript = sqlConnect.Manuscript.FirstOrDefault(b => b.Manuscript_ID==a);
                var userid = User.FindFirst(ClaimTypes.Name)?.Value;
                var fpath = InfoPath.ModelsPath + "wwwroot\\File\\Manuscript\\" + userid;           
                if (!Directory.Exists(fpath))
                {
                    Directory.CreateDirectory(fpath);
                }
                var spath=fpath+ "\\" + a;
                if (!Directory.Exists(spath))
                {
                    Directory.CreateDirectory(spath);
                }
                var mpath = spath + "\\主要稿件\\";
                if (!Directory.Exists(mpath))
                {
                    Directory.CreateDirectory(mpath);
                }
                FileStream stream = new FileStream(InfoPath.ModelsPath+"wwwroot\\File\\Manuscript\\" + userid + "\\"+a+ "\\主要稿件\\" + file.FileName, FileMode.Create);
                file.CopyTo(stream);
                manuscript.Manuscript_MainDataPath = "wwwroot\\File\\Manuscript\\" + userid + "\\" + a + "\\主要稿件\\" + file.FileName;
                sqlConnect.Update(manuscript);
                sqlConnect.SaveChanges();
                return Ok();
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
            return BadRequest();
        }

        [HttpPost("uploadother")]
        [Authorize]
        public IActionResult UploadDataOther([FromForm] IFormFile file)
        {
            //StringValues value1;
            //Request.Headers.TryGetValue("ManuscriptID", out value1);
            try
            {
                var a = int.Parse(Request.Headers["ManuscriptID"]);
                var manuscript = sqlConnect.Manuscript.FirstOrDefault(b => b.Manuscript_ID == a);
                var userid = User.FindFirst(ClaimTypes.Name)?.Value;
                var fpath = InfoPath.ModelsPath + "wwwroot\\File\\Manuscript\\" + userid;
                if (!Directory.Exists(fpath))
                {
                    Directory.CreateDirectory(fpath);
                }
                var spath = fpath + "\\" + a;
                if (!Directory.Exists(spath))
                {
                    Directory.CreateDirectory(spath);
                }
                var mpath = spath + "\\其他资料\\";
                if (!Directory.Exists(mpath))
                {
                    Directory.CreateDirectory(mpath);
                }
                FileStream stream = new FileStream(InfoPath.ModelsPath + "wwwroot\\File\\Manuscript\\" + userid + "\\" + a + "\\其他资料\\" + file.FileName, FileMode.Create);
                file.CopyTo(stream);
                manuscript.Manuscript_OtherDataPath = "wwwroot\\File\\Manuscript\\" + userid + "\\" + a + "\\其他资料\\" + file.FileName;
                sqlConnect.Update(manuscript);
                sqlConnect.SaveChanges();
                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return BadRequest();
        }

        [HttpPost("AddAuthor")]
        public IActionResult AddaAuthor([FromBody] ManuscriptAuthor author)
        {
            try
            {
                var info = sqlConnect.Manuscript.FirstOrDefault(b => b.Manuscript_ID.Equals(author.Manscript_ID));
                info.Author_name = author.Author_name;
                info.Author_sex = author.Author_sex;
                info.Author_Phone = author.Author_Phone;
                info.Author_Address = author.Author_Address;
                info.Author_dec = author.Author_dec;
                sqlConnect.Update(info);
                sqlConnect.SaveChanges();
                return Ok();
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
            return BadRequest();
        }

        [HttpGet]
        public IActionResult GetManscriptInfo(int id)
        {
            var info = sqlConnect.Manuscript.FirstOrDefault(b => b.Manuscript_ID.Equals(id));
            return Ok(info);
        }
    }
}