// -------------------------------------------------------------------
// File:    BcryptPasswordHasher.cs
// Author:  N/A
// Created: N/A
// Purpose: BCrypt-based implementation of IPasswordHasher<VC_user> for secure password hashing.
// Dependencies:
//   - Microsoft.AspNetCore.Identity.IPasswordHasher<VC_user>
//   - BCrypt.Net
//   - VC_IMS.Models.VC_user
// -------------------------------------------------------------------

using Microsoft.AspNetCore.Identity;
using VC_IMS.Models;

namespace VC_IMS.Services
{
    /// <summary>
    /// Implements <see cref="IPasswordHasher{VC_user}"/> using BCrypt for hashing and verifying passwords.
    /// </summary>
    public class BcryptPasswordHasher : IPasswordHasher<VC_user>
    {
        /// <summary>
        /// Creates a salted BCrypt hash of the specified <paramref name="password"/>.
        /// </summary>
        /// <param name="user">
        /// The <see cref="VC_user"/> instance (unused in this implementation).
        /// </param>
        /// <param name="password">
        /// The plaintext password to hash.
        /// </param>
        /// <returns>A salted hash of the password.</returns>
        public string HashPassword(VC_user user, string password) =>
            BCrypt.Net.BCrypt.HashPassword(password);

        /// <summary>
        /// Verifies that the provided plaintext password matches the stored hashed password.
        /// </summary>
        /// <param name="user">
        /// The <see cref="VC_user"/> instance (unused in this implementation).
        /// </param>
        /// <param name="hashedPassword">
        /// The stored BCrypt hash to verify against.
        /// </param>
        /// <param name="providedPassword">
        /// The plaintext password to verify.
        /// </param>
        /// <returns>
        /// <see cref="PasswordVerificationResult.Success"/> if the passwords match;
        /// otherwise, <see cref="PasswordVerificationResult.Failed"/>.
        /// </returns>
        public PasswordVerificationResult VerifyHashedPassword(
            VC_user user,
            string hashedPassword,
            string providedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(providedPassword, hashedPassword)
                ? PasswordVerificationResult.Success
                : PasswordVerificationResult.Failed;
        }
    }
}
