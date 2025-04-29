using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Reposatory.Configuration
{
    public class OrderConfiguration : IEntityTypeConfiguration<Orders_Management>
    {
        public void Configure(EntityTypeBuilder<Orders_Management> builder)
        {
            builder.Property(x => x.OrderNumber).HasMaxLength(100);
            builder.Property(x => x.Price);
            builder.Property(x => x.Product);
            builder.Property(x => x.OrderDate);
            builder.Property(x => x.Status).HasMaxLength(50);
            builder.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(p=>p.UserId);
            builder.HasOne(x => x.Client)
                .WithMany()
                .HasForeignKey(p => p.ClientId);
        }
    

    }
}
