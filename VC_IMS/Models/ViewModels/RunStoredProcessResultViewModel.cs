using System.Data;

namespace VC_IMS.Models.ViewModels;

public class RunStoredProcessResultViewModel
{
    public int ProcessId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Error { get; set; }
    public DataTable? Table { get; set; }
}
