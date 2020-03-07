﻿using Contribution_system_Models;
using Contribution_system_Models.Models;
using System;
using System.Collections.Generic;
using System.Text;
using static Contribution_system_Models.WebModel.ManuscriptModel;
using System.IO;
using Microsoft.AspNetCore.Http;
using System.Linq;
using Contribution_system_Models.WebModel;

namespace Contribution_system_Commond.Page
{
    public static class AuthorManuscriptPageAPI
    {
        /// <summary>
        /// 像数据库中添加新的稿件信息
        /// </summary>
        /// <param name="manuscript"></param>
        /// <param name="author_id">作者ID</param>
        /// <returns></returns>
        public static int AddNewManuscript(DraftManuscript manuscript,string author_id)
        {
            SqlConnect sqlConnect = new SqlConnect();
            try {
                manuscript.DraftManuscript_Status = ManuscriptMode.WriteInfo;
                manuscript.Author_ID = author_id;
                manuscript.Edit_Time = DateTime.Now.ToString();
                sqlConnect.DraftManuscript.Add(manuscript);
                sqlConnect.SaveChanges();
                return manuscript.DraftManuscript_ID;
            }catch(Exception e)
            {
                Console.WriteLine(e);
                return 0;
    }
}

        /// <summary>
        /// 将主要稿件上传到文件进行保存
        /// </summary>
        /// <param name="file">上传的文件</param>
        /// <param name="author_id">上传到稿件ID</param>
        /// <param name="userid">作者的ID</param>
        /// <returns></returns>
        public static bool AddMainManuscriptUpload(IFormFile file, int Manuscript_id, string userid)
        {
            try
            {
                SqlConnect sqlConnect = new SqlConnect();
                var manuscript = sqlConnect.DraftManuscript.FirstOrDefault(b => b.DraftManuscript_ID == Manuscript_id);
                var fpath = InfoPath.ModelsPath + "wwwroot/File/Manuscript/" + userid;
                if (!Directory.Exists(fpath))
                {
                    Directory.CreateDirectory(fpath);
                }
                var spath = fpath + "/" + Manuscript_id;
                if (!Directory.Exists(spath))
                {
                    Directory.CreateDirectory(spath);
                }
                var mpath = spath + "/主要稿件/";
                if (!Directory.Exists(mpath))
                {
                    Directory.CreateDirectory(mpath);
                }
                FileStream stream = new FileStream(InfoPath.ModelsPath + "wwwroot/File/Manuscript/" + userid + "/" + Manuscript_id + "/主要稿件/" + file.FileName, FileMode.Create);
                file.CopyTo(stream);
                manuscript.DraftManuscript_MainDataPath = "wwwroot/File/Manuscript/" + userid + "/" + Manuscript_id + "/主要稿件/" + file.FileName;
                manuscript.DraftManuscript_Status = ManuscriptMode.UploadFile;
                manuscript.Edit_Time = DateTime.Now.ToString();
                sqlConnect.Update(manuscript);
                sqlConnect.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return false;
        }

        /// <summary>
        /// 将其他稿件信息上传到文件进行保存
        /// </summary>
        /// <param name="file"></param>
        /// <param name="Manuscript_id"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public static bool AddOtherManuscriptUpload(IFormFile file, int Manuscript_id, string userid)
        {
            try
            {
                SqlConnect sqlConnect = new SqlConnect();
                var manuscript = sqlConnect.DraftManuscript.FirstOrDefault(b => b.DraftManuscript_ID == Manuscript_id);
                var fpath = InfoPath.ModelsPath + "wwwroot/File/Manuscript/" + userid;
                if (!Directory.Exists(fpath))
                {
                    Directory.CreateDirectory(fpath);
                }
                var spath = fpath + "/" + Manuscript_id;
                if (!Directory.Exists(spath))
                {
                    Directory.CreateDirectory(spath);
                }
                var mpath = spath + "/其他资料/";
                if (!Directory.Exists(mpath))
                {
                    Directory.CreateDirectory(mpath);
                }
                FileStream stream = new FileStream(InfoPath.ModelsPath + "wwwroot/File/Manuscript/" + userid + "/" + Manuscript_id + "/其他资料/" + file.FileName, FileMode.Create);
                file.CopyTo(stream);
                manuscript.DraftManuscript_OtherDataPath = "wwwroot/File/Manuscript/" + userid + "/" + Manuscript_id + "/其他资料/" + file.FileName;
                manuscript.DraftManuscript_Status = ManuscriptMode.UploadFile;
                manuscript.Edit_Time = DateTime.Now.ToString();
                sqlConnect.Update(manuscript);
                sqlConnect.SaveChanges();
                return true ;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return false;
        }

        public static bool AddManuscriptAuthor(Contribution_system_Models.WebModel.ManuscriptAuthor author)
        {
            try
            {
                SqlConnect sqlConnect = new SqlConnect();
                var info = sqlConnect.DraftManuscript.FirstOrDefault(b => b.DraftManuscript_ID.Equals(author.Manscript_ID));
                info.Author_name = author.Author_name;
                info.Author_sex = author.Author_sex;
                info.Author_Phone = author.Author_Phone;
                info.Author_Address = author.Author_Address;
                info.Author_dec = author.Author_dec;
                info.DraftManuscript_Status = ManuscriptMode.Complete;
                info.Edit_Time = DateTime.Now.ToString();
                sqlConnect.Update(info);
                sqlConnect.SaveChanges();
                ManuscriptReview review = new ManuscriptReview();
                review.ManuscriptReview_Title = info.DraftManuscript_Title;
                review.ManuscriptReview_Etitle = info.DraftManuscript_Etitle;
                review.ManuscriptReview_Keyword = info.DraftManuscript_Keyword;
                review.ManuscriptReview_Reference = info.DraftManuscript_Reference;
                review.ManuscriptReview_Abstract = info.DraftManuscript_Abstract;
                review.ManuscriptReview_Text = info.DraftManuscript_Text;
                review.ManuscriptReview_MainDataPath = info.DraftManuscript_MainDataPath;
                review.ManuscriptReview_OtherDataPath = info.DraftManuscript_OtherDataPath;
                review.Author_name = info.Author_name;
                review.Author_Phone = info.Author_Phone;
                review.Author_sex = info.Author_sex;
                review.Author_Address = info.Author_Address;
                review.Author_dec = info.Author_dec;
                review.Author_ID = info.Author_ID;
                review.ManuscriptReview_Status = "等待编辑审查";
                review.ManuscriptReview_Time = DateTime.Now.ToString();
                sqlConnect.ManuscriptReview.Add(review);
                // sqlConnect.Manuscript.Remove(info);
                sqlConnect.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return false;
        }
    }
}