using System;
using System.Collections.Generic;
using System.Text;

namespace HBProducts.Models
{
    class ProductData
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
    }
}
