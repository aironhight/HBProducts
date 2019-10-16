using HBProducts.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace HBProducts.Models
{
    class Product : BaseViewModel
    {
        private String model;
        private String type;
        private String threedModel;
        private ArrayList dataList = new ArrayList();

        public Product(string model, string type, string threedModel, ArrayList dataList)
        {
            this.model = model;
            this.type = type;
            this.threedModel = "http://pcm.um.edu.my/wp-content/uploads/2017/11/empty-avatar-700x480.png";
            this.dataList = dataList;
       
            //For testing purpose...
            ProductData productData = new ProductData("ThumbnailURL", "https://www.hbproducts.dk/images/HBAC-2.png", true);
            dataList.Add(productData);
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

        public string ThreeDModel
        {
            set { SetProperty(ref threedModel, value); }
            get { return threedModel; }
        }

        public ImageSource ImgSource
        {
            get {
                //return ImageSource.FromUri(new Uri("https://www.hbproducts.dk/images/HBAC-2.png"));
                //Go through the datalist to find the ThumbnailURL
                foreach (ProductData data in dataList)
                {
                    if (data.GetType().Equals("ThumbnailURL"))
                        return ImageSource.FromUri(new Uri(data.GetValue()));
                }
                //Return null if it does not have any ThumbnailURL...
                return null;
                //return ImageSource.FromUri(new Uri(getThumbnailURL()));
            }
        }

        public ArrayList DataList
        {
            set { SetProperty(ref dataList, value); }
            get { return dataList; }
        }
    }
}
