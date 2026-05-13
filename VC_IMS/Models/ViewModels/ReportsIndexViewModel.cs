namespace VC_IMS.Models.ViewModels
{
    public class ReportsIndexViewModel
    {
        public IEnumerable<VC_reports> Reports { get; set; } = System.Linq.Enumerable.Empty<VC_reports>();
        public int? SelectedId { get; set; }
        public string? ViewerUrl { get; set; }   // SSRS iframe URL or Inline action URL
    }
}
