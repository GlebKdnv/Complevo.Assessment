// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Complevo.Assesment.Data.Entities;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Complevo.Assesment.Data.Tools
{
    public static class UserAccountCryptoUtilities
    {
        /// <summary>
        /// Creats UserAccount Entity generating cryptographic salt and password hash
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static UserAccount CreateUserAccount(string userName, string password)
        {
            var saltBytes = new byte[128 / 8];
            //random salt
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                rngCsp.GetNonZeroBytes(saltBytes);
            };
            var salt = Convert.ToBase64String(saltBytes);
            string hashed = GetPasswHash(password, saltBytes);
            return new UserAccount() { Name = userName, PasswordHash = hashed, Salt = salt };
        }


        /// <summary>
        /// Generates passwords hash
        /// </summary>
        /// <param name="password"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        public static string GetPasswHash(string password, byte[] salt)
        {
            var hash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 1000,
            numBytesRequested: 256 / 8));
            return hash;
        }
    }
}
