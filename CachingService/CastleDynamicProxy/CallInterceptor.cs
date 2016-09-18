using System;
using Castle.DynamicProxy;
using System.Linq;

namespace CachingService
{
    public class CallLogger : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            var className = invocation.InvocationTarget.ToString();
            var methodName = invocation.Method.Name;
            Console.Write("{0}.{1} [IN], parameters {2} ",
                className,
                methodName,
                string.Join(", ", invocation.Arguments.Select(a => (a ?? "").ToString()).ToArray()));

            invocation.Proceed();

            Console.Write("{0}.{1} [OUT]",
                className,
                methodName);
        }
    }
}

