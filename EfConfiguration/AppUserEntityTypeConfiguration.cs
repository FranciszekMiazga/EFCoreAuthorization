using EFWebApplicationWithAuthorization.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFWebApplicationWithAuthorization.EfConfiguration
{
    public class AppUserEntityTypeConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> opt)
        {
            opt.HasKey(e => e.IdUser);
            opt.Property(e => e.IdUser).ValueGeneratedOnAdd();
            opt.Property(e => e.Login).IsRequired();
            opt.Property(e => e.Password).IsRequired();
            opt.Property(e => e.Salt).IsRequired();
        }
    }
}
