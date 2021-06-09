using EFWebApplicationWithAuthorization.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace EFWebApplicationWithAuthorization.EfConfiguration
{
    public class MedicamentEntityTypeConfiguration : IEntityTypeConfiguration<Medicament>
    {
        public void Configure(EntityTypeBuilder<Medicament> opt)
        {
            opt.HasKey(e => e.IdMedicament);
            opt.Property(e => e.IdMedicament).ValueGeneratedOnAdd();
            opt.Property(e=>e.Name).IsRequired().HasMaxLength(100);
            opt.Property(e => e.Description).IsRequired().HasMaxLength(100);
            opt.Property(e => e.Type).IsRequired().HasMaxLength(100);

            opt.HasMany(s => s.Prescription_Medicaments)
                .WithOne(d => d.Medicament)
                .HasForeignKey(d => d.IdMedicament);
            opt.HasData(
                new Medicament { IdMedicament = 1, Name = "Pfizer", Description = "abs", Type = "typeA" },
                new Medicament { IdMedicament = 2, Name = "Astrazeneca", Description = "rda", Type = "typeB" },
                new Medicament { IdMedicament = 3, Name = "Paracetamol", Description = "bda", Type = "typeC" },
                new Medicament { IdMedicament = 4, Name = "Rutinoscorbin", Description = "www", Type = "typeD" },
                new Medicament { IdMedicament = 5, Name = "Aspirin", Description = "hgd", Type = "typeE" }
                );
        }
    }
}
