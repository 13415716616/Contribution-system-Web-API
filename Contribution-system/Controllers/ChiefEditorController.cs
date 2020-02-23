using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contribution_system_Models;
using Contribution_system_Models.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Contribution_system.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChiefEditorController : ControllerBase
    {
        SqlConnect sqlConnect;

        public ChiefEditorController(SqlConnect sqlConnect)
        {
            this.sqlConnect = sqlConnect;
        }

        [HttpGet("GetSecondManuscript")]
        public IActionResult GetSecondManuscript()
        {
            List<ManuscriptReview> manuscripts = sqlConnect.ManuscriptReview.Where(b => b.ManuscriptReview_Second_Info == ""&&b.ManuscriptReview_Status=="等待主编审查").ToList();
            return Ok(manuscripts);
        }

        [HttpGet("CompleteManuscript")]
        public IActionResult AdoptionManuscript(int id)
        {
            var info = sqlConnect.ManuscriptReview.FirstOrDefault(b => b.ManuscriptReview_ID == id);
            CompleteManuscript manuscript = new CompleteManuscript();
            manuscript.Author_ID = info.Author_ID;
            manuscript.Manuscript_Title = info.ManuscriptReview_Title;
            manuscript.Manuscript_Etitle = info.ManuscriptReview_Etitle;
            manuscript.Manuscript_Keyword = info.ManuscriptReview_Keyword;
            manuscript.Manuscript_Abstract = info.ManuscriptReview_Abstract;
            manuscript.Manuscript_Reference = info.ManuscriptReview_Reference;
            manuscript.Manuscript_Text = info.ManuscriptReview_Text;
            manuscript.Manuscript_MainDataPath = info.ManuscriptReview_MainDataPath;
            manuscript.Manuscript_OtherDataPath = info.ManuscriptReview_OtherDataPath;
            manuscript.Author_name = info.Author_name;
            manuscript.Author_sex = info.Author_sex;
            manuscript.Author_Phone = info.Author_Phone;
            manuscript.Author_Address = info.Author_Address;
            manuscript.Author_dec = info.Author_dec;
            manuscript.Complete_Time = DateTime.Now.ToString();
            sqlConnect.CompleteManuscript.Add(manuscript);
            sqlConnect.SaveChanges();
            return Ok();
        }
    }
}