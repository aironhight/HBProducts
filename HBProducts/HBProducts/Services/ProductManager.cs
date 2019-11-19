using HBProducts.Models;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace HBProducts.Services
{
    class ProductManager
    {
        private HttpClient client;

        public ProductManager()
        {
            client = new HttpClient();
        }

        public async Task<String> getProductWithId(int id)
        {
            try
            {
                var response = await client.GetStringAsync(Constants.ProductsURI + "/" + id);

                //If the response contains null.
                if (response.Contains("null")) //We check with ".Contains()" because the api returns "null" as a JSON string. The "==" operator does not work in that case.
                    return "Error:Product not found. Try again!";

                //If the request was successful...
                return JsonConvert.DeserializeObject<string>(response); //JSON string of a product.
            }
            //catch (Java.Net.UnknownHostException netConnection)
            //{
            //    //No internet connection.
            //    return "Error:No internet connection";
            //}
            //catch (Javax.Net.Ssl.SSLException)
            //{
            //    //Connection interrupted
            //    return "Error:Connection interrupted.";
            //}
            catch (Exception ex)
            {
                return "Error:" + ex.Message;
            }
        }

        public async Task<String> requestProductsAsync()
        {
            try
            {
                var response = await client.GetStringAsync(Constants.ProductsURI);
                return JsonConvert.DeserializeObject<string>(response); //Deserializes the response into a JSON String
            }
            //catch (Java.Net.UnknownHostException netConnection)
            //{
            //    //No internet connection.
            //    return "Error:Check your internet connection";
            //}
            //catch(Javax.Net.Ssl.SSLException)
            //{
            //    //Connection interrupted
            //    return "Error:Connection interrupted.";
            //}
            catch (Exception ex)
            {
                return "Error:Unexpected error: " + ex.Message.ToString();
            }
        }
    }
}
