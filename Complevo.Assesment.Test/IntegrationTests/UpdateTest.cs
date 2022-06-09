// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Complevo.Assesment.Services.Dto;
using Microsoft.AspNetCore.Http;
using NUnit.Framework;

namespace Complevo.Assesment.Test.IntegrationTests
{
    internal class UpdateTest
    {
        private string _token;

        [SetUp]
        public async Task Login()
        {
            _token = await TestUtiility.GetToken();
        }

        [Test]
        public async Task SimpleUpdate()
        {
            int productNumber = 1;
            using var app = new ComplevoAssignmentWebAppFactory("InMemDbUpdate1", x => TestUtiility.SeedTestProducts(x, productNumber));
            using var client = app.CreateClientWithToken(_token);
            var updProductDto = new ProductDto
            {
                Id= 1,
                Name = "New Prod Name",
                Description = "New Prod Descr",
            };
            var content = new StringContent(JsonSerializer.Serialize(updProductDto), Encoding.UTF8, "application/json");
            var response = await client.PutAsync(@"/api/Products/1", content);
            Assert.AreEqual(StatusCodes.Status200OK, (int)response.StatusCode);
        }


        [Test]
        public async Task UpdateNotExist()
        {
            int productNumber = 1;
            using var app = new ComplevoAssignmentWebAppFactory("InMemDbUpdate2", x => TestUtiility.SeedTestProducts(x, productNumber));
            using var client = app.CreateClientWithToken(_token);
            var updProductDto = new ProductDto
            {
                Id = 3,
                Name = "New Prod Name",
                Description = "New Prod Descr",
            };
            var content = new StringContent(JsonSerializer.Serialize(updProductDto), Encoding.UTF8, "application/json");
            var response = await client.PutAsync(@"/api/Products/3", content);
            Assert.AreEqual(StatusCodes.Status404NotFound, (int)response.StatusCode);
        }
    }
}
