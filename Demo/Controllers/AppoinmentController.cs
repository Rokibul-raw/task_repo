using Demo.Data;
using Demo.DTO;
using Demo.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Demo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppoinmentController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public AppoinmentController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost("appointments")]
        [Authorize]
        public IActionResult Post(AppoinmentDTO appoinmentDTO)
        {
            var apoinment = new Appointment
            {
              PatientName= appoinmentDTO.PatientName,
              PatientContact=appoinmentDTO.PatientContactInformation,
              AppointmentDateTime=appoinmentDTO.Date,
              DoctorId=appoinmentDTO.DoctorID,
            };
            _dbContext.Appointments.Add(apoinment);
            _dbContext.SaveChanges();
            return Ok(new {Message="Appointment Saved Successfully",Data= apoinment });
        }

        [HttpGet("appointments")]
        [Authorize]
        public async Task<IActionResult> GetUsersAsync()
        {
            var result = await _dbContext.Appointments
                                .Select(a => new
                                {
                                    a.AppointmentId,
                                    a.PatientName,
                                    a.PatientContact,
                                    a.AppointmentDateTime,
                                    a.DoctorId
                                })
                                .ToListAsync();

            return Ok(new { Message = "Success", Data = result });
        }

        [HttpGet("appointments/{id}")]
        [Authorize]
        public async Task<IActionResult> GetUserByIdAsync(int id)
        {
            var result = await _dbContext.Appointments
                                  .Where(u => u.AppointmentId == id)
                                  .Select(a => new
                                  {
                                      a.AppointmentId,
                                      a.PatientName,
                                      a.PatientContact,
                                      a.AppointmentDateTime,
                                      a.DoctorId
                                  })
                                  .FirstOrDefaultAsync();
            return Ok(new { Message = "Success", Data = result });
        }

        [HttpPut("appointments/{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateUserAsync(int id, [FromBody] AppoinmentDTO appoinmentDTO)
        {
            var existingUser = await _dbContext.Appointments.FirstOrDefaultAsync(u => u.AppointmentId == id);

            if (existingUser == null)
            {
                return NotFound(new { Message = "User not found" });
            }
            existingUser.PatientName = appoinmentDTO.PatientName;
            existingUser.PatientContact = appoinmentDTO.PatientContactInformation;
            existingUser.AppointmentDateTime = appoinmentDTO.Date;
            existingUser.DoctorId = appoinmentDTO.DoctorID;

            _dbContext.Appointments.Update(existingUser);
            await _dbContext.SaveChangesAsync();

            return Ok(new { Message = "User updated successfully" });
        }

        [HttpDelete("appointments/{id}")]
        public async Task<IActionResult> DeleteAppointmentAsync(int id)
        {
            var appointment = await _dbContext.Appointments
                                              .FirstOrDefaultAsync(a => a.AppointmentId == id);

            if (appointment == null)
            {
                return NotFound(new { Message = "Appointment not found" });
            }

            _dbContext.Appointments.Remove(appointment);
            await _dbContext.SaveChangesAsync();

            return Ok(new { Message = "Appointment deleted successfully" });
        }
    }
}
