namespace Superstore.Helpers.Contants
{
    public class CacheKeys
    {
        public static readonly string CsvData = "CsvData";
        public static readonly string SalesPeriods = "SalesPeriods";

        public static List<string> GetAllCacheKeys()
        {
            return new List<string>()
            {
                CsvData,
                SalesPeriods
            };
        }
    }
}
