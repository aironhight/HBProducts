﻿using HBProducts.ViewModels;
using System;

namespace HBProducts.Models
{
    public class ProductDataWeb
    {
        private String dataType;
        private String dataValue;
        private Boolean isUrl;

        public ProductDataWeb(String dataType, String dataValue, Boolean isUrl)
        {
            this.dataType = dataType;
            this.dataValue = dataValue;
            this.isUrl = isUrl;
        }

        public String GetType()
        {
            return dataType;
        }

        public String GetValue()
        {
            return dataValue;
        }

        public Boolean IsUrl()
        {
            return isUrl;
        }

        public String Value
        {
            get { return dataValue; }
        }

        public String Type
        {
            get { return dataType; }
        }
    }
}
