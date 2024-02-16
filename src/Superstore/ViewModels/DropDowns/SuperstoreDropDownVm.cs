using Superstore.Models;

namespace Superstore.ViewModels.DropDowns
{
    public class SuperstoreDropDownVm
    {
        public IEnumerable<StandardLookup> StandardLookups { get; set; }
        public string[] BindVales { get; set; }
        public string Width { get; set; } = "500px";
	}
}
