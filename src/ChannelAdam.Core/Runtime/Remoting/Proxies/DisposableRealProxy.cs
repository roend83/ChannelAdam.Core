//-----------------------------------------------------------------------
// <copyright file="DisposableRealProxy.cs">
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
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.Remoting;
    using System.Runtime.Remoting.Messaging;
    using System.Runtime.Remoting.Proxies;
    using System.Security;
    using System.Security.Permissions;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Abstract class that implements the Dispose Pattern on top of a <see cref="RealProxy"/>.
    /// </summary>
    /// <remarks>
    /// The Dispose Pattern - <see cref="http://msdn.microsoft.com/en-us/library/b1yfkh5e.aspx"/>
    /// <see cref="http://msdn.microsoft.com/en-us/library/vstudio/b1yfkh5e(v=vs.100).aspx"/>
    /// </remarks>
    public abstract class DisposableRealProxy : RealProxy, IDisposable, IRemotingTypeInfo
    {
        #region Fields

        private readonly Type typeToProxy;

        private bool isDisposed = false;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="DisposableRealProxy"/> class.
        /// </summary>
        /// <param name="typeToProxy">The type to proxy.</param>
        protected DisposableRealProxy(Type typeToProxy) : base(typeToProxy)
        {
            this.typeToProxy = typeToProxy;
        }
         
        #endregion

        #region Destructor

        /// <summary>
        /// Finalizes an instance of the <see cref="DisposableRealProxy"/> class.
        /// </summary>
        /// <remarks>
        /// This destructor will be called by the GC only if the Dispose method does not get called.
        /// Do not provide destructors in types derived from this class - derived types should instead override the Dispose method.
        /// </remarks>
        ~DisposableRealProxy()
        {
            try
            {
                this.Dispose(false);
            }
            catch (Exception e)
            {
                // Suppress exceptions thrown from a destructor/finalizer so they do not go back into the Garbage Collector!
                // But, allow the client code to handle the exception - e.g. log it
                this.OnDestructorException(e);
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when the object is about to be disposed.
        /// </summary>
        public event EventHandler<DisposingEventArgs> Disposing;

        /// <summary>
        /// Occurs when the object has been disposed.
        /// </summary>
        public event EventHandler Disposed;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets a value indicating whether this instance is disposed.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is disposed; otherwise, <c>false</c>.
        /// </value>
        public bool IsDisposed
        {
            get { return this.isDisposed; }
        }

        /// <summary>
        /// Gets or sets the behaviour to perform when an exception occurs during the destructor/finalize.
        /// </summary>
        /// <value>
        /// The exception behaviour.
        /// </value>
        public Action<Exception> DestructorExceptionBehaviour
        {
            get;
            set;
        }

        #endregion

        #region Protected Properties - IRemotingTypeInfo Implementation

        /// <summary>
        /// Gets or sets the fully qualified type name of the server object in a <see cref="T:System.Runtime.Remoting.ObjRef" />.
        /// </summary>
        /// <value>The fully qualified type name of the server object in a <see cref="T:System.Runtime.Remoting.ObjRef" />.</value>
        public string TypeName
        {
            get { return this.typeToProxy.FullName; }
            set { }
        }

        #endregion

        #region Protected Properties

        /// <summary>
        /// Gets the actual object that is being proxied.
        /// </summary>
        /// <value>
        /// The proxied object.
        /// </value>
        protected abstract object ProxiedObject { get; }

        #endregion

        #region Public Methods - Dispose Pattern

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);

            // This object is being cleaned up by the Dispose method.
            // Calling GC.SupressFinalize() takes this object off the finalization queue and prevents
            // finalization code for this object from executing a second time.
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Public Methods - IRemotingTypeInfo Implementation

        /// <summary>
        /// Determines whether the ProxiedObject can be cast to the specified fromType.
        /// </summary>
        /// <param name="fromType">The type to test a cast to.</param>
        /// <param name="o">The object to be cast (which in this case will be "this" transparent proxy - an instance of <c>System.Runtime.Remoting.Proxies.__TransparentProxy</c>).</param>
        /// <returns>
        /// Returns <c>true</c> if the ProxiedObject can be cast to the type.
        /// </returns>
        [SecurityCritical]
        public bool CanCastTo(Type fromType, object o)
        {
            if (fromType == null)
            {
                return false;
            }

            return fromType == this.typeToProxy || 
                   fromType == typeof(IDisposable) || 
                   fromType.IsInstanceOfType(this.ProxiedObject);
        }

        #endregion

        #region Public Methods - RealProxy - Overrides

        /// <summary>
        /// Invokes the specified message.
        /// </summary>
        /// <param name="msg">The message.</param>
        /// <returns>The IMessage.</returns>
        /// <remarks>
        /// This method returns exceptions it caught in the return object ReturnMessage.Exception property.
        /// </remarks>
        public override IMessage Invoke(IMessage msg)
        {
            IMessage returnMessage;

            var methodCallMessage = (IMethodCallMessage)msg;

            try
            {
                if (this.isDisposed)
                {
                    throw new ObjectDisposedException(string.Format("{0} proxying {1}", this.GetType().FullName, this.typeToProxy.FullName));
                }

                object result = null;
                
                if (methodCallMessage.MethodName == "Dispose")
                {
                    result = this.InvokeMethod(methodCallMessage, this);
                }
                else
                {
                    result = this.InvokeMethod(methodCallMessage, this.ProxiedObject);
                }

                returnMessage = new ReturnMessage(
                                result,                                 // Operation result
                                null,                                   // Out arguments
                                0,                                      // Out arguments count
                                methodCallMessage.LogicalCallContext,   // Call context
                                methodCallMessage);                     // Original message
            }
            catch (Exception e)
            {
                returnMessage = new ReturnMessage(e, methodCallMessage);
            }

            return returnMessage;
        }

        #endregion

        #region Protected Methods - RealProxy - Abstract Method for Invoking a Method

        /// <summary>
        /// Invokes the method and returns the object to be returned to the caller of the proxy.
        /// </summary>
        /// <param name="methodCallMessage">The method call message.</param>
        /// <param name="onThis">The object on which to invoke the method.</param>
        /// <returns>The object to be returned to the caller of the proxy.</returns>
        /// <exception cref="System.InvalidCastException">The value of methodCallMessage.MethodBase cannot be cast to a MethodInfo.</exception>
        protected abstract object InvokeMethod(IMethodCallMessage methodCallMessage, object onThis);

        #endregion

        #region Protected Methods - Dispose Pattern - Virtual Methods

        /// <summary>
        /// Release managed resources here.
        /// </summary>
        protected virtual void DisposeManagedResources()
        {
            // override this method
        }

        /// <summary>
        /// Release unmanaged resources here.
        /// </summary>
        protected virtual void DisposeUnmanagedResources()
        {
            // override this method
        }

        /// <summary>
        /// Called when the object is about to be disposed.
        /// </summary>
        /// <param name="isDisposing">If set to <c>true</c> then the object is being disposed from a call to Dispose(); <c>false</c> if it is from a finalizer/destructor.</param>
        protected virtual void OnDisposing(bool isDisposing)
        {
            if (this.Disposing != null)
            {
                this.Disposing(this, new DisposingEventArgs(isDisposing));
            }
        }

        /// <summary>
        /// Called when the object has been disposed.
        /// </summary>
        protected virtual void OnDisposed()
        {
            if (this.Disposed != null)
            {
                this.Disposed(this, EventArgs.Empty);
            }
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Called when there is an exception in the destructor/finalize.
        /// </summary>
        /// <param name="exception">The exception.</param>
        protected virtual void OnDestructorException(Exception exception)
        {
            try
            {
                if (this.DestructorExceptionBehaviour != null)
                {
                    this.DestructorExceptionBehaviour(exception);
                }
            }
            catch (Exception ex)
            {
                // Failsafe - Suppress this so they do not go back into the Garbage Collector!
                Console.Error.WriteLine("Exception occurred during destructor: " + ex.ToString());
            }
        }

        #endregion

        #region Private Methods - Dispose Pattern

        private void Dispose(bool disposing)
        {
            if (this.isDisposed)
            {
                return;
            }

            this.OnDisposing(disposing);

            this.DisposeUnmanagedResources();

            if (disposing)
            {
                this.DisposeManagedResources();
            }

            this.isDisposed = true;

            this.OnDisposed();
        }

        #endregion
    }
}
