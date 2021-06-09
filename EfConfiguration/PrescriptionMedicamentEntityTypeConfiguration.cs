using System;
using EFWebApplicationWithAuthorization.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFWebApplicationWithAuthorization.EfConfiguration
{
    public class PrescriptionMedicamentEntityTypeConfiguration : IEntityTypeConfiguration<Prescription_Medicament>
    {
        public void Configure(EntityTypeBuilder<Prescription_Medicament> opt)
        {
            opt.HasKey(e => new { e.IdPrescription, e.IdMedicament });
            opt.Property(e => e.Dose);
            opt.HasOne(e => e.Medicament).WithMany(e => e.Prescription_Medicaments)
                .HasForeignKey(d => d.IdMedicament);
            opt.HasOne(e => e.Prescription).WithMany(e => e.Prescription_Medicaments)
                .HasForeignKey(d => d.IdPrescription);

            opt.HasData(
                new Prescription_Medicament {IdMedicament=1,IdPrescription=2,Dose=23,Details="avs" },
                new Prescription_Medicament { IdMedicament = 2, IdPrescription = 1, Dose = 2, Details = "add" },
                new Prescription_Medicament { IdMedicament = 5, IdPrescription = 1, Dose = 12, Details = "gd" },
                new Prescription_Medicament { IdMedicament = 3, IdPrescription = 3, Dose = 21, Details = "gdasprawdz" },
                new Prescription_Medicament { IdMedicament = 4, IdPrescription = 3, Dose = 1, Details = "gdasprawdz" }
                );
        }
    }
}
