// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Complevo.Assesment.Services.Dto;
using Microsoft.AspNetCore.Http;
using NUnit.Framework;

namespace Complevo.Assesment.Test.IntegrationTests
{
    internal class ReadTest
    {
        private string _token;

        [SetUp]
        public async Task Login()
        {
            _token = await TestUtiility.GetToken();
        }

        [Test]
        public async Task GetAll()
        {
            int productNumber = 100;
            using var app = new ComplevoAssignmentWebAppFactory("InMemDbRead1", x => TestUtiility.SeedTestProducts(x, productNumber));
            using var client = app.CreateClientWithToken(_token);
            var response = await client.GetAsync(@"/api/Products");
            var result = await TestUtiility.GetResultAsync<IEnumerable<ProductDto>>(response);
            Assert.AreEqual(productNumber, result.Count());
        }

        [Test]
        public async Task GetPaged()
        {
            int productNumber = 100;
            using var app = new ComplevoAssignmentWebAppFactory("InMemDbRead2", x => TestUtiility.SeedTestProducts(x, productNumber));
            using var client = app.CreateClientWithToken(_token);
            var take = 10;
            var skip = 15;
            var response = await client.GetAsync(@$"/api/Products?limit={take}&offset={skip}");

            var result =await TestUtiility.GetResultAsync<IEnumerable<ProductDto>>(response);
            Assert.AreEqual(take, result.Count());
        }

        [Test]
        public async Task GetNotExisted()
        {
            int productNumber = 10;
            using var app = new ComplevoAssignmentWebAppFactory("InMemDbRead3", x => TestUtiility.SeedTestProducts(x, productNumber));
            using var client = app.CreateClientWithToken(_token);
            var response = await client.GetAsync(@"/api/Products/20");
            Assert.AreEqual((int)response.StatusCode, StatusCodes.Status404NotFound);
        }

    }
}
