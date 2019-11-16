using System;
using System.Collections.Generic;
using System.Text;

namespace HBProducts.Models
{
    public class User
    {
        // The variables are protected so that they can be 
        //  accessed from any class that inherits this class
        protected string name { get; set; }
        protected bool isEmp { get; set; }

        public User(string name, bool isEmp)
        {
            this.name = name;
            this.isEmp = isEmp;
        }
        
        public User() { }

        public string Name { get { return name; } set { this.name = value; } }

        public bool IsEmp { get { return isEmp; } set { this.isEmp = value; } }
    }
}
