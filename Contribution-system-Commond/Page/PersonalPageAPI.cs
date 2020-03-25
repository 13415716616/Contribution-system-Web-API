using Contribution_system_Models;
using Contribution_system_Models.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Contribution_system_Models.WebModel;
using Microsoft.EntityFrameworkCore;

namespace Contribution_system_Commond.Page
{
    public static class PersonalPageAPI
    {
        /// <summary>
        /// 获取作者的个人信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Author GetAuthorInfo(string id)
        {
            SqlConnect sqlConnect = new SqlConnect();
            var info= sqlConnect.Authors.FirstOrDefault(b => b.Author_ID.Equals(id));
            return info;
        }

        public static bool AddAuthorTags(string tag,string id)
        {
            try { 
                SqlConnect sqlConnect = new SqlConnect();
                var info = sqlConnect.Authors.FirstOrDefault(b => b.Author_ID.Equals(id));
                if (info.Author_tags == null || info.Author_tags == "")
                {
                    List<string> tags = new List<string>();
                    tags.Add(tag);
                    info.Author_tags = JsonConvert.SerializeObject(tags);
                    sqlConnect.Update(info);
                    sqlConnect.SaveChanges();
                    return true;
                }
                else
                {
                    List<string> tags = JsonConvert.DeserializeObject<List<string>>(info.Author_tags);
                    tags.Add(tag);
                    info.Author_tags = JsonConvert.SerializeObject(tags);
                    sqlConnect.Update(info);
                    sqlConnect.SaveChanges();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public static AuthorManuscriptNum GetAuthorManuscriptNumNum(string id)
        {
            SqlConnect sqlConnect=new SqlConnect();
            AuthorManuscriptNum num=new AuthorManuscriptNum();
            num.DarftManuscript = sqlConnect.Manuscript.Count(b => b.Author_ID == id);
            num.ReviewsManusript = sqlConnect.ManuscriptReview.Count(b => b.Author_ID == id);
          //  num.CompleteManuscript = sqlConnect.CompleteManuscript.Count(b => b.Author_ID == id);
            return num;
        }        
    }
}
