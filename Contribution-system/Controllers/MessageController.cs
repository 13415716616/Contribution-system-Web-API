using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contribution_system_Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Contribution_system.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        [HttpGet("GetMessageInfo")]
        public IActionResult GetMessageInfo(int id)
        {
            SqlConnect sqlConnect = new SqlConnect();
            var info= sqlConnect.Message.FirstOrDefault(b => b.Message_ID == id);
            return Ok(info);
        }
    }
}