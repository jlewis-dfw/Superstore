namespace Superstore.Helpers.Contants
{
	/// <summary>
	/// Contants for ISuperstorecahce CacheKeys
	/// </summary>
	public class CacheKeys
    {

		public static readonly string Dashboard = "Dashboard";
		public static readonly string CsvData = "CsvData";
		public static readonly string StandardLookupDataSet = "StandardLookupDataSet";
		public static List<string> GetAllCacheKeys()
        {
            return new List<string>()
            {
                CsvData,
                Dashboard,
				StandardLookupDataSet

			};
        }
    }
}
