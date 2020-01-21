using HBProducts.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace HBProducts.Models
{
    [Serializable]
    public class Product
    {
        private String model;
        private String type;
        private String threedModel;
        private ObservableCollection<ProductData> dataList;
        private int id;
      
        public Product(string model, string type, string threedModel, ObservableCollection<ProductData> dataList, int id)
        {
            this.model = model;
            this.type = type;
            this.threedModel = threedModel;
            this.dataList = dataList;
            this.id = id;
        }

        public string Model
        {
            set { model = value; }
            get { return model; }
        }

        public string Type
        {
            set { type = value; }
            get { return type; }
        }

        public string FullName
        {
            get { return model + " " + type; }
        }

        public string ThreeDModel
        {
            set { threedModel = value; }
            get { return threedModel; }
        }

        public ObservableCollection<ProductData> DataList
        {
            set { dataList = value; }
            get { return dataList; }
        }

        public int Id
        {
            set { id =  value; }
            get { return id; }
        }

        //Methods for data manipulation starting from here-----------------------------

        /**
         * Filters out the ProductData returning data which is either with only URL's or without URL's
         * param urlData : Set to true if you want data with URL's; set to false if you want non-url data.
         */
        private List<ProductData> urlData(bool urlData)
        {
            return dataWithoutImages().Where(data => data.IsUrl == urlData).ToList();
        }


        public List<ProductData> NoURLData
        {
            get { return urlData(false); }
        }

        public List<ProductData> URLData
        {
            get { return urlData(true); }
        }
        /**
         * Returns a new ImageSource with the thumbnail for the product
         */
        public ImageSource ThumbNailSource
        {
            get { return ImageSource.FromUri(new Uri(dataList.Where(data => data.Type.Equals("Thumbnail")).First<ProductData>().Value)); }
        }

        /**
        * Returns an ImageSource with the High resolution image(big image) for the product.
        */
        public ImageSource ProductImage
        {
            get {
                return ImageSource.FromUri(new Uri(dataList.Where(data => data.Type.Equals("Image")).First<ProductData>().Value));
            }
        }

        /**
         * FIlters the ProductData and returns ProductData which does not contain the Image and Thumbnail.
         */
        private List<ProductData> dataWithoutImages()
        {
            return dataList.Where(data => !((data.Type.Equals("Image") || (data.Type.Equals("Thumbnail"))))).ToList();
        }

    }
}
