using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Complevo.Assesment.Data;
using Complevo.Assesment.Data.Entities;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Complevo.Assesment.Test
{
    public static class TestUtiility
    {
        public static void SeedTestProducts(ProductContext context, int count)
        {
            var products = Enumerable.Range(1, count).Select(x => new Product()
            {
                Name = $"Test Product #{x}",
                Description = $"Test Product Description #{x}"
            });
            context.AddRange(products);
        }

        public static async Task<T> GetResultAsync<T>(HttpResponseMessage responce)
        {
            responce.EnsureSuccessStatusCode();
            var stringContent = await responce.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(stringContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
    }
}
