namespace Superstore.Helpers.Contants
{
    /// <summary>
    /// Contants for ISuperstorecahce
    /// </summary>
	public class CacheKeys
    {
        public static readonly string CsvData = "CsvData";
        public static readonly string SalesPeriods = "SalesPeriods";
        public static readonly string RegionSalesByCategoryByTimeCategoryViewModel = "RegionSalesByCategoryByTimeCategoryViewModel";

        public static List<string> GetAllCacheKeys()
        {
            return new List<string>()
            {
                CsvData,
                SalesPeriods,
                RegionSalesByCategoryByTimeCategoryViewModel
            };
        }
    }
}
