using System;
using System.Collections.Generic;
using System.Text;

namespace HBProducts.Models
{
    public class Employee : User
    {
        //private List<int> ratings;

        public Employee(string name) : base(name)
        {
            //ratings = new List<int>();
        }
    }
}
