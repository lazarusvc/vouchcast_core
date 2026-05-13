using System.Collections.Generic;
using VC_IMS.Services.SystemSettings;

namespace VC_IMS.Areas.Admin.ViewModels.SystemSettings
{
    public class SystemSettingsIndexViewModel
    {
        public string? ActiveEnvironment { get; set; }
        public IList<SystemSettingsSectionSummary> Sections { get; set; } = new List<SystemSettingsSectionSummary>();
    }
}
