using EFWebApplicationWithAuthorization.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace EFWebApplicationWithAuthorization.EfConfiguration
{
    public class PrescriptionEntityTypeConfiguration : IEntityTypeConfiguration<Prescription>
    {
        public void Configure(EntityTypeBuilder<Prescription> opt)
        {
            opt.HasKey(e => e.IdPerscription);
            opt.Property(e => e.IdPerscription).ValueGeneratedOnAdd();
            opt.Property(e => e.Date).IsRequired();
            opt.Property(e => e.DueDate).IsRequired();
            opt.Property(e => e.IdPatient).IsRequired().HasMaxLength(100);

            opt.HasMany(s => s.Prescription_Medicaments)
                .WithOne(d => d.Prescription)
                .HasForeignKey(d => d.IdPrescription);

            opt.HasOne(s => s.Doctor)
                .WithMany(d => d.Prescriptions)
                .HasForeignKey(d => d.IdDoctor);

            opt.HasOne(s => s.Patient)
                .WithMany(d => d.Prescriptions)
                .HasForeignKey(d => d.IdPatient);
            opt.HasData(
                new Prescription { IdPerscription=1, Date=DateTime.Parse("12-12-2000"),DueDate=DateTime.Parse("06-10-1990"),IdPatient=1,IdDoctor=2},
                new Prescription { IdPerscription = 2, Date = DateTime.Parse("12-12-2000"), DueDate = DateTime.Parse("06-10-1980"), IdPatient = 3, IdDoctor = 4 },
                new Prescription { IdPerscription = 3, Date = DateTime.Parse("12-12-2000"), DueDate = DateTime.Parse("06-10-1970"), IdPatient = 3, IdDoctor = 3 }
                );

        }
    }
}
