using System;
using System.Collections.Generic;
using System.Text;

namespace Contribution_system_Models.WebModel
{
    public class Manuscript
    {
         public enum ManuscriptMode
        {
            Empty=0,
            WriteInfo=1,
            UploadFile=2,
            Complete=3
        }
    }
}
