using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Contribution_system_Models.Models
{
    public class ManuscriptAuthor
    {
        [Key]
        public int ManuscriptAuthor_ID { get; set; }

        public string ManuscriptAuthor_Class { get; set; }

        public int Manuscript_ID { get; set; }

        public string ManuscriptAuthor_Name { get; set; }

        public string ManuscriptAuthor_Sex { get; set; }

        public string ManuscriptAuthor_Education { get; set; }

        public string ManuscriptAuthor_Phone { get; set; }

        public string ManuscriptAuthor_Email { get; set; }

        public string ManuscriptAuthor_Address { get; set; }

        //[ForeignKey("Manuscript_ID")]
        //public virtual Manuscript Manuscript { get; set; }
    }
}
