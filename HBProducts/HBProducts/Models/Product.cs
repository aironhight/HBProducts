using HBProducts.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace HBProducts.Models
{
    [Serializable]
    public class Product : BaseViewModel
    {
        private String model;
        private String type;
        private String threedModel;
        private List<ProductData> dataList;
      
        public Product(string model, string type, string threedModel, List<ProductData> dataList)
        {
            this.model = model;
            this.type = type;
            this.threedModel = "http://pcm.um.edu.my/wp-content/uploads/2017/11/empty-avatar-700x480.png";
            this.dataList = dataList;

            //For testing purpose...
            ProductData imageData = new ProductData("Image", "https://www.kaeltefischer.de/sites/default/files/styles/header_bild/public/Header_Landingpage_HBProducts.jpg?itok=HKffyGX5", true);
            ProductData productData = new ProductData("Thumbnail", "https://www.hbproducts.dk/images/HBAC-2.png", true);
            ProductData manual = new ProductData("Manual", "https://www.hbproducts.dk/images/manualer/hbsc2-hv/HBSC2-SSR_Instruction%20manual_001-UK.pdf", true);
            ProductData quickStartGuide = new ProductData("Quick Start guide", "https://www.hbproducts.dk/images/manualer/hbsc2/HBSC2_-_Quick_guide-UK_009.pdf", true);
            ProductData summary = new ProductData("Description", "This is a very nice Sensor indeed.", false);

            dataList.Add(productData);
            dataList.Add(imageData);
            dataList.Add(manual);
            dataList.Add(quickStartGuide);
            dataList.Add(summary);
        }

        public string Model
        {
            set { SetProperty(ref model, value); }
            get { return model; }
        }

        public string Type
        {
            set { SetProperty(ref type, value); }
            get { return type; }
        }

        public string FullName
        {
            get { return model + " " + type; }
        }

        public string ThreeDModel
        {
            set { SetProperty(ref threedModel, value); }
            get { return threedModel; }
        }

        public ImageSource ThumbNailSource
        {
            get {
                //Go through the datalist to find the ThumbnailURL
                foreach (ProductData data in dataList)
                {
                    if (data.GetType().Equals("Thumbnail"))
                        return ImageSource.FromUri(new Uri(data.GetValue()));
                }
                //Return null if it does not have any ThumbnailURL...
                return null;
            }
        }

        public ImageSource ProductImage
        {
            get
            {
                //Go through the datalist to find the ThumbnailURL
                foreach (ProductData data in dataList)
                {
                    if (data.GetType().Equals("Image"))
                        return ImageSource.FromUri(new Uri(data.GetValue()));
                }
                //Return null if it does not have any ThumbnailURL...
                return null;
            }
        }

        public List<ProductData> DataList
        {
            set { SetProperty(ref dataList, value); }
            get { return dataWithoutImages(); }
        }

        public List<ProductData> NoURLData
        {
            get { return urlData(false); }
        }

        public List<ProductData> URLData
        {
            get { return urlData(true); }
        }

        //Methods for data manipulation after-----------------------------

        /**
         * FIlters the ProductData and returns ProductData which does not contain the Image and Thumbnail.
         */
        private List<ProductData> dataWithoutImages()
        {
            return dataList.Where(data => !((data.GetType().Equals("Image") || (data.GetType().Equals("Thumbnail"))))).ToList();
        }

        /**
         * Filters out the ProductData returning data which is either with only URL's or without URL's
         * param urlData : Set to true if you want data with URL's; set to false if you want non-url data.
         */
        private List<ProductData> urlData(bool urlData)
        {
            return dataWithoutImages().Where(data => data.IsUrl() == urlData).ToList();
        }

    }
}
