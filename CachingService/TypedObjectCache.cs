using System.Collections.Generic;

namespace CachingService
{
    using System;
    using System.Collections.Specialized;
    using System.Runtime.Caching;
    using log4net;

    public class TypedObjectCache<T> : MemoryCache where T : class
    {
        private CacheItemPolicy HardDefaultCacheItemPolicy = new CacheItemPolicy()
            {
                SlidingExpiration = new TimeSpan(0, 15, 0)
            };
        
        private ILog logger;

        private CacheItemPolicy defaultCacheItemPolicy;

        public TypedObjectCache(string name, ILog logger, NameValueCollection config = null, CacheItemPolicy policy = null) : base(name, config)
        {
            defaultCacheItemPolicy = policy??HardDefaultCacheItemPolicy;
            this.logger = logger;
        }

        public void Set(string cacheKey, T cacheItem)
        {
            this.logger.Debug(String.Format("Add Element {0} in cache {1}", cacheKey, this.Name));
            base.Set(cacheKey, cacheItem, defaultCacheItemPolicy);
        }

        public void Set(string cacheKey, Func<T> getData)
        {
            this.logger.Debug(String.Format("Add Element {0} in cache {1}", cacheKey, this.Name));
            this.Set(cacheKey, getData(), defaultCacheItemPolicy);
        }

        // TODO : use the lastupdate field to verify the timestamp of the cached element ?
        public bool TryGetAndSet(string cacheKey, Func<T> getData, out T returnData)
        {
            if(TryGet(cacheKey, out returnData))
            {
                return true;
            }
            returnData = getData();
            this.logger.Debug(String.Format("Add Element {0} in cache {1}", cacheKey, this.Name));
            this.Set(cacheKey, returnData, defaultCacheItemPolicy);
            return returnData != null;
        }

        public bool TryGet(string cacheKey, out T returnItem)
        {
            this.logger.Debug(String.Format("Try Get Element {0} in cache {1}", cacheKey, this.Name));
            returnItem = (T)this[cacheKey];
            return returnItem != null;
        }

        // TODO Faut il enregister tous les cache dans un manger pour les rendre visible et accessible via un controler ?
        // cela permettrait de checker le contenu, de faire des clear etc..
        public void Clear(string cacheKey)
        {
            this.Remove(cacheKey);
        }

        public List<T> GetAllCachedItems()
        {
            var returnItems = new List<T>();
            foreach (var item in this)
            {
                returnItems.Add((T)item.Value); 
            }

            return returnItems;
        }
    }
}