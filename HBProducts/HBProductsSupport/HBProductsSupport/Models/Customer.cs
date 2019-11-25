using System;
using System.Collections.Generic;
using System.Text;

namespace HBProductsSupport.Models
{
    public class Customer : User
    {
        private string company;
        private string email;
        private string telNum;
        private string country;

        public Customer() { }

        public Customer(string name, string company, string email, string telnum, string country) : base(name)
        {
            this.company = company;
            this.email = email;
            this.telNum = telnum;
            this.country = country;
        }

        public string Company { get { return company; } set { this.company = value; } }
        public string Email { get { return email; } set { this.email = value; } }
        public string Telephone { get { return telNum; } set { this.telNum = value; } }
        public string Country { get { return country; } set { this.country = value; } }
    }
}
