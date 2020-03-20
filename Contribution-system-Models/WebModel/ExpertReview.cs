using System;
using System.Collections.Generic;
using System.Text;

namespace Contribution_system_Models.WebModel
{
    public class ShowExpertReview
    {
        public int EditorReview_ID { get; set; }

        public int Manuscript_ID { get; set; }

        public string Manuscript_Title { get; set; }

        public string Author_ID { get; set; }

        public string Review_Time { get; set; }
    }
}
