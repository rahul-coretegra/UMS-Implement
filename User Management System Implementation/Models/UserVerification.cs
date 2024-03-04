using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace User_Management_System_Implementation.Models
{
    public class UserVerification
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Key]
        public string Identity { get; set; }

        public string Otp { get; set; }

        public DateTime? OtpTimeStamp { get; set; }
    }
}
