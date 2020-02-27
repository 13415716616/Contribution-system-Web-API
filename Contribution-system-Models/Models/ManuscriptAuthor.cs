using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Contribution_system_Models.Models
{
    public class ManuscriptAuthor
    {
        [Key]
        public int ManuscriptAuthor_ID { get; set; }
        public string ManuscriptAuthor_name { get; set; }
        public string ManuscriptAuthor_sex { get; set; }
        public string ManuscriptAuthor_Phone { get; set; }
        public string ManuscriptAuthor_Address { get; set; }
        public string ManuscriptAuthor_dec { get; set; }
    }
}
