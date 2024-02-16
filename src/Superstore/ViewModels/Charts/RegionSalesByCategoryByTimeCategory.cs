using Superstore.Models;

namespace Superstore.ViewModels.Charts
{


	public class RegionSalesByCategoryByTimeCategoryViewModel
	{
        public List<RegionSalesByCategoryByTimeCategory> RegionSalesByCategoryByTimeCategories { get; set; } = new();
        public List<SeriesData> SeriesCollection { get; set; } = new();

    }

    public class RegionSalesByCategoryByTimeCategory
	{
		public string Region { get; set; } = default!;
		public string Category { get; set; } = default!;
		public double Total { get; set; }
		public string TimeCategory { get; set; }
		public Season Season { get; set; }
	}

}
