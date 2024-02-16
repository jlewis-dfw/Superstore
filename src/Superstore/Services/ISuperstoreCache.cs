using Microsoft.Extensions.Caching.Memory;
using Superstore.Helpers.Contants;

namespace Superstore.Services
{
	public interface ISuperstoreCache
	{
		object GetCacheDataByKey(string key);
		T GetCahceDataByType<T>(string key) where T : class;
		void RemoveObjectFromCache(string key);
		void ResetAllInMemoryData();
		void AddObjectToCache(string key, object data, double absoluteExpiration = 15);
	}

	public class SuperstoreCache : ISuperstoreCache
	{
		private IMemoryCache _cache;

		public SuperstoreCache(IMemoryCache memoryCache)
		{
			_cache = memoryCache;
		}
		public object GetCacheDataByKey(string key)
		{
			try
			{
				return _cache.Get(key);
			}
			catch (Exception) { return null; }
		}

		public T GetCahceDataByType<T>(string key) where T : class
		{
			try
			{
				return (T)_cache.Get(key);
			}
			catch (Exception)
			{
				return null;
			}
		}

		public void RemoveObjectFromCache(string key)
		{
			_cache.Remove(key);
		}

		public void ResetAllInMemoryData()
		{
			foreach (var item in CacheKeys.GetAllCacheKeys())
			{
				RemoveObjectFromCache(item);
			}
		}

		public void AddObjectToCache(string key, object data, double absoluteExpiration = 15)
		{
			var cacheEntryOptions = new MemoryCacheEntryOptions()
				.SetAbsoluteExpiration(TimeSpan.FromMinutes(absoluteExpiration));
			_cache.Set(key, data, cacheEntryOptions);
		}


	}
}
