using Complevo.Assesment.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Complevo.Assesment.Data.EntityTypeConfiguration
{
    internal class ProductTypeConfig : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property<string>(x=>x.Name).HasMaxLength(1024);
            builder.Property<string>(x=>x.Description).HasMaxLength(4000);

        }
    }
}
