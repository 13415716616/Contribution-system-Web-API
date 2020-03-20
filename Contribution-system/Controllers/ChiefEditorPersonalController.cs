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
    public class ChiefEditorPersonalController : ControllerBase
    {
        [HttpGet("GetChiefEditorInfo")]
        [Authorize]
        public IActionResult GetExpertInfo()
        {
            var id = User.FindFirst(ClaimTypes.Name)?.Value;
            SqlConnect sqlConnect = new SqlConnect();
            var info = sqlConnect.ChiefEditor.FirstOrDefault(b => b.ChiefEditor_ID == id);
            return Ok(info);
        }

        [HttpPost("UpdateChiefEditorInfo")]
        [Authorize]
        public IActionResult UpdateChiefEditorInfo([FromBody] ChiefEditor chiefEditor)
        {
            SqlConnect sqlConnect = new SqlConnect();
            var id= User.FindFirst(ClaimTypes.Name)?.Value;
            var info = sqlConnect.ChiefEditor.FirstOrDefault(b => b.ChiefEditor_ID == chiefEditor.ChiefEditor_ID);
            info.ChiefEditor_Sex = chiefEditor.ChiefEditor_Sex;
            info.ChiefEditor_Name = chiefEditor.ChiefEditor_Name;
            info.ChiefEditor_Phone = chiefEditor.ChiefEditor_Phone;
            info.ChiefEditor_Email = chiefEditor.ChiefEditor_Email;
            info.ChiefEditor_Education = chiefEditor.ChiefEditor_Education;
            info.ChiefEditor_Dec = chiefEditor.ChiefEditor_Dec;
            sqlConnect.Update(info);
            sqlConnect.SaveChanges();
            return Ok();
        }

        [HttpPost("UpdateChiefEditorImg")]
        [Authorize]
        public IActionResult UpdateChiefEditorImg([FromForm] IFormFile file)
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
            var info = sqlConnect.ChiefEditor.FirstOrDefault(b => b.ChiefEditor_ID == id);
            info.ChiefEditor_avtor = "/File/Image/" + file.FileName;
            sqlConnect.Update(info);
            sqlConnect.SaveChanges();
            return Ok(info.ChiefEditor_avtor);
        }

        [HttpGet("ChiefEditorInfo")]
        public IActionResult GetChiefEditorInfo()
        {
            ChiefEditorManuscriptInfo info = new ChiefEditorManuscriptInfo();
            return Ok();
        }
    }
}