namespace Superstore.Extensions
{
	public static class IEnumerableExtenstions
	{
		public static IEnumerable<T> SetValue<T>(this IEnumerable<T> items, Action<T>
			 updateMethod)
		{
			foreach (T item in items)
			{
				updateMethod(item);
			}
			return items;
		}
	}

}


