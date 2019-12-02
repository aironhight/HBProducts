using HBProducts.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace HBProducts.Services
{
    class Settings
    {
        public static void SaveUserData(Customer customer)
        {
            SaveToPreferences("userData", customer);
        }

        public static Customer GetUserData()
        {
            String userData = Preferences.Get("userData", "");
            if (userData.Length == 0) //Return an empty customer if there isnt any customer data
                return new Customer("", "", "", "", "");

            Customer toReturn = JsonConvert.DeserializeObject<Customer>(userData);
            return JsonConvert.DeserializeObject<Customer>(userData);
        }

        public static void ClearUserData()
        {
            Preferences.Remove("userData");
        }

        public static void AddToFavorites(Product product)
        {
            List<int> favList = GetFavoriteProducts(); //Get the favorite products 
             
            if (!favList.Contains(product.Id)) { 
                favList.Add(product.Id); //Add the current product to the list if it isn't there.
                SaveToPreferences("favorites", favList); //Save the new product list.
            }
        }

        public static List<int> GetFavoriteProducts()
        {
            String favorites = Preferences.Get("favorites", "");
            return (favorites.Length==0) ? new List<int>() : JsonConvert.DeserializeObject<List<int>>(favorites); //Return an empty list if there are no favorites.
        }

        //Removes a product from facorites.
        public static void RemoveProductFromFavorites(Product product)
        {
            List<int> favList = GetFavoriteProducts();
            favList.Remove(product.Id);
            SaveToPreferences("favorites", favList);
        }

        //Returns true if the product is in the favorites list, false otherwise
        public static bool ProductIsFavorite(Product product)
        {
            return GetFavoriteProducts().Contains(product.Id);
        }

        private static void SaveToPreferences(string key, object value)
        {
            Preferences.Set(key, JsonConvert.SerializeObject(value));
        }
    }
}
