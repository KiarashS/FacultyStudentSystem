namespace ContentManagementSystem.DataLayer
{
    public static class PersianHelper
    {
        public static string ApplyUnifiedYeKe(this string data)
        {
            if (string.IsNullOrEmpty(data)) return data;
            return data.Replace("ي", "ی").Replace("ك", "ک");
        }
    }
}
