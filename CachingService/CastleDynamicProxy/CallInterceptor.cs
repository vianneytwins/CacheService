using System;
using Castle.DynamicProxy;
using System.Linq;
using System.Runtime.Caching;
using Castle.Core.Internal;

namespace CachingService
{
    public class CallLogger : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            var className = invocation.TargetType.FullName;
            var methodName = invocation.Method.Name;
            var parameters = string.Join(", ", invocation.Arguments.Select(a => (a ?? "").ToString()).ToArray());
            Console.WriteLine("{0}.{1} ({2}) [IN]",
                className,
                methodName,
                parameters);

            var cacheAttribute = invocation.Method.GetAttribute<CachedAttribute>();

            if(cacheAttribute == null)
            {
                invocation.Proceed();
                Console.WriteLine("{0}.{1} [OUT]",
                    className,
                    methodName);
                return;
            }


            var cacheKey =
                String.Concat(className, ".", methodName, "(", String.Join(", ", invocation.Arguments), ")");
            if (MemoryCache.Default.Contains(cacheKey, cacheAttribute.CacheRegion))
            {
                Console.WriteLine("{0}.{1} ({2}) [CACHE HIT]",
                    className,
                    methodName,
                    parameters);
                invocation.ReturnValue = MemoryCache.Default.Get(cacheKey, cacheAttribute.CacheRegion);
            }
            else
            {
                Console.WriteLine("{0}.{1} ({2}) [NOT IN CACHE]",
                    className,
                    methodName,
                    parameters);
                invocation.Proceed();


                var policy = new CacheItemPolicy(); 
                //policy.Priority = (MyCacheItemPriority == MyCachePriority.Default) ? CacheItemPriority.Default : CacheItemPriority.NotRemovable; 
                policy.AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(10.00);

                MemoryCache.Default.Add(cacheKey, invocation.ReturnValue, policy, cacheAttribute.CacheRegion);
            }

            Console.WriteLine("{0}.{1} [OUT]",
                className,
                methodName);
        }


    }
}

