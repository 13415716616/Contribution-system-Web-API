using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Contribution_system_Models;
using Contribution_system_Models.WebModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Contribution_system.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorPersonalController : ControllerBase
    {
        [HttpPost("UpdateAuthorImg")]
        [Authorize]
        public IActionResult UpdateAuthorImg([FromForm] IFormFile file)
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
            var info = sqlConnect.Authors.FirstOrDefault(b => b.Author_ID == id);
            // info.Author_Avtor = "/File/Image/" + file.FileName;
            sqlConnect.Update(info);
            sqlConnect.SaveChanges();
            return Ok();
        }
    }
}