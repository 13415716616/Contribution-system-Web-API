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

    public class ManuscriptAuthor
    {
        public int Manscript_ID { get; set; }

        public string Author_name { get; set; }

        public string Author_sex { get; set; }

        public string Author_Phone { get; set; }

        public string Author_Address { get; set; }

        public string Author_dec { get; set; }
    }
}
