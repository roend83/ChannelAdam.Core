//-----------------------------------------------------------------------
// <copyright file="DisposableObjectRealProxy.cs">
//     Copyright (c) 2014 Adam Craven. All rights reserved.
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

namespace ChannelAdam.Runtime.Remoting.Proxies
{
    using System;
    using System.Reflection;
    using System.Runtime.Remoting.Messaging;

    /// <summary>
    /// Abstract class that invokes the ProxiedObject in process.
    /// </summary>
    public abstract class DisposableObjectRealProxy : DisposableRealProxy
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="DisposableObjectRealProxy"/> class.
        /// </summary>
        /// <param name="typeToProxy">The type to proxy.</param>
        protected DisposableObjectRealProxy(Type typeToProxy)
            : base(typeToProxy)
        {
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

            if (methodInfo == null)
            {
                throw new ArgumentNullException(nameof(methodInfo));
            }

            try
            {
                result = methodInfo.Invoke(onThis, args);
            }
            catch (TargetInvocationException targetEx)
            {
                if (targetEx.InnerException != null)
                {
                    // Unwrap the real exception from the TargetInvocationException
                    throw new AggregateException(new[] { targetEx.InnerException });
                }

                throw;
            }
            ////catch (Exception)
            ////{
            ////    throw;
            ////}

            return result;
        }

        #endregion Virtual Methods for Invoking a Method
    }
}