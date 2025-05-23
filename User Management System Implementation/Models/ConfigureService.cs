﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace User_Management_System_Implementation.Models
{
    public class ConfigureService
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Key]
        public string UniqueId { get; set; }

        [Required]
        public string ItemUniqueId { get; set; }

        [Required]
        public string ItemName { get; set; }

        [Required]
        public string ItemValue { get; set; }

        [Required]
        public string ServiceId { get; set; }

        public Service Service { get; set; }

        [Required]
        public TrueFalse IsConfigured { get; set; }

    }
}
