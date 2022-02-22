using Domain.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Domain.VO
{
    public class UserVO
    {
        public Guid Uid { get; set; }

        public string UserName { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public bool EmailConfirmed { get; set; }

        public DateTime LastAccess { get; set; }

        public string Role { get; set; }

        public string RefreshToken { get; set; }

        public DateTime? RefreshTokenExpiryTime { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

    }
}