namespace Superstore.Helpers.Contants
{
	public static class StandardLookupFields
	{

        public static string[] Seasons = new string[] { "Winter", "Spring", "Summer", "Autumn" };
        public static string[] Regions = new string[] { "Central", "East", "South", "West" };
        public static string[] Categories = new string[] { "Furniture", "Technology", "Office Supplies" };

        public const string Region = "Region";
		public const string Season = "Season";
        public const string Category = "Category";

        public static List<string> GetAllStandardLookupFields()
		{
			return new List<string>()
			{
				Region,
				Season,
                Category
            };
		}
	}
}

