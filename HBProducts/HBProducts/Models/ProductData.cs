using HBProducts.ViewModels;
using System;
using Xamarin.Forms;

namespace HBProducts.Models
{
    [Serializable]
    public class ProductData : BaseViewModel
    {
        private String dataType;
        private String dataValue;
        private Boolean isUrl;

        public ProductData(String dataType, String dataValue, Boolean isUrl)
        {
            this.dataType = dataType;
            this.dataValue = dataValue;
            this.isUrl = isUrl;
        }

        public Boolean IsUrl
        {
            get { return isUrl; }
            set { SetProperty(ref isUrl, value); }

        }

        public String Value
        {
            get { return dataValue; }
            set { SetProperty(ref dataValue, value); }

        }

        public String Type
        {
            get { return dataType; }
            set { SetProperty(ref dataType, value); }

        }

        public HtmlWebViewSource HtmlValue
        {
            get {
                string htmlText = Value.Replace(@"\", string.Empty);
                var html = new HtmlWebViewSource
                {
                    Html = htmlText
                };
                return html;
            }
        }
    }
}
