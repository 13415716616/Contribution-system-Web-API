using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Contribution_system_Models.Models
{
    public class ExpertFiled
    {
        [Key]
        public int Filed_ID { get; set; }

        public string Filed_Name { get; set; }

        public string Filed_Dec { get; set; }
    }
}
