using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Contribution_system_Models.Models
{
    public class Author
    {
        [Key]
        public string Author_ID { get; set; }

        public string Author_Name { get; set; }

        public string Author_Education { get; set; }

        public string Author_Password { get; set; }

        public string Author_Phone { get; set; }

        public string Author_Email { get; set; }

        public string Author_Dec { get; set; }

        public string Author_Address { get; set; }

        public string Author_tags { get; set; }

        public string Author_Avtor { get; set; }
    }
}
