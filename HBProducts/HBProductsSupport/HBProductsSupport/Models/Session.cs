using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace HBProductsSupport.Models
{
    public class Session
    {
        private string timeStarted;
        private ObservableCollection<Message> messageList;
        private Customer customer;
        private Employee employee;
        private int sessionID;

        public Session(string timeStarted, ObservableCollection<Message> mList, Customer customer, Employee employee, int sessionID)
        {
            this.timeStarted = timeStarted;
            this.messageList = mList;
            this.customer = customer;
            this.employee = employee;
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

        public ObservableCollection<Message> MessageList
        {
            get { return this.messageList; }
            set { this.messageList = value; }
        }

        public Customer Customer
        {
            get { return this.customer; }
            set { this.customer = value; }
        }

        public Employee Employee
        {
            get { return this.employee; }
            set { this.employee = value; }
        }

        public void AddMessage(Message message)
        {
            messageList.Insert(0, message);
        }

        public int GetLatestCustomerMessageID()
        {
            if (messageList == null) return 0;
            for(int i = messageList.Count-1; i>0; i--)
            {
                if (!messageList[i].IsEmployee)
                    return messageList[i].Id;
            }
            return 0;
        }
    }
}
