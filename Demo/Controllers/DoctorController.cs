using Demo.Data;
using Demo.DTO;
using Demo.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        public DoctorController(ApplicationDbContext dbContext)
        {
            _dbContext= dbContext;
        }
        [HttpPost("doctor")]
        public IActionResult Post([FromBody] DoctorDTO doctorDTO)
        {
            var doctor = new Doctor
            {
              DoctorName= doctorDTO.DoctorName,
            };
            _dbContext.Doctors.Add(doctor);
            _dbContext.SaveChanges();
            return Ok(new {Message="Saved Succesfully",Data= doctor});

        }
        
    }
}
