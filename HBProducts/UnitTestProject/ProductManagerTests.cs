using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HBProducts.Models;
using HBProducts.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace UnitTestProject
{
    [TestClass]
    public class ProductManagerTests
    {
        ProductManager pm = new ProductManager();

        [TestMethod]
        public async Task PrimitiveDataTest()
        {
            Task<string> answer = pm.getProductWithId(2);//Get the answer
            answer.Wait();
            Product product = JsonConvert.DeserializeObject<Product>(answer.Result);
            Assert.AreEqual("Refrigerant Switch - Low Temperature", product.Model);
            Assert.AreEqual("NH3", product.Type);
            Assert.AreEqual(2, product.Id);
        }

        [TestMethod]
        public async Task NegativeIDTest()
        {
            Task<string> answer = pm.getProductWithId(-1);//Negative ID
            answer.Wait();
            Assert.IsTrue(answer.Result.Contains("Error"));            
        }

        [TestMethod]
        public async Task HugeIDTest() //Huge ID
        {
            Task<string> answer = pm.getProductWithId(100000000);
            answer.Wait();
            Assert.IsTrue(answer.Result.Contains("Error"));
        }

        [TestMethod]
        public async Task ProductDataTest()
        {
            Task<string> answer = pm.getProductWithId(2);//Get the answer
            answer.Wait();
            Product product = JsonConvert.DeserializeObject<Product>(answer.Result);
            Assert.AreEqual(6, product.DataList.Count);
        }


        [TestMethod]
        public async Task ProductsCountTest()
        {
            Task<string> answer = pm.requestProductsAsync();
            answer.Wait();
            ProductList pl = JsonConvert.DeserializeObject<ProductList>(answer.Result);
            Assert.AreEqual(3, pl.ProductsCount());
        }

        [TestMethod]
        public async Task ProductsNamesTest()
        {
            Task<string> answer = pm.requestProductsAsync();
            answer.Wait();
            ProductList pl = JsonConvert.DeserializeObject<ProductList>(answer.Result);
            List<string> names = new List<string>();
            foreach(Product p in pl.Products)
            {
                names.Add(p.Model);
            }
            Assert.IsTrue(names.Contains("Refrigerant Switch 24 V AC/DC"));
            names.Remove("Refrigerant Switch 24 V AC/DC"); //Two times in the database
            Assert.IsTrue(names.Contains("Refrigerant Switch - Low Temperature"));
            Assert.IsTrue(names.Contains("Refrigerant Switch 24 V AC/DC"));
        }

        [TestMethod]
        public async Task NoInternetTest()
        {
            Task<string> answer = pm.requestProductsAsync();
            answer.Wait();
            Task<string> answer2 = pm.getProductWithId(2);
            answer2.Wait();

            Assert.IsTrue(answer.Result.Contains("Error"));
            Assert.IsTrue(answer2.Result.Contains("Error"));
        }

    }
}
