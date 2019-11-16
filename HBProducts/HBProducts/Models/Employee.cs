using System;
using System.Collections.Generic;
using System.Text;

namespace HBProducts.Models
{
    class Employee : User
    {
        private List<int> ratings;

        public Employee(string name, bool isEmp) : base(name, isEmp)
        {
            ratings = new List<int>();
        }
    }
}
