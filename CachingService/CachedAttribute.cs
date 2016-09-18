using System;
using System.Collections.Generic;

namespace CachingService
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class CachedAttribute : Attribute
    {
        public string CacheRegion { get; set; }
        public int TimeoutInMinutes { get; set; }
        public Type CacheKeyGenerator { get; set; }
    }

}

