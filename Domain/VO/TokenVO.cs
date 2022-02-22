using Newtonsoft.Json;
using System;

namespace Domain.VO
{
    public class TokenVO
    {
        public TokenVO(bool authenticated, string created, string expiration, string accessToken, string refreshToken, UserVO user)
        {
            Authenticated = authenticated;
            Created = created;
            Expiration = expiration;
            AccessToken = accessToken;
            RefreshToken = refreshToken;
            User = user;
        }

        public TokenVO() { }

        public bool Authenticated { get; set; }

        public string Created { get; set; }

        public string Expiration { get; set; }

        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }

        public UserVO User { get; set; }

    }
}