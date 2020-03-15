using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Contribution_system_Models.Models
{
    public class EditorReview
    {
        [Key]
        public int EditorReview_ID { get; set; }

        public int Manuscript_ID { get; set; }

        public string Editor_ID { get; set; }

        public string Editor_Type { get; set; }

        public string Editor_Opinion { get; set; }

        public string Review_Time { get; set; } 
    }
}
