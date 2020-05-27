using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Contribution_system_Models;
using Contribution_system_Models.Models;
using Contribution_system_Models.WebModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Contribution_system.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpertPersonalController : ControllerBase
    {
        //上传角色头像函数
        [HttpPost("UpdateExpertImg")]
        [Authorize]
        public IActionResult UpdateImg([FromForm] IFormFile file)
        {
            SqlConnect sqlConnect = new SqlConnect();
            var fpath = InfoPath.FilePath + "/wwwroot/File/Image/";
            if (!Directory.Exists(fpath))
            {
                Directory.CreateDirectory(fpath);
            }
            FileStream stream = new FileStream(InfoPath.FilePath + "/wwwroot/File/Image/" + file.FileName, FileMode.Create);
            file.CopyTo(stream);
            var id = User.FindFirst(ClaimTypes.Name)?.Value;
            var info = sqlConnect.Expert.FirstOrDefault(b => b.Expert_ID == id);
            info.Expert_avtor = "/File/Image/" + file.FileName;
            sqlConnect.Update(info);
            sqlConnect.SaveChanges();
            return Ok(info.Expert_avtor);
        }

        //获取专家信息函数
        [HttpGet("GetExpertInfo")]
        [Authorize]
        public IActionResult GetExpertInfo()
        {
            var id = User.FindFirst(ClaimTypes.Name)?.Value;
            SqlConnect sqlConnect = new SqlConnect();
            var info =sqlConnect.Expert.FirstOrDefault(b => b.Expert_ID == id);
            return Ok(info);
        }

        [HttpPost("UpdateExpertInfo")]
        [Authorize]
        public IActionResult UpdateExpertInfo([FromBody] Expert expert)
        {
            var id = User.FindFirst(ClaimTypes.Name)?.Value;
            SqlConnect sqlConnect = new SqlConnect();
            var info = sqlConnect.Expert.FirstOrDefault(b => b.Expert_ID == id);
            info.Expert_Name = expert.Expert_Name;
            info.Expert_Sex = expert.Expert_Sex;
            info.Expert_Education = expert.Expert_Education;
            info.Expert_Email = expert.Expert_Email;
            info.Expert_Phone = expert.Expert_Phone;
            info.Expert_Address = expert.Expert_Address;
            info.Expert_Dec = expert.Expert_Dec;
            sqlConnect.Update(info);
            sqlConnect.SaveChanges();
            return Ok();
        }
    }
}