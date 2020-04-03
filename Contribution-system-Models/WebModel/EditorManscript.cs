using System;
using System.Collections.Generic;
using System.Text;

namespace Contribution_system_Models.WebModel
{
    public class ShowManuscript
    {
        public int Manuscript_ID { get; set; }

        public string Author_Name { get; set; }

        public string Manuscript_Title { get; set; }

        public string Manuscript_Keyword { get; set; }

        public string Manuscript_Status { get; set; }

        public string ManuscriptColumn { get; set; }

        public string Time { get; set; }

    }

    public class CommentInfo
    {       
        public int manscriptid { get; set; }
        public string name { get; set; }
        public string role { get; set; }
        public string avtor { get; set; }
        public string comment { get; set; }
        public string time { get; set; }
    }

}
