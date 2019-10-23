using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Input;


namespace HBProducts.Models
{ 
    public class ProductWeb
    {
        private String model;
        private String type;
        private String threedModel;
        private List<ProductDataWeb> dataList;
      
        public ProductWeb(string model, string type, string threedModel, List<ProductDataWeb> dataList)
        {
            this.model = model;
            this.type = type;
            this.threedModel = "http://pcm.um.edu.my/wp-content/uploads/2017/11/empty-avatar-700x480.png";
            this.dataList = dataList;

            //For testing purpose...
            ProductDataWeb imageData = new ProductDataWeb("Image", "https://www.kaeltefischer.de/sites/default/files/styles/header_bild/public/Header_Landingpage_HBProducts.jpg?itok=HKffyGX5", true);
            ProductDataWeb productData = new ProductDataWeb("Thumbnail", "https://www.hbproducts.dk/images/HBAC-2.png", true);
            ProductDataWeb manual = new ProductDataWeb("Manual", "https://www.hbproducts.dk/images/manualer/hbsc2-hv/HBSC2-SSR_Instruction%20manual_001-UK.pdf", true);
            ProductDataWeb quickStartGuide = new ProductDataWeb("Quick Start guide", "https://www.hbproducts.dk/images/manualer/hbsc2/HBSC2_-_Quick_guide-UK_009.pdf", true);
            ProductDataWeb summary = new ProductDataWeb("Description", "This is a very nice Sensor indeed.", false);

            dataList.Add(productData);
            dataList.Add(imageData);
            dataList.Add(manual);
            dataList.Add(quickStartGuide);
            dataList.Add(summary);
        }

        public string Model
        {
            get { return model; }
        }

        public string Type
        {
            get { return type; }
        }

        public string FullName
        {
            get { return model + " " + type; }
        }

        public string ThreeDModel
        { 
            get { return threedModel; }
        }

        //public ImageSource ThumbNailSource
        //{
        //    get {
        //        //Go through the datalist to find the ThumbnailURL
        //        foreach (ProductData data in dataList)
        //        {
        //            if (data.GetType().Equals("Thumbnail"))
        //                return ImageSource.FromUri(new Uri(data.GetValue()));
        //        }
        //        //Return null if it does not have any ThumbnailURL...
        //        return null;
        //    }
        //}

        //public ImageSource ProductImage
        //{
        //    get
        //    {
        //        //Go through the datalist to find the ThumbnailURL
        //        foreach (ProductData data in dataList)
        //        {
        //            if (data.GetType().Equals("Image"))
        //                return ImageSource.FromUri(new Uri(data.GetValue()));
        //        }
        //        //Return null if it does not have any ThumbnailURL...
        //        return null;
        //    }
        //}

        //public list<productdata> datalist
        //{
        //    //set { setproperty(ref datalist, value); }
        //    get { return datawithoutimages(); }
        //}

        //public List<ProductData> NoURLData
        //{
        //    get { return urlData(false); }
        //}

        //public List<ProductData> URLData
        //{
        //    get { return urlData(true); }
        //}

        //Methods for data manipulation after-----------------------------

        /**
         * FIlters the ProductData and returns ProductData which does not contain the Image and Thumbnail.
         */
        //private List<ProductData> dataWithoutImages()
        //{
        //    return dataList.Where(data => !((data.GetType().Equals("Image") || (data.GetType().Equals("Thumbnail"))))).ToList();
        //}

        /**
         * Filters out the ProductData returning data which is either with only URL's or without URL's
         * param urlData : Set to true if you want data with URL's; set to false if you want non-url data.
         */
        //private List<ProductData> urlData(bool urlData)
        //{
        //    return dataWithoutImages().Where(data => data.IsUrl() == urlData).ToList();
        //}

    }
}
