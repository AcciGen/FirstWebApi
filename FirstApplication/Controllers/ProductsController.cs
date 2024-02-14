using Dapper;
using FirstApplication.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace FirstApplication.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ProductsController : ControllerBase
    {
        const string CONNECTIONSTRING = "Server=localhost;Port=5432;Database=Web;User Id=postgres;Password=135;";

        [HttpGet]
        public List<Product> Get()
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(CONNECTIONSTRING))
            {
                return connection.Query<Product>("Select * from products;").ToList();
            }
        }

        [HttpPost]
        public Product Create(Product newProduct)
        {
            string sql = "Insert into products (name, price) values (@name, @price);";

            using (NpgsqlConnection connection = new NpgsqlConnection(CONNECTIONSTRING))
            {
                connection.Execute(sql, new Product
                {
                    Name = newProduct.Name,
                    Price = newProduct.Price
                });

                return newProduct;
            }
        }

        [HttpDelete]
        public int Delete(string name)
        {
            string sql = $"Delete from products where name = @name";

            using (NpgsqlConnection connection = new NpgsqlConnection(CONNECTIONSTRING))
            {
                var response = connection.Execute(sql, new { Name = name });

                return response;
            }
        }

        [HttpPut]
        public Product Update(int id, Product newProduct)
        {
            string sql = $"Update products set name = @name, price = @price where id = {id}";

            using (NpgsqlConnection connection = new NpgsqlConnection(CONNECTIONSTRING))
            {
                connection.Execute(sql, new Product
                {
                    Name = newProduct.Name,
                    Price = newProduct.Price,

                });

                return newProduct;
            }
        }


        [HttpPatch]
        public int UpdatePatch(int id, string name)
        {
            string sql = $"Update products set name = @name where id = @id";

            using (NpgsqlConnection connection = new NpgsqlConnection(CONNECTIONSTRING))
            {
                var response = connection.Execute(sql, new { Name = name, Id = id });

                return response;
            }
        }
    }
}
