using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace User_Management_System_Implementation.Models
{
    public class MenuAccess
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Key]
        public string UniqueId { get; set; }

        [Required]
        public string RoleId { get; set; }

        public UserRole UserRole { get; set; }

        [Required]
        public string MenuId { get; set; }

        public Menu Menu { get; set; }

        [Required]
        public TrueFalse IsAccess { get; set; }
    }
}
