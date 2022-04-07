using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Model
{
    [Table("Users")]
    public class User
    {
        [Key]
        public Guid Uid { get; set; }

        [Required]
        [MaxLength(20)]
        public string UserName { get; set; }

        [Required]
        [MaxLength(130)]
        public string FullName { get; set; }

        [Required]
        [MaxLength(50)]
        public string Email { get; set; }

        [MaxLength(50)]
        public string ChangedEmail { get; set; }

        public bool EmailConfirmed { get; set; }

        [Required]
        [MaxLength(255)]
        public string Password { get; set; }

        public DateTime? LastAccess { get; set; }

        [MaxLength(255)]
        public string EmailToken { get; set; }

        [MaxLength(255)]
        public string RefreshToken { get; set; }

        public DateTime? RefreshTokenExpiryTime { get; set; }

        public UserRole UserRole { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public enum UserRole
    {
        Visitor = 0,
        Default = 1,
        Admin = 2
    }
}