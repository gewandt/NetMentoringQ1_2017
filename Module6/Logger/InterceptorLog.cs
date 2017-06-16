using Castle.DynamicProxy;
using Newtonsoft.Json;
using System;
using System.Reflection;
using System.Text;

namespace Logger
{
    public class InterceptorLog : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            var method = invocation.Method;
            var methodParametes = method.GetParameters();
            var arguments = invocation.Arguments;

            Log($"{DateTime.Now} called {method.Name} with parameters: {AppendMethodParameters(methodParametes)}");

            invocation.Proceed();
            var result = JsonConvert.SerializeObject(invocation.ReturnValue);

            Log($"{DateTime.Now} {method.Name} returns: {result}");
        }

        private static string AppendMethodParameters(ParameterInfo[] methodParametes)
        {
            StringBuilder parameters = new StringBuilder();
            foreach (var item in methodParametes)
            {
                var paramNane = item.Name;
                var paramValue = JsonConvert.SerializeObject(item);
                parameters.AppendFormat("{0}:{1} ", paramNane, paramValue);
            }
            return parameters.ToString();
        }

        private void Log(string content)
        {
            Console.WriteLine(content);
        }
    }
}
