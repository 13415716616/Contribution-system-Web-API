using System.Linq;
using System.Security.Claims;
using Contribution_system_Commond.Page;
using Contribution_system_Models;
using Contribution_system_Models.WebModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Contribution_system_Models.Models;

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

        //添加作者的Tags标签函数
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

        //获取稿件个数
        [HttpGet("GetAuthorManuscriptNum")]
        [Authorize]
        public IActionResult GetAuthorManuscriptNum()
        {
            var id = User.FindFirst((ClaimTypes.Role))?.Value;
            return Ok(PersonalPageAPI.GetAuthorManuscriptNumNum(id));
        }
        
        //获取修改的信息
        [Authorize]
        [HttpPost("UpdateAuthorInfo")]
        public IActionResult UpdateAuthorInfo([FromBody] Author author)
        {
            SqlConnect sqlConnect=new SqlConnect();
            var id=User.FindFirst(ClaimTypes.Name)?.Value;
            var info = sqlConnect.Authors.FirstOrDefault(b => b.Author_ID == id);
            info = author;
            sqlConnect.Update(info);
            return Ok();
        }
    }
}