using HBProducts.Models;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace HBProducts.Services
{
    public class ProductManager
    {
        private HttpClient client;

        public ProductManager()
        {
            client = new HttpClient();
        }

        //Requests all data for a product with a given ID
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
            catch (OperationCanceledException e)
            {
                return "Error:Timeout error while requesting product info:" + Environment.NewLine + e.Message;
            }

            catch (Exception ex)
            {
                return "Error:" + ex.Message;
            }
        }

        //Requests data for all products from the API(Thumbnail and product name/type).
        public async Task<String> requestProductsAsync()
        {
            try
            {
                var response = await client.GetStringAsync(Constants.ProductsURI);
                return JsonConvert.DeserializeObject<string>(response); //Deserializes the response into a JSON String
            }
            catch(OperationCanceledException e)
            {
                return "Error:Timeout error while requesting products info:" + Environment.NewLine + e.Message;
            }
            catch (Exception ex)
            {
                return "Error:Unexpected error while requesting products: " + ex.Message.ToString();
            }
        }
    }
}
