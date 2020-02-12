using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Contribution_system_Models.Models
{
    public class ChiefEditor
    {
        [Key]
        public string ChiefEditor_ID { get; set; }

        public string ChiefEditor_Name { get; set; }

        public string ChiefEditor_Password { get; set; }

        public string ChiefEditor_Phone { get; set; }

        public string ChiefEditor_Email { get; set; }

        public string ChiefEditor_Dec { get; set; }
    }
}
