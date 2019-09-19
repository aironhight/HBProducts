using System;
using System.Collections.Generic;
using System.Text;

namespace HBProducts.Models
{
    public enum MenuItemType
    {
        Home,
        Scan,
        Products,
        FAQ,
        ContactUs,
        About
    }
    public class HomeMenuItem
    {
        public MenuItemType Id { get; set; }

        public string Title { get; set; }
    }
}
