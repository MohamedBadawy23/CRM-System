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
    public class ClientConfiguration : IEntityTypeConfiguration<Clients_Management>
    {
        public void Configure(EntityTypeBuilder<Clients_Management> builder)
        {
            builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Phone);
            builder.Property(x => x.Email).HasMaxLength(100);
            builder.Property(x => x.Address).HasMaxLength(200);
            builder.Property(x => x.Date);
            builder.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(p=>p.UserId);
               
        }

        
    }
    
}
