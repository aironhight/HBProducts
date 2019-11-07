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
            Preferences.Set("userData", JsonConvert.SerializeObject(customer));
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
    }
}
