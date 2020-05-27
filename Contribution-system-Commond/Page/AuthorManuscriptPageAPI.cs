using Contribution_system_Models;
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
        public static int AddNewManuscript(Manuscript manuscript,string author_id)
        {
            SqlConnect sqlConnect = new SqlConnect();
            try {
                ManuscriptState state = new ManuscriptState();                   
                sqlConnect.Manuscript.Add(manuscript);
                sqlConnect.SaveChanges();
                state.Manuscript_ID = manuscript.Manuscript_ID;
                state.Author_ID = author_id;
                state.Manuscript_State = "稿件编辑中";
                sqlConnect.ManuscriptState.Add(state);
                sqlConnect.SaveChanges();
                return manuscript.Manuscript_ID;
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
        public static bool AddMainManuscriptUpload(IFormFile file, int MainManuscript)
        {
            try
            {
                SqlConnect sqlConnect = new SqlConnect();
                var fpath = InfoPath.FilePath + "wwwroot/File/MainManuscript/";
                if (!Directory.Exists(fpath))
                {
                    Directory.CreateDirectory(fpath);
                }
                FileStream stream = new FileStream(InfoPath.FilePath + "wwwroot/File/MainManuscript/" + file.FileName, FileMode.Create);
                file.CopyTo(stream);
                ManuscriptFile fileinfo = new ManuscriptFile();
                fileinfo.ManuscriptFile_Name = file.Name;
                fileinfo.ManuscriptFile_Path = "/wwwroot/File/MainManuscript/" + file.FileName;
                fileinfo.ManuscriptFile_Type = "Main";
                fileinfo.Manuscript_ID = MainManuscript;
                sqlConnect.Update(fileinfo);
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
        /// <param name="OtherManuscript"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public static bool AddOtherManuscriptUpload(IFormFile file, int OtherManuscript)
        {
            try
            {
                SqlConnect sqlConnect = new SqlConnect();
                var fpath = InfoPath.FilePath + "wwwroot/File/OtherManuscript/";
                if (!Directory.Exists(fpath))
                {
                    Directory.CreateDirectory(fpath);
                }
                FileStream stream = new FileStream(InfoPath.FilePath + "wwwroot/File/OtherManuscript/" + file.FileName, FileMode.Create);
                file.CopyTo(stream);
                ManuscriptFile fileinfo = new ManuscriptFile();
                fileinfo.ManuscriptFile_Name = file.Name;
                fileinfo.ManuscriptFile_Path = "/wwwroot/File/OtherManuscript/" + file.FileName;
                fileinfo.ManuscriptFile_Type = "Other";
                fileinfo.Manuscript_ID = OtherManuscript;
                sqlConnect.Update(fileinfo);
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
