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
        public async Task SameIDTest()
        {
            Task<int> req1 = cm.GetSesionId("testing_mail@test.com", "Tester");
            req1.Wait();
            Task<int> req2 = cm.GetSesionId("testing_mail@test.com", "Tester");
            req2.Wait();
            Assert.AreEqual(req1.Result, req2.Result);
            Assert.AreEqual(req1.Result, 61);
        }

        [TestMethod]
        public async Task TestSessionTasks()
        {
            Task<string> req1 = cm.GetSessionInfo(61);
            req1.Wait();
            Session sess = JsonConvert.DeserializeObject<Session>(req1.Result);

            Assert.AreEqual("Tester", sess.Customer.Name);
            Assert.AreEqual("testing_mail@test.com", sess.Customer.Email);

            await cm.sendMessage(61, new Message(false, "This is a test", "", -1));

            req1 = cm.GetSessionInfo(61);
            req1.Wait();
            sess = JsonConvert.DeserializeObject<Session>(req1.Result);

            Assert.AreEqual("This is a test", sess.MessageList[0].Text);
        }

    }
}
