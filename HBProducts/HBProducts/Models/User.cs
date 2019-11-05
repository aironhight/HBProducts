using System;
using System.Collections.Generic;
using System.Text;

namespace HBProducts.Models
{
    public class User
    {
        protected String name { get; set; }


        public User(String name)
        {
            this.name = name;
        }
        
        public User() { }

        public string Name { get { return name; } }
    }
}
