using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using static Contribution_system_Models.WebModel.ManuscriptModel;

namespace Contribution_system_Models.Models
{
    public class DraftManuscript
    {
        [Key]
        public int DraftManuscript_ID { get; set; }
        public string Author_ID { get; set; }
        public string DraftManuscript_Title { get; set; }
        public string DraftManuscript_Etitle { get; set; }
        public string DraftManuscript_Keyword { get; set; }
        public string DraftManuscript_Abstract { get; set; }
        public string DraftManuscript_Reference { get; set; }
        public string DraftManuscript_Text { get; set; }
        public string DraftManuscript_MainDataPath { get; set; }
        public string DraftManuscript_OtherDataPath { get; set; }
        public ManuscriptMode DraftManuscript_Status { get; set; }
        public string Author_name { get; set; }
        public string Author_sex { get; set; }
        public string Author_Phone { get; set; }
        public string Author_Address { get; set; }
        public string Author_dec { get; set; }
        public string Edit_Time { get; set; }
    }
}
