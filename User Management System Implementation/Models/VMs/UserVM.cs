using System.ComponentModel.DataAnnotations;

namespace User_Management_System_Implementation.Models.VMs
{
    public class UserVM
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        [RegularExpression("^([a-zA-z\\s]{3,50})$")]
        public string UserName { get; set; }

        [Required]
        [StringLength(10, MinimumLength = 10)]
        [RegularExpression("^[789]\\d{9}$")]
        public string PhoneNumber { get; set; }

        [Required]
        [RegularExpression("^[a-z0-9+_.-]+@[a-z0-9.-]+$")]
        public string Email { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string Password { get; set; }

        public TrueFalse IsVerifiedEmail { get; set; }

        public TrueFalse IsVerifiedPhoneNumber { get; set; }

        public TrueFalse IsActiveUser { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public List<RoleAccess> UserAndRoles { get; set; }

    }
}
