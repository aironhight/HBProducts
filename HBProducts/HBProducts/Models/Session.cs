using System;
using System.Collections.Generic;
using System.Text;

namespace HBProducts.Models
{
    public class Session
    {
        private string timeStarted;
        private List<Message> messageList;
        private Customer customer;
        private Employee employee;
        private int sessionID;

        public Session(string timeStarted, List<Message> mList, Customer customer, Employee employee, int sessionID)
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

        public List<Message> MessageList
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
            messageList.Add(message);
        }
    }
}
