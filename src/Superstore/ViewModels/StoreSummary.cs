namespace Superstore.ViewModels
{
    public class StoreSummary
    {
        public int UniqueCustomers { get; set; }
        public int TotalItemsShipped { get; set; }
		public int TotalOrders { get; set; }
		public string TotalSales { get; set; }
        public string TotalProfit { get; set; }
        public string FirstOrderDate { get; set; }
        public string LastOrderDate { get; set; }
    }
}
