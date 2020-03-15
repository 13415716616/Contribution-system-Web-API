using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Contribution_system_Models.Models
{
    public class ExpertReview
    {
        [Key]
        public int ExpertReview_ID { get; set; }

        public int Manuscript_ID { get; set; }

        public string Expert_ID { get; set; }

        public string SelectedTopic { get; set; }

        public string Methon { get; set; }

        public string Content { get; set; }

        public string Data { get; set; }

        public string Value { get; set; }

        public string Other { get; set; }

        public string Comment { get; set; }

        public string Suggest { get; set; }

        public string Opinion { get; set; }

        public string Review_Status { get; set; }

        public string Review_Time { get; set; }
    }
}
