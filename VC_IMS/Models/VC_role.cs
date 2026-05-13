// ==================== SwRole.cs ====================
// -------------------------------------------------------------------
// Author:  N/A
// Created: N/A
// Purpose: ASP.NET Core Identity role entity for VC_IMS.
// Dependencies:
//   - Microsoft.AspNetCore.Identity.IdentityRole<int>
// -------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;


namespace VC_IMS.Models;

/// <summary>
/// Represents a role within the VC_IMS application.
/// Inherits from <c>IdentityRole&lt;int&gt;</c> to include standard identity properties.
/// </summary>
public class VC_role : IdentityRole<int>
{
   
}
