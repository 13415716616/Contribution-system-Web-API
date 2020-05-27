using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using Contribution_system_Models;
using Contribution_system_Models.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Contribution_system.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManuscriptReviewController : ControllerBase
    {
        SqlConnect sqlConnect;

        public ManuscriptReviewController(SqlConnect _sqlConnect)
        {
            sqlConnect = _sqlConnect;
        }
    }
}