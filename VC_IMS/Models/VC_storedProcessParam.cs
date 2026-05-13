namespace VC_IMS.Models
{
    public class VC_storedProcessParam
    {
        public int Id { get; set; }
        public int StoredProcessId { get; set; }
        public VC_storedProcess StoredProcess { get; set; } = null!;

        // Include the "@" (e.g., "@FromDate")
        public string Key { get; set; } = string.Empty;
        // Persist as string; convert to the right type during execution
        public string? Value { get; set; }
        // Supported: "Int","Float","Decimal","Bit","DateTime","NVarChar","UniqueIdentifier","Text"
        public string DataType { get; set; } = "NVarChar";
    }
}