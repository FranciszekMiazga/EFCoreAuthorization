
using EFWebApplicationWithAuthorization.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFWebApplicationWithAuthorization.EfConfiguration
{
    public class DoctorEntityTypeConfiguration : IEntityTypeConfiguration<Doctor>
    {
        public void Configure(EntityTypeBuilder<Doctor> opt)
        {
            opt.HasKey(e => e.IdDoctor);
            opt.Property(e => e.IdDoctor).ValueGeneratedOnAdd();
            opt.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
            opt.Property(e => e.LastName).IsRequired().HasMaxLength(100);
            opt.Property(e => e.Email).IsRequired().HasMaxLength(100);

            opt.HasMany(s => s.Prescriptions)
                .WithOne(d => d.Doctor)
                .HasForeignKey(d=>d.IdDoctor);

            opt.HasData(
                new Doctor {IdDoctor=1, FirstName="Jan",LastName="Kowalski",Email="kowalski@wp.pl"},
                new Doctor { IdDoctor = 2, FirstName = "Mateusz", LastName = "Nowak", Email = "nowak@wp.pl" },
                new Doctor { IdDoctor = 3, FirstName = "Grzegorz", LastName = "Modzelewski", Email = "modzel@gmail.com" },
                new Doctor { IdDoctor = 4, FirstName = "Adam", LastName = "Piotrkowski", Email = "piter@wp.pl" },
                new Doctor { IdDoctor = 5, FirstName = "Natalia", LastName = "Drzazga", Email = "drzazga@wp.pl" }
                );
        }
    }
}
