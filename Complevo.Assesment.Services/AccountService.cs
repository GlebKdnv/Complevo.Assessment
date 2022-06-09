// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Text;
using System.Threading.Tasks;
using Complevo.Assesment.Data;
using Complevo.Assesment.Data.Entities;
using Complevo.Assesment.Data.Tools;

namespace Complevo.Assesment.Services
{
    public class AccountService
    {
        private readonly ApplicationContext _applicationContext;

        public AccountService(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }

        public bool LoginSuccessful(string userName, string password)
        {
            var user = _applicationContext.UserAccounts.FirstOrDefault(x => userName.ToLower() == x.Name.ToLower());
            if (user == null)
            {
                return false;
            }
            var computedHash = UserAccountCryptoUtilities.GetPasswHash(password, Convert.FromBase64String(user.Salt));
            return computedHash == user.PasswordHash;

        }
    }
}

