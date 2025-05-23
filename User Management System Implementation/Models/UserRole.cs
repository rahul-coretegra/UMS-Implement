﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace User_Management_System_Implementation.Models
{
    public class UserRole
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Key]
        public string RoleId { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        [RegularExpression("^(?=.*[a-zA-Z])[a-zA-Z ]{3,50}$")]
        public string RoleName { get; set; }

        [Required]
        public RoleLevels RoleLevel { get; set; }

        [Required]
        public TrueFalse Status { get; set; }
    }
}
