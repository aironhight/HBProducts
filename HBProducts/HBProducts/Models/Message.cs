using System;
using System.Collections.Generic;
using System.Text;

namespace HBProducts.Models
{
    public class Message
    {
        private User sender { get; }
        private String text { get; }
        private String timeSend { get; }

        public Message(User sender, String text, String timeSend)
        {
            this.sender = sender;
            this.text = text;
            this.timeSend = timeSend;
        }


    }
}
