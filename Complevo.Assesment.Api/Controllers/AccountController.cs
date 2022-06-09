// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Complevo.Assesment.Api.Services;
using Complevo.Assesment.Services;
using Complevo.Assesment.Services.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Complevo.Assesment.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly TokenService _tokenService;
        private readonly AccountService _accountService;

        public AccountController(TokenService tokenService, AccountService accountService)
        {
            _tokenService = tokenService;
            _accountService = accountService;
        }


        /// <summary>
        /// Login and get bearer token
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        [HttpPost("Login")]
        [AllowAnonymous]        
        public ActionResult Login(LoginDto login)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            if (_accountService.LoginSuccessful(login.UserName, login.Password))
            {
                return new JsonResult(new { UserName = login.UserName, Token = _tokenService.CreateToken(login.UserName) });
            }
            return Unauthorized("Invalid credentials");
        }


    }
}
