using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Complevo.Assesment.Data;
using Complevo.Assesment.Data.Entities;
using Complevo.Assesment.Services.Dto;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Complevo.Assesment.Test
{
    public static class TestUtiility
    {
        public static void SeedTestProducts(ApplicationContext context, int count)
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

        public static async Task<string> GetToken()
        {

            using var app = new ComplevoAssignmentWebAppFactory("LoginDb", null);
            using var client = app.CreateDefaultClient();
            var userLogin = new LoginDto() { UserName = "User1", Password = "password1" };
            var content = new StringContent(JsonSerializer.Serialize(userLogin), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/api/Account/Login", content);
            var result = await response.Content.ReadAsStringAsync();
            var document = JsonDocument.Parse(result);
            var token = document.RootElement.GetProperty("token").ToString();
            return token;
        }
    }

    public static class TestUtiilityExtension
    {
        public static HttpClient SetAuth(this HttpClient client, string token)
        {
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            return client;
        }
    }
}
