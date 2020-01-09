using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Contribution_system_Models.WebModel
{
    public class LoginInfo
    {
        public string username { get; set; }
        
        public string password { get; set; }
    }

    static public class InfoPath
    {
        static public string ModelsPath = getpath.path(System.Environment.CurrentDirectory) + "\\Contribution-system-Models\\";

        static public string RouterInfo = ModelsPath + "Information\\RouterInfo.json";

        static public string Role = ModelsPath + "Information\\Role.json";

        static public string AuthorRouterInfo=ModelsPath + "Information\\AuthorRouterInfo.json";

        static public string EditorRouterinfo= ModelsPath + "Information\\EditorRouterinfo.json";

        static public string AuthorRole = ModelsPath + "Information\\AuthorRole.json";

        static public class getpath
        {
            static public string path(string path)
            {
                var info1 = new DirectoryInfo(path);
                string a = info1.Parent.FullName;
                return a;
            }
        }
    }

    public class UserRoleInfo
    {
        public string name { get; set; }
        public string id { get; set; }
        public string avatar { get; set; }
        public JObject role { get; set; }
    }
}
