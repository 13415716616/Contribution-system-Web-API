using System;
using System.Collections.Generic;
using System.Text;

namespace Contribution_system_Models.Models
{
    public class ManuscriptSubmission
    {
        public int Submission_ID { get; set; }

        public int Manusript_ID { get; set; }

        public string Author_ID { get; set; }

        public string Editor_ID { get; set; }

        public string Expert_ID { get; set; }

        public string ChiefEditor_ID { get; set; }

        public string Submission_State { get; set; }
    }
}
