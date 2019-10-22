using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HBProducts.Models;
using HBProducts.WebAPI.Models;

namespace HBProducts.WebAPI.Controllers
{
    public class ProductController : ApiController
    {
        private readonly Database obj = new Database();

        // GET: api/Product
        public IEnumerable<ProductList> Get()
        {
            yield return obj.GetAll();
        }

        // GET: api/Product/5
        public IEnumerable<Product> Get(int id)
        {
            yield return obj.GetProduct(id);
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
