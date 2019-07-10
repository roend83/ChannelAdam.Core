//-----------------------------------------------------------------------
// <copyright file="RetryEnabledDisposableObjectRealProxy.cs">
//     Copyright (c) 2015 Adam Craven. All rights reserved.
// </copyright>
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//    http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//-----------------------------------------------------------------------

using System.Reflection;

namespace ChannelAdam.Runtime.Remoting.Proxies
{
    using System;
    using System.Runtime.Remoting.Messaging;

    using ChannelAdam.TransientFaultHandling;

    /// <summary>
    /// Abstract class that invokes the ProxiedObject in process, following a specified retry policy.
    /// </summary>
    public abstract class RetryEnabledDisposableObjectRealProxy : DisposableObjectRealProxy
    {
        #region Fields

        private readonly IRetryPolicyFunction retryPolicy;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="RetryEnabledDisposableObjectRealProxy" /> class.
        /// </summary>
        /// <param name="typeToProxy">The type to proxy.</param>
        /// <param name="retryPolicy">The retry policy.</param>
        protected RetryEnabledDisposableObjectRealProxy(Type typeToProxy, IRetryPolicyFunction retryPolicy)
            : base(typeToProxy)
        {
            this.retryPolicy = retryPolicy;
        }

        #endregion Constructor

        #region Virtual Methods for Invoking a Method

        /// <summary>
        /// Invokes the method and returns the object to be returned to the caller of the proxy.
        /// </summary>
        /// <param name="methodInfo">The method to invoke.</param>
        /// <param name="onThis">The object on which to invoke the method.</param>
        /// <param name="args">The arguments for the method.</param>
        /// <returns>The object to be returned to the caller of the proxy.</returns>
        /// <exception cref="System.InvalidCastException">The value of methodCallMessage.MethodBase cannot be cast to a MethodInfo.</exception>
        protected override object InvokeMethod(MethodBase methodInfo, object onThis, object[] args)
        {
            object result = null;

            if (this.retryPolicy != null)
            {
                result = this.retryPolicy.Execute(() => base.InvokeMethod(methodInfo, onThis, args));
            }
            else
            {
                result = base.InvokeMethod(methodInfo, onThis, args);
            }

            return result;
        }

        #endregion Virtual Methods for Invoking a Method
    }
}