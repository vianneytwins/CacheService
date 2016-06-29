using System;
using Autofac;
using System.Collections.Generic;

namespace CachingService
{
    public class MyService : IMyService
	{
        private TypedObjectCache<MyDataType> cache;

        public MyService(TypedObjectCache<MyDataType> cache)
        {
            this.cache = cache;
        }

        private MyDataType InternalGet(string id)
        {
            return new MyDataType {
                Id = id,
                LastUpdate = DateTime.Now,
                MyPropertyId = 3,
                Name = "MyName"
            };
        }

        public MyDataType Get(string id)
        {
            var returnData = new MyDataType();
            this.cache.TryGetAndSet(id,() => InternalGet(id), out returnData);
            return returnData;
        }

        public List<MyDataType> GetAll()
        {
            return this.cache.GetAllCachedItems();
        }
	}

}
