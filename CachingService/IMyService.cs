using System;
using Autofac;
using System.Collections.Generic;

namespace CachingService
{
	public interface IMyService
	{
        [Cached]
        MyDataType Get (string id);

        List<MyDataType> GetAll ();
	}


}
