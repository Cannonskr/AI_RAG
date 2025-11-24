using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

class Program
{
    static async Task Main()
    {
        using var client = new HttpClient();
        client.BaseAddress = new Uri("http://localhost:5032/");

        var random = new Random();

        var products = new[]
        {
            new { Name = "iPhone 15 Pro", Category = "Smartphone", Price = 41900 },
            new { Name = "Samsung Galaxy S24", Category = "Smartphone", Price = 27900 },
            new { Name = "iPad Pro 12.9", Category = "Tablet", Price = 34900 },
            new { Name = "MacBook Pro 16", Category = "Laptop", Price = 95900 },
            new { Name = "AirPods Pro 2", Category = "Audio", Price = 7990 }
        };

        for (int i = 1; i <= 50; i++)
        {
            var p = products[i % products.Length];

            var item = new
            {
                saleDate = new DateTime(2024, 1, i <= 31 ? i : i - 31, 10, 0, 0),
                productId = 100 + i,
                productName = p.Name,
                category = p.Category,
                quantity = random.Next(1, 6),
                unitPrice = p.Price,
                customerId = 3000 + i,
                note = i % 2 == 0 ? "ลูกค้าซื้อพร้อมเคส" : ""
            };

            var response = await client.PostAsJsonAsync("api/sold-items/insert", item);

            if (response.IsSuccessStatusCode)
                Console.WriteLine($"Inserted {item.productName} for Customer {item.customerId}");
            else
                Console.WriteLine($"Failed to insert {item.productName}: {response.StatusCode}");
        }

        Console.WriteLine("✅ Finished sending 50 items via API!");
    }
}