﻿using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace HBProducts.Models
{
    class FAQItem
    {
        private string question;
        private string answer;
        private string imagePath;

        public FAQItem(string question, string answer, string imagePath)
        {
            this.question = question;
            this.answer = answer;
            this.imagePath = imagePath;
        }

        public string Question
        {
            get { return question; }
        }

        public string Answer
        {
            get { return answer; }
        }

        public ImageSource Image
        {
            get {
                    return ImageSource.FromFile(imagePath);
            }
        }
    }
}
