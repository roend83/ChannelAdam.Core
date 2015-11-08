using ChannelAdam.TransientFaultHandling;
using System;

namespace ChannelAdam.Core.BehaviourSpecs.TestDoubles
{
    public class SimpleRetryPolicyFunction : IRetryPolicyFunction
    {
        private readonly int maxRetryCount;

        public SimpleRetryPolicyFunction(int maxRetryCount)
        {
            this.maxRetryCount = maxRetryCount;
        }

        public TResult Execute<TResult>(Func<TResult> func)
        {
            Console.WriteLine("Execute()");

            Exception lastException = null;

            for (int i = 0; i <= this.maxRetryCount; i++)
            {
                try
                {
                    if (func != null)
                    {
                        return func.Invoke();
                    }
                }
                catch (Exception ex)
                {
                    lastException = ex;
                    Console.WriteLine("Exception occurred: " + ex.ToString());
                }
            }

            if (lastException != null)
            {
                throw lastException;
            }

            return default(TResult);
        }
    }
}
