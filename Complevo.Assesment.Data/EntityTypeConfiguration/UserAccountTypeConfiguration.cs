// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Complevo.Assesment.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Complevo.Assesment.Data.EntityTypeConfiguration
{
    internal class UserAccountTypeConfiguration : IEntityTypeConfiguration<UserAccount>
    {
        public void Configure(EntityTypeBuilder<UserAccount> builder)
        {
            builder.HasKey(x => x.Name);
            builder.Property(x=>x.Name).IsRequired();
            builder.Property(x => x.PasswordHash).IsRequired();
            builder.Property(x => x.Salt).IsRequired();

        }
    }
}
