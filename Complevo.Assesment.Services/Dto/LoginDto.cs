// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Complevo.Assesment.Services.Dto
{
    public class LoginDto
    {
        /// <summary>
        /// Account user name
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// Account password
        /// </summary>
        public string Password { get; set; }

    }
}
