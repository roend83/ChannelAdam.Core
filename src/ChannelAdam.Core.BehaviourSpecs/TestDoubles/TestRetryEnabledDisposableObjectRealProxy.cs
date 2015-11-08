using ChannelAdam.TransientFaultHandling;
using ChannelAdam.Runtime.Remoting.Proxies;

namespace ChannelAdam.Core.BehaviourSpecs.TestDoubles
{
    public class TestRetryEnabledDisposableObjectRealProxy<TObjectToProxy> : RetryEnabledDisposableObjectRealProxy
    {
        public readonly TObjectToProxy obj;

        public TestRetryEnabledDisposableObjectRealProxy(TObjectToProxy objectToProxy, IRetryPolicyFunction retryPolicy)
            : base(typeof(IFakeService), retryPolicy)
        {
            this.obj = objectToProxy;
        }

        protected override object ProxiedObject
        {
            get
            {
                return this.obj;
            }
        }
    }
}
