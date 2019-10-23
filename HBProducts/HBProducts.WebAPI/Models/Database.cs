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
        static readonly string connstr = String.Format("Server={0};Port={1};" +
           "User Id={2};Password={3};Database={4};Ssl Mode={5};",
           "gosho.postgres.database.azure.com", "5432", "gosho123@gosho",
           "@HB123456", "Kuramiqnko", "Require");
        private readonly NpgsqlConnection conn = new NpgsqlConnection(connstr);
        private NpgsqlCommand command;
        private NpgsqlDataReader dataReader;
        private readonly NpgsqlConnection conn2 = new NpgsqlConnection(connstr);
        private NpgsqlCommand command2;
        private NpgsqlDataReader dataReader2;


        public ProductList GetAll()
        {
            var list = new ProductList();
            Product pTemp;
            command = new NpgsqlCommand("Select p.id, m.model, t.type " +
                                            " from product p " +
                                            " inner join model m " +
                                            " on m.id = p.model_id " +
                                            " inner join type t " +
                                            " on t.id = p.type_id ", conn);
            conn.Open();
            dataReader = command.ExecuteReader();
            while (dataReader.Read())
            {
                pTemp = new Product((String)dataReader[1], (String)dataReader[2], "", GetProductDataForList((int)dataReader[0]));
                list.AddProduct(pTemp);
            }
            conn.Close();
            return list;
        }

        public Product GetProduct(int id)
        {
            Product pTemp;
            command = new NpgsqlCommand("Select p.id, m.model, t.type, p.three_d_model " +
                                            " from product p " +
                                            " inner join model m " +
                                            " on m.id = p.model_id "+
                                            " inner join type t " +
                                            " on t.id = p.type_id " +
                                            " where p.id = 5", conn);
            conn.Open();
            dataReader = command.ExecuteReader();
            dataReader.Read();
            pTemp = new Product((String)dataReader[1], (String)dataReader[2], (String)dataReader[3], GetProductData((int)dataReader[0]));
            conn.Close();
            return pTemp;
        }

        // This method is used for getting
        // the product data for a specific 
        // product from the database.
        private List<ProductData> GetProductData(int id)
        {

            var pdList = new List<ProductData>();
            command2 = new NpgsqlCommand("Select  pi.product_id, dt.datatype, pi.datavalue, pi.isurl " +
                                            " from product_info pi " +
                                            " inner join datatype dt " +
                                            " on dt.id = pi.datatype_id " +
                                            " where pi.product_id = " + id, conn2);
            conn2.Open();
            dataReader2 = command2.ExecuteReader();
            ProductData prData;
            while (dataReader2.Read())
            {
                prData = new ProductData((String)dataReader2[1], (String)dataReader2[2], (Boolean)dataReader2[3]);
                pdList.Add(prData);
            }
            conn2.Close();
            return pdList;
        }

        // This method is used for getting
        // the Thimbnail information for a 
        // specific product from the database.
        private List<ProductData> GetProductDataForList(int id)
        { 
            var pdList = new List<ProductData>();
            command2 = new NpgsqlCommand("Select  pi.product_id, dt.datatype, pi.datavalue " +
                                            " from product_info pi " +
                                            " inner join datatype dt " +
                                            " on dt.id = pi.datatype_id " +
                                            " where pi.product_id = " + id + " and dt.datatype = 'Thumbnail'", conn2);
            conn2.Open();
            dataReader2 = command2.ExecuteReader();
            ProductData prData;
            dataReader2.Read();
            prData = new ProductData((String)dataReader2[1], (String)dataReader2[2], true);
            pdList.Add(prData);
            conn2.Close();
            return pdList;
        }
    }
}