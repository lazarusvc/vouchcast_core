using Microsoft.AspNetCore.Identity;
using VC_IMS.Models;

namespace VC_IMS.Services
{
    /// <summary>
    /// Accepts bcrypt ($2a/$2b/$2y) and legacy Identity PBKDF2 (AQAAAA...) hashes.
    /// PBKDF2 success returns SuccessRehashNeeded so Identity upgrades the hash to bcrypt.
    /// </summary>
    public sealed class CompatibleBcryptHasher : IPasswordHasher<VC_user>
    {
        // If you want to standardize on a specific cost (you recently used 12):
        private const int WorkFactor = 12;

        private readonly PasswordHasher<VC_user> _pbkdf2 = new(); // Identity default PBKDF2

        public string HashPassword(VC_user user, string password)
            => BCrypt.Net.BCrypt.HashPassword(password, workFactor: WorkFactor);

        public PasswordVerificationResult VerifyHashedPassword(
            VC_user user,
            string hashedPassword,
            string providedPassword)
        {
            if (string.IsNullOrEmpty(hashedPassword))
                return PasswordVerificationResult.Failed;

            // Common bcrypt prefixes
            if (hashedPassword.StartsWith("$2a$")
             || hashedPassword.StartsWith("$2b$")
             || hashedPassword.StartsWith("$2y$"))
            {
                return BCrypt.Net.BCrypt.Verify(providedPassword, hashedPassword)
                    ? PasswordVerificationResult.Success
                    : PasswordVerificationResult.Failed;
            }

            // Identity PBKDF2 (AQAAAA...) fallback
            if (hashedPassword.StartsWith("AQAAAA"))
            {
                var res = _pbkdf2.VerifyHashedPassword(user, hashedPassword, providedPassword);
                return res == PasswordVerificationResult.Success
                    ? PasswordVerificationResult.SuccessRehashNeeded
                    : res;
            }

            // Unknown format: last-resort PBKDF2 attempt
            var fallback = _pbkdf2.VerifyHashedPassword(user, hashedPassword, providedPassword);
            return fallback == PasswordVerificationResult.Success
                ? PasswordVerificationResult.SuccessRehashNeeded
                : PasswordVerificationResult.Failed;
        }
    }
}
