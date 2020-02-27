using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Contribution_system_Models.Models
{
    public class ManuscriptSubmitted
    {
        [Key]
        public int ManuscriptSubmitted_ID { get; set; }
        public string Editor_ID { get; set; }
        public string Author_ID { get; set; }
        public int Manuscript_ID { get; set; }
        public int ManuscriptAuthor_ID { get; set; }
        public string ManuscriptSubmitted_Status { get; set; }
        public string ManuscriptSubmitted_First_Info { get; set; }
        public string ManuscriptSubmitted_Second_Info { get; set; }   
    }
}
