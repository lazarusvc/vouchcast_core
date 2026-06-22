namespace VC_IMS.Helpers
{
    public static class CategoryHelper
    {
        public static string GetCategoryBadgeClass(string category)
        {
            var colors = new[] { "badge-blue", "badge-purple", "badge-green",
                         "badge-amber", "badge-teal", "badge-coral" };
            var index = Math.Abs((category ?? "").GetHashCode()) % colors.Length;
            return colors[index];
        }
    }
}
