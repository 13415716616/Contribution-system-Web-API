using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Contribution_system_Models.Models
{
    public class ManuscriptReview
    {
        [Key]
        public int ManuscriptReview_ID { get; set; }
        public string Author_ID { get; set; }
        public string ManuscriptReview_Title { get; set; }
        public string ManuscriptReview_Etitle { get; set; }
        public string ManuscriptReview_Keyword { get; set; }
        public string ManuscriptReview_Abstract { get; set; }
        public string ManuscriptReview_Reference { get; set; }
        public string ManuscriptReview_Text { get; set; }
        public string ManuscriptReview_MainDataPath { get; set; }
        public string ManuscriptReview_OtherDataPath { get; set; }
        public string Author_name { get; set; }
        public string Author_sex { get; set; }
        public string Author_Phone { get; set; }
        public string Author_Address { get; set; }
        public string Author_dec { get; set; }
        public string Editor_ID { get; set; }
        public string ManuscriptReview_Status { get; set; }
        public string ManuscriptReview_First_Info { get; set; }
        public string ManuscriptReview_Second_Info { get; set; }
    }
}
