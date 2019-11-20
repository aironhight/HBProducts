using HBProducts.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace HBProducts.ViewModels
{
    class FAQViewModel : BaseViewModel
    {
        private List<FAQItem> faqItems;

        public FAQViewModel() {
            faqItems = new List<FAQItem>();
            faqItems.Add(new FAQItem(Constants.q1, Constants.a1));
            faqItems.Add(new FAQItem(Constants.q2, Constants.a2));
            faqItems.Add(new FAQItem(Constants.q5, Constants.a5, "Diagram2.png"));
            faqItems.Add(new FAQItem(Constants.q4, Constants.a4));
            faqItems.Add(new FAQItem(Constants.q3, Constants.a3, "Diagram1.png"));
            
        }

        public List<FAQItem> FAQItems
        {
            get { return faqItems; }
        }
    }
}
