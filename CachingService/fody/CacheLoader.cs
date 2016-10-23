using System;
using log4net;
using CachingService;
using Autofac;

namespace fody
{
    public class CacheLoader
    {
        private ILog logger;

        IComponentContext iocContainer;

        public CacheLoader(ILog logger, IComponentContext iocContainer)
        {
            this.logger = logger;
            this.iocContainer = iocContainer;
        }

        public void LoadCache()
        {
            //list of call that needs to be done to load the cache
            var myService = this.iocContainer.Resolve<IMyService>();
            myService.Get("Funky");
        }
    }
}

