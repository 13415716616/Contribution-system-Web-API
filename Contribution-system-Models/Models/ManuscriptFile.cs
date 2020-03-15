using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Contribution_system_Models.Models
{
    public class ManuscriptFile
    {
        [Key]
        public int ManuscriptFile_ID { get; set; }

        public string ManuscriptFile_Name { get; set; }

        public string ManuscriptFile_Path { get; set; }

        public string ManuscriptFile_Type { get; set; }

        public int Manuscript_ID { get; set; }
    }
}
