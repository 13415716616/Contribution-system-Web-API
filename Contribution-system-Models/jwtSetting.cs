using System;
using System.Collections.Generic;
using System.Text;

namespace Contribution_system_Models
{
    public class jwtSetting
    {
        public string Issuer { get; set; }

        public string Audience { get; set; }

        public string SecurityKey { get; set; }
    }
}
