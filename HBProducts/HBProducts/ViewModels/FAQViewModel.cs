using HBProducts.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;

namespace HBProducts.ViewModels
{
    class FAQViewModel : BaseViewModel
    {
        private List<FAQItem> faqItems;

        public FAQViewModel() {
            faqItems = new List<FAQItem>();
            faqItems.Add(new FAQItem(Constants.q1, Constants.a1, "product.jpg"));
        }

        public List<FAQItem> FAQItems
        {
            get { return faqItems; }
        }
    }
}
