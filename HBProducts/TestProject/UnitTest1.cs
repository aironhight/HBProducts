using HBProducts.Models;
using HBProducts.Services;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Tests
{
    public class Tests
    {
        private ProductManager pm;

        [SetUp]
        public void Setup()
        {
            pm = new ProductManager();
        }

        public async Task PrimitiveDataTest()
        {
            Task<string> answer = pm.getProductWithId(2);//Get the answer
            answer.Wait();
            Product product = JsonConvert.DeserializeObject<Product>(answer.Result);
            Assert.AreEqual("Refrigerant Switch - Low Temperature", product.Model);
            Assert.AreEqual("NH3", product.Type);
            Assert.AreEqual("3", product.Id);
            Assert.Fail();
        }
    }
}