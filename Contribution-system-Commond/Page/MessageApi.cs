using Contribution_system_Models;
using Contribution_system_Models.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contribution_system_Commond.Page
{
    public static class MessageApi
    {
        public static bool SystemMessage(string Message_Sender, string Recipient,string Message_Title,string Message_Content)
        {
            Message message = new Message();
            message.Message_Sender = Message_Sender;
            message.Message_Recipient = Recipient;
            message.Message_Title = Message_Title;
            message.Message_Content = Message_Content;
            SqlConnect sqlConnect = new SqlConnect();
            sqlConnect.Message.Add(message);
            sqlConnect.SaveChanges();
            return true;
        }
    }
}
