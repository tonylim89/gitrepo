using Microsoft.Data.SqlClient;
using ProjectDotNet.Models;
namespace ProjectDotNet.Data
{
    public class ProductList
    {
        
        public static List<Product> GetProducts()
        {
            List<Product> products = new List<Product>();
            string connectionString = @"Server=(local);Database=eCartdb;Integrated Security=true;encrypt=false";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sql = @"SELECT ProductId, ProductName, ProductDescription, ProductImgSrc, ProductPrice
                            FROM Product";

                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Product product = new Product()
                    {
                        ProductId = (int)reader["ProductId"],
                        ProductName = (string)reader["ProductName"],
                        ProductDescription = (string)reader["ProductDescription"],
                        ProductImgSrc = (string)reader["ProductImgSrc"],
                        ProductPrice = (int)reader["ProductPrice"]
                    };
                    products.Add(product);
                }
                return products;
            }
        }

        public static List<Product> filterlist(string searchquery, List<Product> products)
        {
            List<Product> filteredlist = new List<Product>();
            var iter = products.Where(x => x.ProductDescription.ToLower().Contains(searchquery.ToLower()) ||
                        x.ProductName.ToLower().Contains(searchquery.ToLower()))
                        .Select(x => x);

            foreach(Product product in iter)
            {
                filteredlist.Add(product);
            }
            return filteredlist;
        }
    }
}
