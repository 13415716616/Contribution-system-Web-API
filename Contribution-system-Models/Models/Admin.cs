using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Contribution_system_Models.Models
{
    public class Admins
    {
        /// <summary>
        /// 管理员ID
        /// </summary>
        [Key]
        public string Admin_ID { get; set; }

        /// <summary>
        /// 管理员密码
        /// </summary>
        public string Admin_Password { get; set; }

        /// <summary>
        /// 管理员名称
        /// </summary>
        public string Admin_Name { get; set; }
    }
}
