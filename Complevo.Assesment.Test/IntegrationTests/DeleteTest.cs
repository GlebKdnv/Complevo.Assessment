// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using NUnit.Framework;

namespace Complevo.Assesment.Test.IntegrationTests
{
    internal class DeleteTest
    {
        [Test]
        public async Task SimpleDelete()
        {
            int productNumber = 1;
            using var app = new ComplevoAssignmentWebAppFactory("InMemDbDelete1", x => TestUtiility.SeedTestProducts(x, productNumber));
            using var client = app.CreateDefaultClient();
            var response = await client.DeleteAsync(@"/api/Products/1");
            response.EnsureSuccessStatusCode();
            Assert.AreEqual((int)response.StatusCode, StatusCodes.Status200OK);
        }

        [Test]
        public async Task DeleteNotExist()
        {
            int productNumber = 1;
            using var app = new ComplevoAssignmentWebAppFactory("InMemDbDelete2", x => TestUtiility.SeedTestProducts(x, productNumber));
            using var client = app.CreateDefaultClient();
            var response = await client.DeleteAsync(@"/api/Products/10");            
            Assert.AreEqual((int)response.StatusCode, StatusCodes.Status404NotFound);
        }


    }
}
