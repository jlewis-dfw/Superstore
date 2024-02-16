namespace Superstore.Models
{
	public class Season
	{
        public Season()
        {
            
        }
        public Season(int id, string name)
		{
			Id = id;
			Name = name;
		}

		public int Id { get; set; }
		public string Name { get; set; }


		public static List<Season> GetSeasons() 
		{ 
			return new List<Season>() 
			{  
				new Season(1, "Winter"),
				new Season(2, "Spring"),
				new Season(3, "Summer"),
				new Season(4, "Autumn"),
			}; 
		}



	}
}
