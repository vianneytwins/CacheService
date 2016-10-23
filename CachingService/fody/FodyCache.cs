using System;
using log4net;
using System.Runtime.Caching;
using System.Collections.Generic;

namespace fody
{
    public class FodyCache : ICache
    {
        private ILog logger;
        private static ObjectCache cache = MemoryCache.Default;

        public FodyCache(ILog logger)
        {
            this.logger = logger;
        }

        public bool Contains(string key)
        {
            return cache.Contains(key);
        }

        public T Retrieve<T>(string key)
        {
            this.logger.Info(String.Format("[HIT] Retrieving Element {0} from Cache", key));
            return (T)cache[key];
        }

        public void Store(string key, object data)
        {
            if (data != null)
            {
                
                var policy = new CacheItemPolicy();

                var expiredAtMidNight = (DateTimeOffset)DateTime.Today.AddDays(1);
                policy.AbsoluteExpiration = expiredAtMidNight;

                var callback = new CacheEntryRemovedCallback(this.LogCacheExpirationCallBack); 
                policy.RemovedCallback = callback;

                this.logger.Info(String.Format("Storing Element {0} in Cache with expiring at {1}", key, expiredAtMidNight));
                cache.Set(key, data, policy);
            }
        }

        public void Remove(string key)
        {
            if (cache.Contains(key))
            {
                this.logger.Info(String.Format("Removing Element {0} in Cache", key));
                cache.Remove(key);
            }
        }

        /// <summary>
        /// Retrieves all Element in the cache for a given type. Probably super bad in terms of performance
        /// TODO VC : Find a better way to do it (cache region or TypedCached ?)
        /// </summary>
        /// <returns>The all.</returns>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public System.Collections.Generic.List<T> RetrieveAll<T>()
        {
            var returnItems = new List<T>();
            foreach (var item in cache)
            {
                if (item is T)
                {
                    returnItems.Add((T)item.Value);
                }
            }

            return returnItems;
        }

        private void LogCacheExpirationCallBack(CacheEntryRemovedArguments arguments) 
        { 
            String strLog = String.Concat("Clearing cache - Reason: ", arguments.RemovedReason.ToString(), ", Key-Name: ", arguments.CacheItem.Key); 
            this.logger.Info(strLog);
        }

    }
}

