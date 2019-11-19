﻿using HBProducts.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HBProducts.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FAQPage : ContentPage
    {
        private FAQViewModel viewmodel;
        public FAQPage()
        {
            InitializeComponent();
            viewmodel = new FAQViewModel();
        }

    }
}