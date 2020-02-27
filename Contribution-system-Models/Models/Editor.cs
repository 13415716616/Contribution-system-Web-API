using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Contribution_system_Models.Models
{
    public class Editor
    {
        [Key]
        public string Editor_ID { get; set; }

        public string Editor_Name { get; set; }

        public string Editor_Password { get; set; }

        public string Editor_Phone { get; set; }

        public string Editor_Email { get; set; }

        public string Editor_Dec { get; set; }
    }
}
