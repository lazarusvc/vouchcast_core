// -------------------------------------------------------------------
// File:    IdentityExtensions.cs
// Author:  N/A
// Created: N/A
// Purpose: Provides extension methods for ASP.NET Core Identity functionality.
// Dependencies:
//   - UserManager<VC_user> (Microsoft.AspNetCore.Identity)
//   - VC_IMS.Models.VC_user
// -------------------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using VC_IMS.Models;

namespace VC_IMS.Helpers
{
    /// <summary>
    /// Contains extension methods for <see cref="UserManager{VC_user}"/> related to LDAP authentication.
    /// </summary>
    public static class IdentityExtensions
    {
        /// <summary>
        /// Determines whether the specified <paramref name="user"/> has an external login via the LDAP provider.
        /// </summary>
        /// <param name="userManager">
        /// The <see cref="UserManager{VC_user}"/> instance used to retrieve user logins.
        /// </param>
        /// <param name="user">
        /// The <see cref="VC_user"/> instance whose logins are to be checked.
        /// </param>
        /// <returns>
        /// A <see cref="Task{TResult}"/> that resolves to <c>true</c> if the user has an LDAP login; otherwise, <c>false</c>.
        /// </returns>
        public static async Task<bool> IsLdapUserAsync(this UserManager<VC_user> userManager, VC_user user)
        {
            var logins = await userManager.GetLoginsAsync(user);
            return logins.Any(l => l.LoginProvider == "LDAP");
        }
    }
}
