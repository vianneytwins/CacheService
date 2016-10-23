using System;
using CachingService;
using MethodCache.Attributes;

namespace fody
{
    public class MyFodyService : IMyService
    {
        private ICache Cache { get; set;}

        public MyFodyService(ICache cache)
        {
            this.Cache = cache;
        }

        [Cache]
        public MyDataType Get(string id)
        {
            return new MyDataType {
                Id = id,
                LastUpdate = DateTime.Now,
                MyPropertyId = 3,
                Name = "MyName"
            };
        }

        public System.Collections.Generic.List<MyDataType> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}

