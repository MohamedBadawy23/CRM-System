using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Data.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
{
    public void Configure(EntityTypeBuilder<IdentityRole> builder)
    {
        builder.HasData(
            new IdentityRole
            {
                Id = "1793EA75-1EAE-4214-B3BE-7F0FE277F187",
                Name = "Visitor",
                NormalizedName = "VISITOR"
            },
            new IdentityRole
            {
                Id = "CE296659-E76F-4BA0-804D-45EEDEF18C25",
                Name = "Admin",
                NormalizedName = "ADMIN"
            },
            new IdentityRole
            {
                Id = "D4B5F0E1-2C3D-4E5F-6G7H-8I9J0K1L2M3N",
                Name = "Sales",
                NormalizedName = "SALES"
            });
    }
}
