
using EFWebApplicationWithAuthorization.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFWebApplicationWithAuthorization.DTO_s
{
    public class GetPrescriptionResponse
    {
        public string DoctorFirstName { get; set; }
        public string DoctorLastName { get; set; }
        public string PatientFirstName { get; set; }
        public string PatientLastName { get; set; }
        public int IdPrescription { get; set; }
        public virtual ICollection<Prescription_Medicament> Prescription_Medicaments { get; set; }

    }
}
