using System;
using System.Windows.Input;
using Urho;
using Urho.Resources;
using Xamarin.Forms;

namespace HBProducts.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public AboutViewModel()
        {
            Title = "About";
        }

        public string AboutCompany
        {
            get
            {
                return Constants.aboutCompany1 + Environment.NewLine + Environment.NewLine
                    + Constants.aboutCompany2 + Environment.NewLine + Environment.NewLine
                    + Constants.aboutCompany3;
            }
        }

        public string Paragraph1Label
        {
            get { return Constants.aboutParagraph1Label; }
        }

        public string Paragraph1Text
        {
            get { return Constants.aboutParagraph1Text; }
        }

        public string Paragraph2Label
        {
            get { return Constants.aboutParagraph2Label; }
        }

        public string Paragraph2Text
        {
            get { return Constants.aboutParagraph2Text; }
        }

    }
}