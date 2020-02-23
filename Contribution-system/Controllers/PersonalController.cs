using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Contribution_system_Commond.Page;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Contribution_system.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonalController : ControllerBase
    {
        //获取作者的个人信息
        [HttpGet]
        [Authorize]
        public IActionResult GetAuthorPersonalInfo()
        {
            try
            {
                var id = User.FindFirst(ClaimTypes.Name)?.Value;
                return Ok(PersonalPageAPI.GetAuthorInfo(id));
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("AddAuthorTags")]
        [Authorize]
        public IActionResult AddAuthorTags(string tags)
        {
            try
            {
                var id = User.FindFirst(ClaimTypes.Name)?.Value;
                PersonalPageAPI.AddAuthorTags(tags, id);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("GetCompleteManuscript")]
        public IActionResult GetCompleteManuscript()
        {
            var id= User.FindFirst(ClaimTypes.Name)?.Value;
            var list= PersonalPageAPI.GetCompleteManuscript(id);
            return Ok(list);
        }
    }
}