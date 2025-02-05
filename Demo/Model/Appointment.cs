using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Demo.Model
{
    public class Appointment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AppointmentId { get; set; } // Auto-generated

        [Required]
        [MaxLength(100)]
        public string PatientName { get; set; } = string.Empty;

        [Required]
        [MaxLength(15)]
        public string PatientContact { get; set; } = string.Empty;

        [Required]
        public DateTime AppointmentDateTime { get; set; }

        [ForeignKey("Doctor")]
        public int DoctorId { get; set; }


        public Doctor Doctor { get; set; }
    }
}
