
using EFWebApplicationWithAuthorization.Controllers;
using EFWebApplicationWithAuthorization.DTO_s;
using EFWebApplicationWithAuthorization.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EFWebApplicationWithAuthorization.Services
{
    public interface IDbService
    {
        IEnumerable GetDoctors();
        int PostDoctor(Doctor doctor);
        int PutDoctor(int IdDoctor, Doctor doctor);
        int DeleteDoctor(int IdDoctor);
        Tuple<ReturnedValues, IEnumerable> GetPrescription(int IdPrescription, GetPrescriptionResponse prescriptionResponse);
        bool MakeRegistration(LoginRequest loginRequest);
        JwtSecurityToken GenerateAccessToken();
        string GenerateRefreshToken();
        bool IsRefreshTokenCorrect(string refreshToken);
        bool CheckPasswordCorrectness(LoginRequest loginRequest);
        
    }
    public enum ReturnedValues
    {
        DOCTOR_NOT_EXIST, PRESCRIPTION_NOT_EXIST, PATIENT_NOT_EXIST,EVERYTHING_OK
    }
    public class DatabaseService : IDbService
    {
        private readonly MainDbContext _context;
        private IConfiguration _configuration;

        public DatabaseService(MainDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public IEnumerable GetDoctors()
        {
            return _context.Doctors.Select(e=>e);
        }

        public int PostDoctor(Doctor doctor)
        {
            _context.Doctors.Add(doctor);
            _context.SaveChanges();
            return 1;
        }

        public int PutDoctor(int IdDoctor,Doctor doctor)
        {
            if (IdDoctor != doctor.IdDoctor)
                return 0;

            if (!IfDoctorExistInDb(IdDoctor))
                return -1;

            _context.Doctors.Update(doctor);
            _context.SaveChanges();

            return 1;
        }
        public int DeleteDoctor(int IdDoctor)
        {

            if (_context.Doctors.Where(e => e.IdDoctor == IdDoctor).Any())
            {
                Doctor doctor = _context.Doctors.Where(e => e.IdDoctor == IdDoctor).First();
                _context.Doctors.Remove(doctor);
                _context.SaveChanges();
                return 1;
            }
            else
                return 0;
        }
        private bool IfDoctorExistInDb(int IdDoctor)
        {
            var result = _context.Doctors.Where(e => e.IdDoctor == IdDoctor).Any();

            if (!result)
                return false;

            return true;
        }

        public Tuple<ReturnedValues,IEnumerable> GetPrescription(int IdPrescription, GetPrescriptionResponse prescriptionResponse)
        {
            var doctor =GetDoctorByData(prescriptionResponse);
            if (doctor == null)
                return new(ReturnedValues.DOCTOR_NOT_EXIST,"Doctor with that personal data not exist.");
            var patient = GetPatientByData(prescriptionResponse);
            if (patient == null)
                return new(ReturnedValues.PATIENT_NOT_EXIST,"Patient with that personal data not exist.");
            var prescription = GetPrescriptionMedByData(IdPrescription,prescriptionResponse);
            if (prescription == null)
                return new(ReturnedValues.PRESCRIPTION_NOT_EXIST,"Prescription list does not exist.");

            var result = _context.Prescriptions
                .Where(e => e.IdDoctor == doctor.IdDoctor &&
                e.IdPatient == patient.IdPatient && e.IdPerscription == prescription.IdPerscription);
            
            return new(ReturnedValues.EVERYTHING_OK, result); ;
        }
        private Doctor GetDoctorByData(GetPrescriptionResponse prescriptionResponse)
        {
            var res = _context.Doctors
                .Where(e => e.FirstName == prescriptionResponse.DoctorFirstName &&
                e.LastName == prescriptionResponse.DoctorLastName);
            if (res.Any()==false)
                return null;

            return res.First();
        }
        private Patient GetPatientByData(GetPrescriptionResponse prescriptionResponse)
        {
            var res = _context.Patients
                .Where(e => e.FirstName == prescriptionResponse.PatientFirstName &&
                e.LastName == prescriptionResponse.PatientLastName);
            if (res.Any() == false)
                return null;

            return res.First();
        }
        private Prescription GetPrescriptionMedByData(int IdPrescription,GetPrescriptionResponse prescriptionResponse)
        {
            var res = _context.Prescriptions
                .Include(e => e.Prescription_Medicaments)
                .Where(e => e.IdPerscription == IdPrescription);
            if (res.Any() == false)
                return null;

            return res.First();
        }

        public bool MakeRegistration(LoginRequest loginRequest)
        {
            string pass = loginRequest.Password;

            byte[] salt = new byte[128 / 8];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: pass,
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA1,
                    iterationCount: 10000,
                    numBytesRequested: 256 / 8
                ));
            string saltBase64 = Convert.ToBase64String(salt);

            var user = new AppUser()
            {
                Login = loginRequest.Login,
                Password = hashed,
                Salt = saltBase64,
                RefreshToken = null,
                RefreshTokenExp = null
            };

            _context.AppUsers.Add(user);
            _context.SaveChanges();

            return true;
        }

        public JwtSecurityToken GenerateAccessToken()
        {
            

            Claim[] userClaims = new[]
            {
                new Claim(ClaimTypes.Name,"s20659"),
                new Claim(ClaimTypes.Name,"admin"),
                new Claim(ClaimTypes.Name,"student")
            };
            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SecretKey"]));
            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: "http://localhost:5001",
                audience: "http://localhost:5001",
                claims: userClaims,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: creds
                );

            return token;
        }
        AppUser user=null;
        public bool CheckPasswordCorrectness(LoginRequest loginRequest)
        {
            user = _context.AppUsers.Where(u => u.Login == loginRequest.Login).FirstOrDefault();
            if (user == null)
                return false;

            string storedSalt = user.Salt;
            string passHash = user.Password;
            var saltBytes = Convert.FromBase64String(storedSalt);

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: loginRequest.Password,
                    salt: saltBytes,
                    prf: KeyDerivationPrf.HMACSHA1,
                    iterationCount: 10000,
                    numBytesRequested: 256 / 8
                ));

            if (hashed != passHash)
                return false;
            return true;
        }

        public string GenerateRefreshToken()
        {
            if (user == null)
                return null;

            string GenerateRefreshToken()
            {
                var randomNumber = new byte[32];
                using (var rng = RandomNumberGenerator.Create())
                {
                    rng.GetBytes(randomNumber);
                    return Convert.ToBase64String(randomNumber);
                }
            }
            var refreshToken=GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExp = DateTime.Now.AddDays(1);
            _context.SaveChanges();

            return refreshToken;
        }

        public bool IsRefreshTokenCorrect(string refreshToken)
        {
            var IsCorrect=_context.AppUsers.Where(e => e.RefreshToken == refreshToken).FirstOrDefault();
            if (IsCorrect == null)
                return false;

            return true;
        }
    }
}
