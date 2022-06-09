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
    internal class CreateTest
    {
        private string _token;

        [SetUp]
        public async Task Login()
        {
            _token =await TestUtiility.GetToken();
        }

        [Test]
        public async Task SimpleCreate()
        {
            using var app = new ComplevoAssignmentWebAppFactory("InMemDbCreat1", null);
            using var client = app.CreateClientWithToken(_token);
            var newProductDto = new ProductDto
            {
                Name = "New Prod Name",
                Description = "New Prod Descr",
            };
            var content = new StringContent(JsonSerializer.Serialize(newProductDto), Encoding.UTF8, "application/json");
            var responseMessage = await client.PostAsync($"/api/Products", content);
            responseMessage.EnsureSuccessStatusCode();
            Assert.AreEqual((int)responseMessage.StatusCode, StatusCodes.Status201Created);
            var uri = responseMessage.Headers.GetValues("Location").FirstOrDefault();
            var savedProductResponce = await client.GetAsync(uri);
            var savedProduct =await TestUtiility.GetResultAsync<ProductDto>(savedProductResponce);

            Assert.AreEqual(newProductDto.Name, savedProduct.Name);
           

        }

        [Test]
        public async Task DuplicateIdError()
        {
            using var app = new ComplevoAssignmentWebAppFactory("InMemDbCreat2", null);
            using var client = app.CreateClientWithToken(_token);
            var newProductDto = new ProductDto
            {
                Name = "New Prod Name",
                Description = "New Prod Descr",
            };
            var content = new StringContent(JsonSerializer.Serialize(newProductDto), Encoding.UTF8, "application/json");
            var responseMessage = await client.PostAsync($"/api/Products", content);
            responseMessage.EnsureSuccessStatusCode();

            var dublIdProductDto = new ProductDto
            {
                Id = 1,
                Name = "Another Prod Name",
                Description = "Another Prod Descr",
            };
            var contentDublId = new StringContent(JsonSerializer.Serialize(dublIdProductDto), Encoding.UTF8, "application/json");
            var responseDublMessage = await client.PostAsync($"/api/Products", contentDublId);
            Assert.AreEqual(StatusCodes.Status409Conflict,(int)responseDublMessage.StatusCode);


        }

        [Test]
        public async Task DuplicateNameError()
        {
            using var app = new ComplevoAssignmentWebAppFactory("InMemDbCreat3", null);
            using var client = app.CreateClientWithToken(_token);
            var newProductDto = new ProductDto
            {
                Name = "New Prod Name",
                Description = "New Prod Descr",
            };
            var content = new StringContent(JsonSerializer.Serialize(newProductDto), Encoding.UTF8, "application/json");
            var responseMessage = await client.PostAsync($"/api/Products", content);
            responseMessage.EnsureSuccessStatusCode();

            var dublIdProductDto = new ProductDto
            {                
                Name = "New Prod Name",
                Description = "Another Prod Descr",
            };
            var contentDublId = new StringContent(JsonSerializer.Serialize(dublIdProductDto), Encoding.UTF8, "application/json");
            var responseDublMessage = await client.PostAsync($"/api/Products", contentDublId);
            Assert.AreEqual(StatusCodes.Status409Conflict, (int)responseDublMessage.StatusCode);

        }
    }
}
