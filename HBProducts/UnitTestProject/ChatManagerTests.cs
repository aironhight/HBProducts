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
    public class ChatManagerTests
    {
        ChatManager cm = new ChatManager();

        [TestMethod]
        public async Task PrimitiveDataTest()
        {
            //Task<string> answer = cm.getProductWithId(2);//Get the answer
            //answer.Wait();
            //Product product = JsonConvert.DeserializeObject<Product>(answer.Result);
            //Assert.AreEqual("Refrigerant Switch - Low Temperature", product.Model);
            //Assert.AreEqual("NH3", product.Type);
            //Assert.AreEqual(2, product.Id);
        }


    }
}
