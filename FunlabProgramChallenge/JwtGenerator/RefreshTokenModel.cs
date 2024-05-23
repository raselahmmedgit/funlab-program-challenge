using System;

namespace FunlabProgramChallenge.JwtGenerator
{
    /// <summary>
    /// Jwt Refresh Token model.
    /// </summary>
    public class RefreshTokenModel
    {
        public string RefreshToken { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public DateTime AddedDate { get; set; }
    }
}
