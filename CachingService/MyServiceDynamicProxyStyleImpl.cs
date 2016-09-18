using System;
using System.Collections.Generic;
using Autofac.Extras.DynamicProxy2;

namespace CachingService
{
    [Intercept(typeof(CallLogger))]
    public class MyServiceDynamicProxyStyleImpl :IMyService
    {
        public virtual MyDataType Get(string id)
        {
            return new MyDataType {
                Id = id,
                LastUpdate = DateTime.Now,
                MyPropertyId = 3,
                Name = "MyName"
            };
        }

        public virtual List<MyDataType> GetAll()
        {
            return null;
        }
    }
}

