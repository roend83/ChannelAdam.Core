using System;
using ChannelAdam.Runtime.Remoting.Proxies;
using ChannelAdam.Core.BehaviourSpecs.TestDoubles;
using TechTalk.SpecFlow;
using ChannelAdam.TransientFaultHandling;
using Moq;
using ChannelAdam.TestFramework.MSTest;

namespace ChannelAdam.Core.BehaviourSpecs
{
    [Binding]
    [Scope(Feature = "Retrying")]
    public class RetryingUnitTestSteps : MoqTestFixture
    {
        const int RetryNone = 0;
        const int RetryOnce = 1;

        int retriesCount;
        Mock<IFakeService> mock;
        TestRetryEnabledDisposableObjectRealProxy<IFakeService> proxy;

        #region Setup / Teardown

        [BeforeScenario]
        public void Setup()
        {
            this.mock = base.MyMockRepository.Create<IFakeService>();
        }

        [AfterScenario]
        public void CleanUp()
        {
            Logger.Log("About to verify mock objects");
            base.MyMockRepository.Verify();
        }

        #endregion

        #region Given

        [Given(@"a retry enabled disposable object real proxy for an action that will always fail - with no retry policy specified")]
        public void GivenARetryEnabledDisposableObjectRealProxyForAnActionThatWillAlwaysFail_WithNoRetryPolicySpecified()
        {
            CreateMockThatAlwaysThrowsException();
            CreateRetryProxyWithNoRetryPolicy();
        }

        [Given(@"a retry enabled disposable object real proxy for an action that will always fail - with no retries configured")]
        public void GivenARetryEnabledDisposableObjectRealProxyForAnActionThatWillAlwaysFail_WithNoRetriesConfigured()
        {
            CreateMockThatAlwaysThrowsException();
            CreateRetryProxyWithRetriesCount(RetryNone);
        }

        [Given(@"a retry enabled disposable object real proxy for an action that will always fail - with one retry configured")]
        public void GivenARetryEnabledDisposableObjectRealProxyForAnActionThatWillAlwaysFail_WithOneRetryConfigured()
        {
            CreateMockThatAlwaysThrowsException();
            CreateRetryProxyWithRetriesCount(RetryOnce);
        }

        #endregion

        #region When

        [When(@"the proxy is executed for an action that fails")]
        public void WhenTheProxyIsExecutedForAnActionThatFails()
        {
            var transparent = (IFakeService)this.proxy.GetTransparentProxy();

            base.Try(() => transparent.DoIt());
        }

        #endregion

        #region Then

        [Then(@"the action was executed without retries")]
        [Then(@"the retry policy was invoked and the retries happened as expected")]
        public void ThenTheRetryPolicyWasInvokedAndTheRetriesHappenedAsExpected()
        {
            base.AssertExpectedException();

            int count = this.retriesCount + 1;
            Logger.Log($"Verifying mock was called {count} times");
            this.mock.Verify(m => m.DoIt(), Times.Exactly(count));
        }

        #endregion

        #region Private Methods

        private void CreateMockThatAlwaysThrowsException()
        {
            const string exceptionText = @"This is always thrown";

            this.ExpectedException.MessageShouldContainText = exceptionText;

            this.mock.Setup(m => m.DoIt())
                .Throws(new Exception(exceptionText))
                .Verifiable();
        }

        private void CreateRetryProxyWithNoRetryPolicy()
        {
            this.retriesCount = RetryNone;

            this.proxy = new TestRetryEnabledDisposableObjectRealProxy<IFakeService>(
                this.mock.Object, null);
        }

        private void CreateRetryProxyWithRetriesCount(int retryCount)
        {
            this.retriesCount = retryCount;

            this.proxy = new TestRetryEnabledDisposableObjectRealProxy<IFakeService>(
                this.mock.Object, new SimpleRetryPolicyFunction(this.retriesCount));
        }

        #endregion
    }
}
