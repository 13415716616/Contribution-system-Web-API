using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Contribution_system_Models.Models
{
    public class Layout
    {
        [Key]
        public int Layout_ID { get; set; }

        public int Manuscript_ID { get; set; }

        public string Layout_Image { get; set; }

        public string Layout_Dec { get; set; }

    }
}
