using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace User_Management_System_Implementation.Models
{
    public class RoleAccess
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Key]
        public string UniqueId { get; set; }

        [Required]
        public string UserId { get; set; }

        [JsonIgnore]
        public User User { get; set; }

        [Required]
        public string RoleId { get; set; }

        public UserRole UserRole { get; set; }

        [Required]
        public TrueFalse AccessToRole { get; set; }

    }
}
