using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Runtime.Caching;

namespace CachingService 
{ 
    public enum MyCachePriority 
    { 
        Default, 
        NotRemovable 
    }

    public class MyCache 
    { 
        // Gets a reference to the default MemoryCache instance. 
        private static ObjectCache cache = MemoryCache.Default; 
        private CacheItemPolicy policy = null; 
        private CacheEntryRemovedCallback callback = null;

        public void Set(String CacheKeyName, Object CacheItem) 
        { 
            // 
            callback = new CacheEntryRemovedCallback(this.MyCachedItemRemovedCallback); 
            policy = new CacheItemPolicy(); 
            //policy.Priority = (MyCacheItemPriority == MyCachePriority.Default) ? CacheItemPriority.Default : CacheItemPriority.NotRemovable; 
            policy.AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(10.00); 

            policy.RemovedCallback = callback; 
            //policy.ChangeMonitors.Add(new HostFileChangeMonitor(FilePath));

            // Add inside cache 
            cache.Set(CacheKeyName, CacheItem, policy); 
        }

        public Object Get(String CacheKeyName) 
        {  
            return cache[CacheKeyName] as Object; 
        }

        public void Clear(String CacheKeyName) 
        { 
            // 
            if (cache.Contains(CacheKeyName)) 
            { 
                cache.Remove(CacheKeyName); 
            } 
        }

        private void MyCachedItemRemovedCallback(CacheEntryRemovedArguments arguments) 
        { 
            // Log these values from arguments list 
            String strLog = String.Concat("Reason: ", arguments.RemovedReason.ToString(), " | Key-Name: ", arguments.CacheItem.Key, " | Value-Object: ", 
                arguments.CacheItem.Value.ToString()); 
            Console.WriteLine(strLog);
        }
    } 
}

