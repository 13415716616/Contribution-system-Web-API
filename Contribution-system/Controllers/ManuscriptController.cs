using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Contribution_system_Models.Models;

namespace Contribution_system.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManuscriptController : ControllerBase
    {
        [HttpPost]
        public IActionResult AddManuscript(Manuscript manuscript)
        {
            Console.WriteLine(manuscript.ToString());
            return Ok();
        }
    }
}