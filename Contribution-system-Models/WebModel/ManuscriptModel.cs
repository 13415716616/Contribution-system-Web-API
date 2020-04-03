using System;
using System.Collections.Generic;
using System.Text;

namespace Contribution_system_Models.WebModel
{
    public class ManuscriptModel
    {
        public enum ManuscriptMode
        {
            Empty = 0,
            WriteInfo = 1,
            UploadFile = 2,
            Complete = 3
        }
    }

    public class ManuscriptTable
    {
        public int Manuscript_ID { get; set; }

        public string Manuscript_Title { get; set; }

        public string Author_ID { get; set; }

        public string Manuscript_Status { get; set; }

        public string ManuscriptColumn { get; set; }

        public string Time { get; set; }
    }

    public class ReviewManuscriptModel
    {
        public int Manuscript_ID { get; set; }

        public string Manuscript_Name { get; set; }

        public string ManuscriptColumn_ID { get; set; }

        public string Author_ID { get; set; }

        public string Time { get; set; }

        public string File { get; set; }
    }

    public class FirstReview
    {
        public int Manuscript_ID { get; set; }
        
        public string ContentText { get; set; }
    }

}
