using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Contribution_system_Models.Models
{
    public class ManuscriptContent
    {
        [Key]
        public int Manuscript_ID { get; set; }
        public string Manuscript_Title { get; set; }
        public string Manuscript_Etitle { get; set; }
        public string Manuscript_Keyword { get; set; }
        public string Manuscript_Abstract { get; set; }
        public string Manuscript_Reference { get; set; }
        public string Manuscript_Text { get; set; }
        public string Manuscript_MainDataPath { get; set; }
        public string Manuscript_OtherDataPath { get; set; }
    }
}
