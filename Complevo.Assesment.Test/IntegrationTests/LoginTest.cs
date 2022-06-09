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
    internal class LoginTest
    {
        [Test]
        public async Task LoginSuccesful()
        {
            using var app = new ComplevoAssignmentWebAppFactory("LoginDb", null);
            using var client = app.CreateDefaultClient();
            var userLogin = new LoginDto() { UserName = "User1", Password = "password1" };
            var content = new StringContent(JsonSerializer.Serialize(userLogin), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/api/Account/Login", content);
            response.EnsureSuccessStatusCode(); 
        }

        [Test]
        public async Task LoginUnSuccesful()
        {
            using var app = new ComplevoAssignmentWebAppFactory("LoginDb", null);
            using var client = app.CreateDefaultClient();
            var userLogin = new LoginDto() { UserName = "User1123", Password = "password112312" };
            var content = new StringContent(JsonSerializer.Serialize(userLogin), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/api/Account/Login", content);
            Assert.AreEqual(StatusCodes.Status401Unauthorized,(int)response.StatusCode);
        }

    }
}
