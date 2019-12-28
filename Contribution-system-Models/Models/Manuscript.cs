using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using static Contribution_system_Models.WebModel.Manuscript;

namespace Contribution_system_Models.Models
{
public class Manuscript
    {
        [Key]
        public int Manuscript_ID { get; set; }
        public string Author_ID { get; set; }
        public string Manuscript_Title { get; set; }
        public string Manuscript_Etitle { get; set; }
        public string Manuscript_Keyword { get; set; }
        public string Manuscript_Abstract { get; set; }
        public string Manuscript_Reference { get; set; }
        public string Manuscript_Text { get; set; }
        public ManuscriptMode Manuscript_Status { get; set; }
    }
}
