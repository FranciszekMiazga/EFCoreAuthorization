

using EFWebApplicationWithAuthorization.Models;
using EFWebApplicationWithAuthorization.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EFWebApplicationWithAuthorization.Controllers
{
    [Authorize]
    [Route("api/doctor")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly MainDbContext _context;
        private IDbService _dbservice;

        public DoctorController(MainDbContext context, IDbService dbservice)
        {
            _context = context;
            _dbservice = dbservice;
           
        }

        [HttpGet]
        public IActionResult GetDoctors()
        {
            return Ok(_dbservice.GetDoctors());
        }
        [HttpPost]
        public IActionResult PostDoctor(Doctor doctor)
        {
            _dbservice.PostDoctor(doctor);
            return Ok("Doctor is added succesfully.");
        }
        [HttpPut("{IdDoctor}")]
        public IActionResult PutDoctor(int IdDoctor,Doctor doctor)
        {
            var result=_dbservice.PutDoctor(IdDoctor,doctor);
            if (result == 0)
                return BadRequest("IdDoctor from parameters is not the same as in body.");
            else if (result == -1)
                return NotFound("There is no doctor with matching IdDoctor in database.");
            return Ok("Doctor is updated succesfully.");
        }
        [HttpDelete("{IdDoctor}")]
        public IActionResult DeleteDoctor(int IdDoctor)
        {
            int result=_dbservice.DeleteDoctor(IdDoctor);
            if (result == 0)
                return NotFound("Not found doctor with this IdDoctor.");
            return Ok("Deleted the doctor with Id "+IdDoctor);
        }
    }

}
