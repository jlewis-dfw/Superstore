namespace Superstore.ViewModels.Charts
{
    public class RegionSalesByCategory
	{
		public string Region { get; set; } = default!;
		public decimal Total { get; set; }
		public decimal Furniture { get; set; }
		public decimal OfficeSupplies { get; set; }
		public decimal Technology { get; set; }
		public string Season { get; set; }
		public string SalesQuarter { get; set; }
		public string Category { get; set; }
        public decimal Percent { get; set; }
    }

}
