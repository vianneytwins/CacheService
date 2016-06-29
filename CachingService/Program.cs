using System;
using Autofac;
using System.Timers;
using System.Threading;
using log4net.Config;

namespace CachingService
{
    class Program
    {
        private static IContainer Container { get; set; }

        public static void Main(string[] args)
        {
            // Set up a simple configuration that logs on the console.
            BasicConfigurator.Configure();

            Console.WriteLine("Hello World!");

            var builder = new ContainerBuilder();
            builder.RegisterModule(new LoggingModule());

            Console.WriteLine("My cache name is" + typeof(MyDataType).ToString());
            //var myCache = new TypedObjectCache<MyDataType>(typeof(MyDataType).ToString());
            //builder.RegisterInstance(myCache).ExternallyOwned();

            builder.RegisterType<TypedObjectCache<MyDataType>>()
                .WithParameter("name", typeof(MyDataType).ToString())
                .SingleInstance();

            builder.RegisterType<MyService>().As<IMyService>(); 
            Container = builder.Build();

            using (var scope = Container.BeginLifetimeScope())
            {
                var service = scope.Resolve<IMyService>();
                var data = service.Get("Funky");
                Console.WriteLine(data);
                Thread.Sleep(1000);
                data = service.Get("Funky2");
                Console.WriteLine(data);
                data = service.Get("Funky");
                Console.WriteLine(data);

                Console.WriteLine("Let's try the get All");
                var items = service.GetAll();
                Console.WriteLine(items.Count);
                foreach (var item in items)
                {
                    Console.WriteLine(item);
                }
            }

            //SimpleMethod(); 

            Console.ReadLine();
        }

        static void SimpleMethod()
        {
            MyCache objCache = new MyCache();
            String strUserName = objCache.Get("USER_NAME") as String;
            if (String.IsNullOrEmpty(strUserName))
            {
                objCache.Set("USER_NAME", "MyItem");
            }
            Console.WriteLine(objCache.Get("USER_NAME") as String);
        }
    }
}
