using System;
using System.Runtime.Caching;
// from http://codereview.stackexchange.com/questions/48148/generic-thread-safe-memorycache-manager-for-c
namespace CachingService
{
    public static class CacheManager<T>
    {
        public static ObjectCache Cache { get; private set; }
        static CacheManager()
        {
            //Cache = new TypedObjectCache<MyDataType>(typeof(T).ToString());
        }
    }

}

