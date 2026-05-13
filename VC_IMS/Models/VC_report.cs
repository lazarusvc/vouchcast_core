using VC_IMS.Models.Logging;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace VC_IMS.Models
{
    public class VC_reports : IAudited
    {
        public int Id { get; set; }

        [Required, MaxLength(256)]
        [Display(Name = "Report Name (.rdl or alias)")]
        public string Name { get; set; } = default!;

        [MaxLength(512)]
        [Display(Name = "Description")]
        public string? Desc { get; set; }

        [MaxLength(256)]
        [Display(Name = "Path Override (optional)")]
        public string? PathOverride { get; set; }

        [Required(ErrorMessage = "A role is required to gate who can see this report.")]
        [Display(Name = "Visible To Role")]
        public string RoleId { get; set; } = default!;

        [Display(Name = "Apply stored parameters")]
        public bool ParamCheck { get; set; }

        public ICollection<VC_reportParam> Params { get; set; } = new List<VC_reportParam>();

    }
}