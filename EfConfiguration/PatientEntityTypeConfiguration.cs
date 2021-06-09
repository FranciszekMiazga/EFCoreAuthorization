using EFWebApplicationWithAuthorization.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFWebApplicationWithAuthorization.EfConfiguration
{
    public class PatientEntityTypeConfiguration: IEntityTypeConfiguration<Patient>
    {
        public void Configure(EntityTypeBuilder<Patient> opt)
        {
            opt.HasKey(e => e.IdPatient);
            opt.Property(e => e.IdPatient).ValueGeneratedOnAdd();
            opt.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
            opt.Property(e => e.LastName).IsRequired().HasMaxLength(100);
            opt.Property(e => e.BirthDate).IsRequired();

            opt.HasMany(s => s.Prescriptions)
                .WithOne(d => d.Patient)
                .HasForeignKey(d => d.IdPatient);
            opt.HasData(
                new Patient { IdPatient = 1, FirstName = "Jan", LastName = "Kowalski", BirthDate = DateTime.Parse("23-12-2000") },
                new Patient { IdPatient = 2, FirstName = "Mateusz", LastName = "Nowak", BirthDate = DateTime.Parse("12-01-1998") },
                new Patient { IdPatient = 3, FirstName = "Grzegorz", LastName = "Modzelewski", BirthDate = DateTime.Parse("11-02-1981") },
                new Patient { IdPatient = 4, FirstName = "Adam", LastName = "Piotrkowski", BirthDate = DateTime.Parse("07-06-2002") },
                new Patient { IdPatient = 5, FirstName = "Natalia", LastName = "Drzazga", BirthDate = DateTime.Parse("12-08-2012") }
                );
        }
    }
}
