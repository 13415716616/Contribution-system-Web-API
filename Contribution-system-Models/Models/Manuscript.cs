using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using static Contribution_system_Models.WebModel.ManuscriptModel;

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

        public string Manuscript_EKeyword { get; set; }

        public string Manuscript_Abstract { get; set; }

        public string Manuscript_EAbstract { get; set; }

        public string Manuscript_Reference { get; set; }

        public string Manuscript_Content { get; set; }

        public string Manuscript_Status { get; set; }

        public int ManuscriptColumn_ID { get; set; }
        public string Time { get; set; }
    }
}
