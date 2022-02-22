using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Model.Base
{
    public abstract class BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(60)]
        public string Name { get; set; }

        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
