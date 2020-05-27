using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Contribution_system_Models.Models
{
    public class ManuscriptState
    {
        [Key]
        public int Manuscript_ID { get; set; }

        public string Author_ID { get; set; }

        public string Expert_ID { get; set; }

        public string Manuscript_State { get; set; }
   
        public string Manuscript_Result { get; set; }

        public string Manuscript_Time { get; set; }
    }
}
