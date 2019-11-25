using System;
using System.Collections.Generic;
using System.Text;

namespace HBProductsSupport.Models
{
    public class User
    {
        // The variables are protected so that they can be 
        //  accessed from any class that inherits this class
        protected string name { get; set; }

        public User(string name)
        {
            this.name = name;
        }
        
        public User() { }

        public string Name { get { return name; } set { this.name = value; } }
    }
}
