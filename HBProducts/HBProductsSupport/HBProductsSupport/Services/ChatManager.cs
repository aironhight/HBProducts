using HBProductsSupport.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace HBProductsSupport.Services
{
    class ChatManager
    {
        private HttpClient client;

        public ChatManager()
        {
            client = new HttpClient();
        }

        public async Task<string> GetCustMessages(int sessionID, int lastMesage)
        {
            try
            {
                var response = await client.GetStringAsync(Constants.ChatURI + "GetCustMessages/"+sessionID+"/"+lastMesage);
                return JsonConvert.DeserializeObject<string>(response); //Deserializes the response into a JSON String
            }
            catch (Exception ex)
            {
                return "Error:Session Error:" + ex.Message;
            }
        }

        public async Task<string> GetUnansweredSessions()
        {
            try
            {
                var response = await client.GetStringAsync(Constants.ChatURI + "GetUnansweredSessions");
                return JsonConvert.DeserializeObject<string>(response); //Deserializes the response into a JSON String
            }
            catch(Exception ex)
            {
                return "Error:Error Loading sessions:" + ex.Message;
            }
        }

        //empid / sessionID
        public async Task<int> TakeSession(int employeeID, int sessionID)
        {
            try
            {
                var response = await client.GetAsync(Constants.ChatURI + "TakeSession/" + employeeID + "/" + sessionID);
                int responseInt = -1;
                string responseString = await response.Content.ReadAsStringAsync();
                if(Int32.TryParse(responseString, out responseInt))
                {
                    return responseInt;
                } else
                {
                    return -5;
                }
            }
            catch (Exception ex)
            {
                return -404;
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

        public async Task<string> GetEmpSessions(int empID)
        {
            try
            {
                var response = await client.GetStringAsync(Constants.ChatURI + "GetEmpSessions/" + empID);
                return JsonConvert.DeserializeObject<string>(response); //Deserializes the response into a JSON String
            }
            catch (Exception ex)
            {
                return "Error:Error Loading sessions:" + ex.Message;
            }
        }

        public async Task<int> GetEmpID(string empName)
        {
            int userID = -123;
            try
            {
                var res = await client.GetAsync(Constants.ChatURI + "GetEmpID/" + empName);
                string response = await res.Content.ReadAsStringAsync();
                //Debug.WriteLine("Kurvata: " + response);


                if (res.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    //SUCCESSFUL POST REQUEST
                    if (Int32.TryParse(response, out userID))
                    {
                        //Successful tryparse
                        return userID;
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
