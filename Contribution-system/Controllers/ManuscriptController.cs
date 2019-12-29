using System;
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
        public IActionResult AddManuscript(Contribution_system_Models.Models.Manuscript manuscript)
        {
            try { 
                manuscript.Manuscript_Status = ManuscriptMode.WriteInfo;
                manuscript.Author_ID = User.FindFirst(ClaimTypes.Name)?.Value;
               // sqlConnect.Manuscript.Add(manuscript);
                //sqlConnect.SaveChanges();
               // var a = $"{ username }";
                return Ok();
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
            var userid = User.FindFirst(ClaimTypes.Name)?.Value;
            var path = InfoPath.ModelsPath + "Manuscript\\" + userid;
            var a= Directory.Exists(path);
            return BadRequest();
        }
    }
}