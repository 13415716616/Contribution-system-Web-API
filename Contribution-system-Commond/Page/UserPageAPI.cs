using Contribution_system_Models;
using Contribution_system_Models.Models;
using Contribution_system_Models.WebModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Contribution_system_Commond.Page
{
    public static class UserPageApi
    {
        /// <summary>
        /// 添加Author用户到数据库
        /// </summary>
        /// <param name="author"></param>
        /// <returns></returns>
        public static bool AddAuthor(Author author)
        {
            SqlConnect sqlConnect = new SqlConnect();
            try
            {
                sqlConnect.Authors.Add(author);
                sqlConnect.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        /// <summary>
        /// 获取登录角色的Token
        /// </summary>
        /// <param name="loginInfo"></param>
        /// <returns></returns>
        public static string GetLoginRoleToken(LoginInfo loginInfo)
        {
            SqlConnect sqlConnect=new SqlConnect();
            var chiefeditor = sqlConnect.ChiefEditor.FirstOrDefault(c => c.ChiefEditor_ID.Equals(loginInfo.username));
            if (chiefeditor != null && UserCommond.GetMD5Hash(chiefeditor.ChiefEditor_Password).Equals(loginInfo.password))
            {
                string token = UserCommond.SetToken(loginInfo.username, "ChiefEditor");
                return token;
            }
            var editorinfo = sqlConnect.Editors.FirstOrDefault(a => a.Editor_ID.Equals(loginInfo.username));
            if (editorinfo != null && UserCommond.GetMD5Hash(editorinfo.Editor_Password).Equals(loginInfo.password))
            {
                string token = UserCommond.SetToken(loginInfo.username, "Editor");
                return token;
            }
            var expertinfo = sqlConnect.Expert.FirstOrDefault(b => b.Expert_ID.Equals(loginInfo.username));
            if (expertinfo != null && UserCommond.GetMD5Hash(expertinfo.Expert_Password).Equals(loginInfo.password))
            {
                string token = UserCommond.SetToken(loginInfo.username, "Expert");
                return token;
            }
            var authorinfo = sqlConnect.Authors.FirstOrDefault(b => b.Author_ID.Equals(loginInfo.username));
            if (authorinfo != null && UserCommond.GetMD5Hash(authorinfo.Author_Password).Equals(loginInfo.password))
            {
                string token = UserCommond.SetToken(loginInfo.username, "Author");
                return token;
            }
            return "";
        }

        /// <summary>
        /// 根据身份，获取路由信息
        /// </summary>
        /// <param name="Role"></param>
        /// <returns></returns>
        public static string GetLoginRoleRoutor(string Role)
        {
            if (Role.Equals("ChiefEditor"))
                return System.IO.File.ReadAllText(InfoPath.ChiefEditorRouterInfo);
            if (Role.Equals("Editor"))
                return System.IO.File.ReadAllText(InfoPath.EditorRouterinfo);
            if (Role.Equals("Author"))
                return System.IO.File.ReadAllText(InfoPath.AuthorRouterInfo);
            if (Role.Equals("Expert"))
                return System.IO.File.ReadAllText(InfoPath.ExpertRouterInfo);
            else
                return "";
        }

        /// <summary>
        /// 获取前端所需要显示的信息，id为用户的ID,Role为用户的身份
        /// </summary>
        /// <param name="id"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public static string GetLoginInfo(string id,string role)
        {
            SqlConnect sqlConnect = new SqlConnect();
            if (role.Equals("ChiefEditor"))
            {
                var info = sqlConnect.ChiefEditor.FirstOrDefault(b => b.ChiefEditor_ID.Equals(id));
                UserRoleInfo userRole = new UserRoleInfo();
                userRole.id = id;
                userRole.name = info.ChiefEditor_Name;
                userRole.avatar = "/avatar2.jpg";
                Console.WriteLine(InfoPath.ModelsPath);
                userRole.role = JObject.Parse(System.IO.File.ReadAllText(InfoPath.AuthorRole));
                var s = JsonConvert.SerializeObject(userRole);
                return JsonConvert.SerializeObject(userRole);
            }
            if (role.Equals("Author"))
            {
                var info = sqlConnect.Authors.FirstOrDefault(b => b.Author_ID.Equals(id));
                UserRoleInfo userRole = new UserRoleInfo();
                userRole.id = id;
                userRole.name = info.Author_Name;
                userRole.avatar = "/avatar2.jpg";
                Console.WriteLine(InfoPath.ModelsPath);
                userRole.role = JObject.Parse(System.IO.File.ReadAllText(InfoPath.AuthorRole));
                var s = JsonConvert.SerializeObject(userRole);
                return JsonConvert.SerializeObject(userRole);
            }
            if (role.Equals("Editor"))
            {
                var info = sqlConnect.Editors.FirstOrDefault(b => b.Editor_ID.Equals(id));
                UserRoleInfo userRole = new UserRoleInfo();
                userRole.id = id;
                userRole.name = info.Editor_Name;
                userRole.avatar = "/avatar2.jpg";
                Console.WriteLine(InfoPath.ModelsPath);
                userRole.role = JObject.Parse(System.IO.File.ReadAllText(InfoPath.AuthorRole));
                var s = JsonConvert.SerializeObject(userRole);
                return JsonConvert.SerializeObject(userRole);
            }
            if (role.Equals("Expert"))
            {
                var info = sqlConnect.Expert.FirstOrDefault(b => b.Expert_ID.Equals(id));
                UserRoleInfo userRole = new UserRoleInfo();
                userRole.id = id;
                userRole.name = "奇效之";
                userRole.avatar = "/avatar2.jpg";
                Console.WriteLine(InfoPath.ModelsPath);
                userRole.role = JObject.Parse(System.IO.File.ReadAllText(InfoPath.AuthorRole));
                var s = JsonConvert.SerializeObject(userRole);
                return JsonConvert.SerializeObject(userRole);
            }
            else
                return "";
        }
    }
}
