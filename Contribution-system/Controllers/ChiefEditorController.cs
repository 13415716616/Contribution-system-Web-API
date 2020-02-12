using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    }
}