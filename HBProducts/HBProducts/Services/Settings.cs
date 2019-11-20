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
            //Preferences.Set("userData", JsonConvert.SerializeObject(customer));
            SaveToPreferences("userData", customer);
        }

        public static Customer GetUserData()
        {
            String userData = Preferences.Get("userData", "");
            if (userData.Length == 0)
                return null;

            Customer toReturn = JsonConvert.DeserializeObject<Customer>(userData);
            return JsonConvert.DeserializeObject<Customer>(userData);
        }

        public static void ClearUserData()
        {
            Preferences.Remove("userData");
        }

        public static void AddToFavorites(Product product)
        {
            String favorites = Preferences.Get("favorites", ""); //Get the preferences for favorites
            List<int> favList = GetFavoriteProducts();


            //if (favorites.Length == 0) { //If there are no favorites 
            //    favList = new List<int>();
            //    favList.Add(product.Id); //Add a new list to th
            //    SaveToPreferences("favorites", favList);
            //    return;
            //}
             
            //favList = JsonConvert.DeserializeObject<List<int>>(favorites);
            if (!favList.Contains(product.Id)) { 
                favList.Add(product.Id);
                SaveToPreferences("favorites", favList);
            }
        }

        public static List<int> GetFavoriteProducts()
        {
            String favorites = Preferences.Get("favorites", "");
            return (favorites.Length==0) ? new List<int>() : JsonConvert.DeserializeObject<List<int>>(favorites);
        }

        public static void RemoveProductFromFavorites(Product product)
        {
            List<int> favList = GetFavoriteProducts();
            favList.Remove(product.Id);
            SaveToPreferences("favorites", favList);
        }

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
