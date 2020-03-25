using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Contribution_system_Models.Models
{
    public class Message
    {
        [Key]
        public int Message_ID { get; set; }

        public string Message_Sender { get; set; }

        public string Message_Recipient { get; set; }

        public string Message_Title { get; set; }

        public string Message_Content { get; set; }

        public string Message_Time { get; set; }
    }
}
