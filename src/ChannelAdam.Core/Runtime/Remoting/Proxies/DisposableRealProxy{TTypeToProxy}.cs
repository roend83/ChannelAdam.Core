//-----------------------------------------------------------------------
// <copyright file="DisposableRealProxy{TTypeToProxy}.cs">
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
    /// <summary>
    /// Abstract class that implements the Dispose Pattern on top of a <see cref="RealProxy"/>.
    /// </summary>
    /// <remarks>
    /// The Dispose Pattern - <see cref="http://msdn.microsoft.com/en-us/library/b1yfkh5e.aspx"/>
    /// <see cref="http://msdn.microsoft.com/en-us/library/vstudio/b1yfkh5e(v=vs.100).aspx"/>
    /// </remarks>
    /// <typeparam name="TTypeToProxy">The type of the type to proxy.</typeparam>
    public abstract class DisposableRealProxy<TTypeToProxy> : DisposableRealProxy
    {
        protected DisposableRealProxy() : base(typeof(TTypeToProxy))
        {
        }
    }
}
