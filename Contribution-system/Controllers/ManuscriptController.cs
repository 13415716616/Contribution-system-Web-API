using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Contribution_system_Models.Models;
using Microsoft.AspNetCore.Authorization;
using static Contribution_system_Models.WebModel.Manuscript;
using System.Security.Claims;

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
               // var a = $"{ username }";
                return Ok();
            }catch(Exception e)
            {
                Console.WriteLine(e);
                return BadRequest();
            }
        }
    }
}