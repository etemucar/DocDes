using System;

namespace DocDes.Core.Security
{
    public class TokenUser
    {
        public string UserName { get; set; }= string.Empty;
        public int UserId { get; set; }
        public string Email { get; set; }= string.Empty;

        public TokenUser (string userName, int userId, string email)
        {
            UserName = userName;
            UserId = userId;
            Email = email;
        }
    }

    public class AccessToken
    {
        public TokenUser? User { get; set; }
        public string Token { get; set; }= string.Empty;
        public DateTime Expiration { get; set; }
    }
}