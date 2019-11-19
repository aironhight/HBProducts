using HBProducts.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HBProducts.Services
{
    class ChatManager
    {
        private HttpClient client;

        public ChatManager()
        {
            client = new HttpClient();
        }

        public async void sendMessage(int sessionID, Message message)
        {
            try { 
            string jsonMessage = JsonConvert.SerializeObject(message);
            var response = await client.PostAsync(Constants.ChatURI, new StringContent(jsonMessage));
            } catch (Exception ex)
            {
                Debug.WriteLine("Error at ChatManager: " + ex.StackTrace);
            }
            //int result = -1;
            //int.TryParse(response., result);
            //return result;
        }
    }
}
