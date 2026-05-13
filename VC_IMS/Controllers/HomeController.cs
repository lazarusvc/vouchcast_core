// -------------------------------------------------------------------
// File:    HomeController.cs
// Author:  N/A
// Created: N/A
// Purpose: Controller for public-facing pages including home, privacy, and error.
// Dependencies:
//   - Microsoft.AspNetCore.Mvc.Controller
//   - VC_IMS.Models.ErrorViewModel
//   - System.Diagnostics.Activity
// -------------------------------------------------------------------

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VC_IMS.Models;
using VC_IMS.Models.ViewModels;
using System.Diagnostics;

namespace VC_IMS.Controllers
{
    /// <summary>
    /// Provides actions for the home page, privacy policy, and error display.
    /// </summary>
    /// <remarks>
    /// Creates a new <see cref="HomeController"/> with the specified logger.
    /// </remarks>
    /// <param name="context"></param>
    /// <param name="logger">
    /// The <see cref="ILogger{HomeController}"/> used for logging.
    /// </param>
    public class HomeController(VC_IMSDb_moreContext context, ILogger<HomeController> logger) : Controller
    {
        private readonly VC_IMSDb_moreContext _context = context;
        private readonly ILogger<HomeController> _logger = logger;

        // App Identity details
        // ==================
        public static String global_Identity(VC_IMSDb_moreContext _cx, int? id)
        {
            string results = "";
            try
            {
                switch (id)
                {
                    case 1: // ---------------------------------------------------------------- arg:1 - Name
                        results = _cx.VC_identities.Select(x => x.name).FirstOrDefault();
                        break;
                    case 2: // ---------------------------------------------------------------- arg2: - Desc
                        results = _cx.VC_identities.Select(x => x.desc).FirstOrDefault();
                        break;
                    case 3: // ---------------------------------------------------------------- arg3: - Logo
                        results = _cx.VC_identities.Select(x => x.logo).FirstOrDefault();
                        break;
                    case 4: // ---------------------------------------------------------------- arg4: - Media 1
                        results = _cx.VC_identities.Select(x => x.media_01).FirstOrDefault();
                        break;
                    case 5: // ---------------------------------------------------------------- arg5: - Media 2
                        results = _cx.VC_identities.Select(x => x.media_02).FirstOrDefault();
                        break;
                    case 6: // ---------------------------------------------------------------- arg6: - Media 3
                        results = _cx.VC_identities.Select(x => x.media_03).FirstOrDefault();
                        break;
                }
            }
            catch (Exception e)
            {
                throw new Exception("Error:" + e);
                results = "Error:" + e;
            }

            return results;
        }

        /// <summary>
        /// Displays the application home page.
        /// </summary>
        /// <returns>A <see cref="ViewResult"/> for the Index view.</returns>
        public IActionResult Index()
        {
            ViewBag.title = _context.VC_identities.Select(x => x.name).FirstOrDefault();
            ViewBag.frmBtn = _context.VC_forms.AsNoTracking().ToList();
            return View();
        }

        /// <summary>
        /// Displays the privacy policy page.
        /// </summary>
        /// <returns>A <see cref="ViewResult"/> for the Privacy view.</returns>
        public IActionResult Privacy()
        {
            return View();
        }

        /// <summary>
        /// Displays the error page with diagnostic information.
        /// </summary>
        /// <returns>
        /// A <see cref="ViewResult"/> for the Error view, populated with an
        /// <see cref="ErrorViewModel"/> containing the current request ID.
        /// </returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
