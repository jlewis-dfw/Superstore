using Superstore.ViewModels.Charts;

namespace Superstore.ViewModels
{
	public class Dashboard
    {
        public StandardLookupFilters Filters { get; set; }
        public StandardLookupDataSet StandardLookupDataSet { get; set; }
		public PieChart PieChartSalesBySeason { get; set; }
        public PieChart PieChartSalesByRegion { get; set; }
        public PieChart PieChartSalesByCategory { get; set; }
        public LineChart LineChartSalesByCategoryRegionSeason { get; set; }
        public StoreSummary StoreSummary { get; set; }
        public bool IsFiltered { get; set; }
    }
}
