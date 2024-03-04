using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace User_Management_System_Implementation.Models
{
    
    public class Service
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Key]
        public string ServiceId { get; set; }

        [Required]
        public string ServiceUniqueId { get; set; }

        [Required]
        public string ServiceName { get; set; }

        [Required]
        public TypeOfService ServiceType { get; set; }

        [Required]
        public TrueFalse Status { get; set; }

    }
}
