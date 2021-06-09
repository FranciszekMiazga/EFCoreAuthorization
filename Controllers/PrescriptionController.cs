

using EFWebApplicationWithAuthorization.DTO_s;
using EFWebApplicationWithAuthorization.Models;
using EFWebApplicationWithAuthorization.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EFWebApplicationWithAuthorization.Controllers
{
    [Authorize]
    [Route("api/prescription")]
    [ApiController]
    public class PrescriptionController : ControllerBase
    {
        private readonly MainDbContext _context;
        private IDbService _dbservice;

        public PrescriptionController(MainDbContext context, IDbService dbservice)
        {
            _context = context;
            _dbservice = dbservice;
        }
        [HttpGet("{IdPrescription}")]
        public IActionResult GetPrecription(int IdPrescription,GetPrescriptionResponse prescriptionResponse)
        {
            var res=_dbservice.GetPrescription(IdPrescription, prescriptionResponse);
            if (res.Item1.Equals(ReturnedValues.DOCTOR_NOT_EXIST) ||
                res.Item1.Equals(ReturnedValues.PATIENT_NOT_EXIST)||
                res.Item1.Equals(ReturnedValues.PRESCRIPTION_NOT_EXIST))
            {
                return NotFound(res.Item2);
            }
            return Ok(res.Item2);
        }
    }
}
