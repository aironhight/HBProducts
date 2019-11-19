using System;
using System.Collections.Generic;
using System.Text;

namespace HBProducts.Models
{
    public class Session
    {
        private string timeStarted;
        private List<Message> messageList;
        private List<User> userList;
        private int sessionID;

        public Session(string timeStarted, List<Message> mList, List<User> uList, int sessionID)
        {
            this.timeStarted = timeStarted;
            this.messageList = mList;
            this.userList = uList;
            this.sessionID = sessionID;
        }

        public int SessionID
        {
            get { return sessionID; }
            set { this.sessionID = value; }
        }

        public string TimeStarted
        {
            get { return timeStarted; }
            set { this.timeStarted = value; }
        }

        public List<Message> MessageList
        {
            get { return this.messageList; }
            set { this.messageList = value; }
        }

        public List<User> UserList
        {
            get { return this.userList; }
            set { this.userList = value; }
        }

        public void AddMessage(Message message)
        {
            messageList.Add(message);
        }
    }
}
