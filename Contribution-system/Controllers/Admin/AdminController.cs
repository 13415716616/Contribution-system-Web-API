using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contribution_system_Models;
using Contribution_system_Models.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Contribution_system.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        SqlConnect sqlConnect;

        public AdminController(SqlConnect sqlConnect)
        {
            this.sqlConnect = sqlConnect;
        }

        [HttpGet("GetAllAuthor")]
        public IActionResult GetAllAuthor()
        {
            var list = sqlConnect.Authors;
            return Ok(list);
        }

        [HttpGet("GetAllAuthorID")]
        public IActionResult GetAllAuthorID(string id)
        {
            var info = sqlConnect.Authors.FirstOrDefault(b => b.Author_ID.Equals(id));
            return Ok(info);
        }

        [HttpGet("DeleteAuthor")]
        public IActionResult DeleteAuthor(string id)
        {
            var info = sqlConnect.Authors.FirstOrDefault(b => b.Author_ID.Equals(id));
            sqlConnect.Remove(info);
            sqlConnect.SaveChanges();
            return Ok();
        }

        [HttpPost("AddAuthor")]
        public IActionResult AddAuthor([FromBody] Author author)
        {
            sqlConnect.Authors.Add(author);
            sqlConnect.SaveChanges();
            return Ok();
        }

        [HttpPost("ModifAuthor")]
        public IActionResult ModifAuthor([FromBody] Author author)
        {
            var info = sqlConnect.Authors.FirstOrDefault(b => b.Author_ID.Equals(author.Author_ID));
            info.Author_Name = author.Author_Name;
            info.Author_Password = author.Author_Password;
            info.Author_Phone = author.Author_Phone;
            info.Author_Sex = author.Author_Sex;
            info.Author_Email = author.Author_Email;
            info.Author_Education = author.Author_Education;
            info.Author_Address = author.Author_Address;
            info.Author_Dec = author.Author_Dec;
            sqlConnect.Update(info);
            sqlConnect.SaveChanges();
            return Ok();
        }

        [HttpGet("GetAllEditor")]
        public IActionResult GetAllEditor()
        {
            var info = sqlConnect.Editors;
            return Ok(info);
        }

        [HttpPost("AddEditor")]
        public IActionResult AddEditor([FromBody] Editor editor)
        {
            sqlConnect.Editors.Add(editor);
            sqlConnect.SaveChanges();
            return Ok();
        }

        [HttpGet("GetEditorID")]
        public IActionResult GetEditorID(string id)
        {
            var info = sqlConnect.Editors.FirstOrDefault(b => b.Editor_ID.Equals(id));
            return Ok(info);
        }

        [HttpGet("DeleteEditor")]
        public IActionResult DeleteEditor(string id)
        {
            var info = sqlConnect.Editors.FirstOrDefault(b => b.Editor_ID.Equals(id));
            sqlConnect.Remove(info);
            sqlConnect.SaveChanges();
            return Ok();
        }

        [HttpGet("GetAllExpert")]
        public IActionResult GetAllExpert()
        {
            var info = sqlConnect.Expert;
            return Ok(info);
        }

        [HttpPost("AddExpert")]
        public IActionResult AddExpert([FromBody] Expert expert)
        {
            sqlConnect.Expert.Add(expert);
            sqlConnect.SaveChanges();
            return Ok();
        }

        [HttpGet("GetExpertID")]
        public IActionResult GetExpertID(string id)
        {
            var info = sqlConnect.Expert.FirstOrDefault(b => b.Expert_ID.Equals(id));
            return Ok(info);
        }

        [HttpGet("DeleteExpert")]
        public IActionResult DeleteExpert(string id)
        {
            var info = sqlConnect.Expert.FirstOrDefault(b => b.Expert_ID.Equals(id));
            sqlConnect.Remove(info);
            sqlConnect.SaveChanges();
            return Ok();
        }

        [HttpGet("GetAllManuscript")]
        public IActionResult GetAllManuscript()
        {
            var info= sqlConnect.ManuscriptState.Where(b => b.Manuscript_State!=("稿件编辑中"))
                .Join(
                    sqlConnect.Manuscript,
                    state => state.Manuscript_ID,
                    mans => mans.Manuscript_ID,
                    (state, mans) => new 
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
                    (mans, col) => new 
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

    }
}