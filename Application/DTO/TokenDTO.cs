using System;

namespace Application.DTO
{
    public class TokenDTO
    {
        public bool Authenticated { get; set; }

        public DateTime Created { get; set; }

        public DateTime Expiration { get; set; }

        public string AccessToken { get; set; }

        public UserDTO User { get; set; }

    }
}