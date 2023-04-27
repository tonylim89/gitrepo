using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using ProjectDotNet.Models;

namespace ProjectDotNet.Data
{
    public class PurchasesData
    {
        public static Dictionary<int, List<Purchases>> GetPurchases()
        {
            Dictionary<int, List<Purchases>> purchases = new Dictionary<int, List<Purchases>>();
            string connectionString = @"Server=(local);Database=eCartdb;Integrated Security=true;encrypt=false";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sql = @"SELECT cp.PurchaseDate, cp.CustomerId, cp.ProductId, p.ProductName, p.ProductDescription, cp.ActivationCode, cp.ProductRating
                    FROM CustomerPurchases cp, Product p
                    WHERE cp.ProductId = p.ProductId";

                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Purchases purchase = new Purchases()
                    {
                        PurchaseDate = (DateTime)reader["PurchaseDate"],
                        CustomerId = (int)reader["CustomerId"],
                        ProductId = (int)reader["ProductId"],
                        ProductName = (string)reader["ProductName"],
                        ProductDescription = (string)reader["ProductDescription"],
                        ActivationCode = (string)reader["ActivationCode"],
                        ProductRating = (int)reader["ProductRating"]
                    };

                    if (!purchases.ContainsKey(purchase.ProductId))
                    {
                        purchases[purchase.ProductId] = new List<Purchases>();
                    }
                    purchases[purchase.ProductId].Add(purchase);
                }
                return purchases;
            }
        }
    }
}
