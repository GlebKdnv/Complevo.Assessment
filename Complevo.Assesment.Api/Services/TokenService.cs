// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace Complevo.Assesment.Api.Services
{
    /// <summary>
    /// Simples Jwt Token service with SymmetricSecurityKey
    /// </summary>
    public class TokenService 
    {
        private readonly SymmetricSecurityKey _key;

        public TokenService(SymmetricSecurityKey key)
        {
            _key = key;
        }

        public string CreateToken(string username)
        {
            var claims = new List<Claim> { new Claim(JwtRegisteredClaimNames.NameId, username) };
            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);
            
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds,                
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);
            
            return tokenHandler.WriteToken(token);

        }
  
    }
}
