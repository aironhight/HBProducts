using HBProducts.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
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

        //Method used for getting a session ID
        public async Task<int> GetSesionId(string email, string name)
        {
            try
            {
                var response = await client.GetAsync(Constants.ChatURI + "GetChatId/" + email + "/" + name);

                // If the request is succesful
                if (response.IsSuccessStatusCode)
                {
                    // Returns the session Id that the api has return
                    string res = await response.Content.ReadAsStringAsync();
                    return Int32.Parse(res);
                }
                // If the request is unsuccesful
                else
                {
                    // Returns -12, which is handeled from the system as an error
                    return -12;
                }
            }
            // If an error occurred
            catch (Exception e)
            {
                return -123;
            }
        }

        public async Task<string> GetEmpMessages(int sessionID, int lastMesage)
        {
            try
            {
                var response = await client.GetStringAsync(Constants.ChatURI + "GetEmpMessages/"+sessionID+"/"+lastMesage);
                return JsonConvert.DeserializeObject<string>(response); //Deserializes the response into a JSON String
            }
            catch (Exception ex)
            {
                return "Error:Session Error." + ex.Message;
            }
        }

        public async Task<string> GetSessionInfo(int sessionID)
        {
            try
            {
                var response = await client.GetStringAsync(Constants.ChatURI+"GetSession/"+sessionID);
                return JsonConvert.DeserializeObject<string>(response); //Deserializes the response into a JSON String
            }
            catch(Exception ex)
            {
                return "Error:Session Error." + ex.Message;
            }
        }

        // Method used for senting messages.
        public async Task<int> sendMessage(int sessionID, Message message)
        {
            int latestMessageID = -1;
            try {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                     new MediaTypeWithQualityHeaderValue("application/json"));
                string jsonMessage = JsonConvert.SerializeObject(JsonConvert.SerializeObject(message));
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, Constants.ChatURI +"SendMessage/" +  sessionID);
                request.Content = new StringContent(jsonMessage);
                request.Content.Headers.Remove("Content-Type");
                request.Content.Headers.Add("Content-Type", "application/json");
                var res = await client.SendAsync(request);
                string response = await res.Content.ReadAsStringAsync();
                //Debug.WriteLine("Kurvata: " + response);
                

                if (res.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    //SUCCESSFUL POST REQUEST
                    if (Int32.TryParse(response, out latestMessageID))
                    {
                        //Successful tryparse
                        return latestMessageID;
                    }
                    else
                    {
                        //Unsucessful tryparse - Unexpected error ocurred
                        return -12;
                    }
                    
                }
                else
                {
                    return -200;
                }
                
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error at ChatManager: " + ex.StackTrace);
                return -200;
            }
            
        }
    }
}
