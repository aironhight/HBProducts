using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HBProducts.Models;
using HBProducts.WebAPI.Models;
using Newtonsoft.Json;

namespace HBProducts.WebAPI.Controllers
{
    public class ProductController : ApiController
    {
        private readonly Database obj = new Database();

        // GET: api/Product
        public string Get()
        {
            return JsonConvert.SerializeObject(obj.GetAll(), Formatting.Indented);
        }

        // GET: api/Product/5
        public string Get(int id)
        {
            return JsonConvert.SerializeObject(obj.GetProduct(id), Formatting.Indented);
        }

        // POST: api/Product
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Product/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Product/5
        public void Delete(int id)
        {
        }
    }
}
