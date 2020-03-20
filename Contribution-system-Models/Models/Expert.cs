using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Contribution_system_Models.Models
{
    public class Expert
    {
        [Key]
        public string Expert_ID { get; set; }

        public string Expert_Password { get; set; }

        public string Expert_Name { get; set; }

        public string Expert_Sex { get; set; }

        public string Expert_Education { get; set; }

        public string Expert_Email { get; set; }

        public string Expert_Occupation { get; set; }

        public string Expert_Work { get; set; }

        public string Expert_Phone { get; set; }

        public string Expert_Address { get; set; }

        public string Expert_Dec { get; set; }
        
        public string Expert_avtor { get; set; }

        
    }
}
