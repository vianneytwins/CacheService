using System;
using Autofac;
using System.Collections.Generic;

namespace CachingService
{
	public interface IMyService
	{
        MyDataType Get (string id);

        List<MyDataType> GetAll ();
	}


}
