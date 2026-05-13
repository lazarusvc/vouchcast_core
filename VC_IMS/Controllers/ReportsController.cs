using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using VC_IMS.Models;
using VC_IMS.Models.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace VC_IMS.Controllers
{
   [Authorize(Policy = "ReportsView")]
    public class ReportsController : Controller
    {
        public string url = "";
        private readonly VC_IMSDb_moreContext _db;
        private readonly IConfiguration _configuration;

        public ReportsController(VC_IMSDb_moreContext db, IConfiguration configuration)
        { 
            _db = db;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var roleIds = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();
            var reports = await _db.VC_reports.Where(r => roleIds.Contains(r.RoleId))
                                             .OrderBy(r => r.Desc ?? r.Name)
                                             .ToListAsync();
            return View(new ReportsIndexViewModel { Reports = reports });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Run(int id)
        {
            var roleIds = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();
            var reports = await _db.VC_reports.Where(r => roleIds.Contains(r.RoleId))
                                             .OrderBy(r => r.Desc ?? r.Name)
                                             .ToListAsync();

            var rpt = await _db.VC_reports.Include(r => r.Params).FirstOrDefaultAsync(r => r.Id == id);
            if (rpt == null) return NotFound();

            var parms = rpt.ParamCheck
                ? rpt.Params.Select(p => new KeyValuePair<string, string>(p.ParamKey.Trim(), p.ParamValue.Trim()))
                : System.Linq.Enumerable.Empty<KeyValuePair<string, string>>();

            // Report server selector
            string? serv = _configuration.GetValue<string>("Reporting:ReportServer");             

            if (!String.IsNullOrWhiteSpace(serv) && serv == "SSRS")
            {
                string? serv_url = _configuration.GetValue<string>("Reporting:_ssrs__ReportServerUrl");
                string? urlfront = _configuration.GetValue<string>("Reporting:_ssrs__ReportUrlFront");
                string? urlend= _configuration.GetValue<string>("Reporting:_ssrs__ReportUrlEnd");
                url = serv_url + urlfront + rpt.Name.Replace(".rdl", "") + urlend;    
            }

            // Export to DOM
            var vm = new ReportsIndexViewModel
            {
                Reports = reports,
                SelectedId = id,
                ViewerUrl = url
            };
            return View("Index", vm); // <— RETURN INDEX WITH IFRAME BELOW
        }
    }
}
