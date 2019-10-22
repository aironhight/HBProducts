using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HBProducts.Models;

namespace HBProducts.WebAPI.Models
{
    public class Database
    {
        readonly string connstr = String.Format("Server={0};Port={1};" +
           "User Id={2};Password={3};Database={4};Ssl Mode={5};",
           "gosho.postgres.database.azure.com", "5432", "gosho123@gosho",
           "@HB123456", "Kuramiqnko", "Require");
        private NpgsqlConnection conn;
        private NpgsqlCommand command;
        private NpgsqlDataReader dataReader;
        public ProductList GetAll()
        {
            var list = new ProductList();
            conn = new NpgsqlConnection(connstr);
            conn.Open();
            command = new NpgsqlCommand("Select id,model from public.Product", conn);
            dataReader = command.ExecuteReader();
            while (dataReader.Read())
            {

                list.Add(dataReader[1].ToString());
            }
            conn.Close();
            return list;
        }
    }
}