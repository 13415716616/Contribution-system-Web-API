using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contribution_system_Models;
using Contribution_system_Models.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Contribution_system.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class OtherAdminController : ControllerBase
    {
        SqlConnect sqlConnect;

        public OtherAdminController(SqlConnect sqlConnect)
        {
            this.sqlConnect = sqlConnect;
        }

        [HttpPost("AddExpertFiled")]
        public IActionResult AddExpertFiled([FromBody] ExpertFiled filed)
        {
            sqlConnect.ExpertFiled.Add(filed);
            sqlConnect.SaveChanges();
            return Ok();
        }

        [HttpGet("GetExpertFiled")]
        public IActionResult GetExpertFiled()
        {
            var info = sqlConnect.ExpertFiled;
            return Ok(info);
        }

        [HttpGet("GetExpertFiledID")]
        public IActionResult GetExpertFiledID(int id)
        {
            var info = sqlConnect.ExpertFiled.FirstOrDefault(b=>b.Filed_ID.Equals(id));
            return Ok(info);
        }

        [HttpPost("UpdateFiled")]
        public IActionResult UpdateFiled([FromBody] ExpertFiled filed)
        {
            var info = sqlConnect.ExpertFiled.FirstOrDefault(b => b.Filed_ID.Equals(filed.Filed_ID));
            info.Filed_Name = filed.Filed_Name;
            info.Filed_Dec = filed.Filed_Dec;
            sqlConnect.Update(info);
            sqlConnect.SaveChanges();
            return Ok();
        }

        [HttpGet("DeleteFiled")]
        public IActionResult DeleteFiled(int id)
        {
            var info = sqlConnect.ExpertFiled.FirstOrDefault(b => b.Filed_ID.Equals(id));
            sqlConnect.Remove(info);
            sqlConnect.SaveChanges();
            return Ok();
        }

        [HttpPost("AddColumn")]
        public IActionResult AddColumn([FromBody] ManuscriptColumn column)
        {
            sqlConnect.ManuscriptColumn.Add(column);
            sqlConnect.SaveChanges();
            return Ok();
        }

        [HttpGet("GetColumn")]
        public IActionResult GetColumn()
        {
            var info = sqlConnect.ManuscriptColumn;
            return Ok(info);
        }

        [HttpGet("GetColumnID")]
        public IActionResult GetColumnID(int id)
        {
            var info = sqlConnect.ManuscriptColumn.FirstOrDefault(b => b.ManuscriptColumn_ID.Equals(id));
            return Ok(info);
        }

        [HttpPost("UpdateColumn")]
        public IActionResult UpdateColumn([FromBody] ManuscriptColumn column)
        {
            var info = sqlConnect.ManuscriptColumn.FirstOrDefault(b => b.ManuscriptColumn_ID.Equals(column.ManuscriptColumn_ID));
            info.ManuscriptColumn_Name = column.ManuscriptColumn_Name;
            info.ManuscriptColumn_Dec = column.ManuscriptColumn_Dec;
            sqlConnect.Update(info);
            sqlConnect.SaveChanges();
            return Ok();
        }

        [HttpGet("DeleteColumn")]
        public IActionResult DeleteColumn(int id)
        {
            var info = sqlConnect.ManuscriptColumn.FirstOrDefault(b => b.ManuscriptColumn_ID.Equals(id));
            sqlConnect.Remove(info);
            sqlConnect.SaveChanges();
            return Ok();
        }
    }
}