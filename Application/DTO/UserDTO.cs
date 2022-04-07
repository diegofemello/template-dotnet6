using Domain.Model;
using System;

namespace Application.DTO
{
    public class UserDTO
    {
        public Guid Uid { get; set; }

        public string FullName { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public bool EmailConfirmed { get; set; }

        public DateTime LastAccess { get; set; }

        public string UserRole { get; set; }

        public string RefreshToken { get; set; }

        public DateTime? RefreshTokenExpiryTime { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}