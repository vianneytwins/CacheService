using System;
using Autofac;
using System.Collections.Generic;
using System.Dynamic;
using KingAOP;
using System.Linq.Expressions;

namespace CachingService
{
    public class MyServiceKingAopStyleImpl : IMyService, IDynamicMetaObjectProvider
	{
        private TypedObjectCache<MyDataType> cache;

        public MyServiceKingAopStyleImpl(TypedObjectCache<MyDataType> cache)
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

        [MyFirstAspect]
        public MyDataType Get(string id)
        {
            var returnData = new MyDataType();
            this.cache.TryGetAndSet(id,() => InternalGet(id), out returnData);
            return returnData;
        }

        [MyFirstAspect]
        public List<MyDataType> GetAll()
        {
            return this.cache.GetAllCachedItems();
        }

        public DynamicMetaObject GetMetaObject(Expression parameter)
        {
            return new AspectWeaver(parameter, this); // this AspectWeaver will inject AOP mechanics.
        }
	}

}
