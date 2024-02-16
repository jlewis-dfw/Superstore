using Superstore.Models;

namespace Superstore.ViewModels
{
	/// <summary>
	/// Contains IEnumerable of all Seasons, Regions, and Categories
	/// </summary>
	public class StandardLookupDataSet
	{
		public IEnumerable<StandardLookup> Seasons { get; set; } = new List<StandardLookup>();
		public IEnumerable<StandardLookup> Regions { get; set; } = new List<StandardLookup>();
		public IEnumerable<StandardLookup> Categories { get; set; } = new List<StandardLookup>();
	}

}
