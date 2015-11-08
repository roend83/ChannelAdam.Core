//-----------------------------------------------------------------------
// <copyright file="IRetryPolicyFunction.cs">
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

namespace ChannelAdam.TransientFaultHandling
{
    using System;

    /// <summary>
    /// Interface for a retry policy.
    /// </summary>
    public interface IRetryPolicyFunction
    {
        /// <summary>
        /// Repetitively executes the specified action while it satisfies the current retry policy.
        /// </summary>
        /// <typeparam name="TResult">The type of result expected from the executable action.</typeparam>
        /// <param name="func">A delegate that represents the executable action that returns the result of type <typeparamref name="TResult"/>.</param>
        /// <returns>The result from the action.</returns>
        TResult Execute<TResult>(Func<TResult> func);
    }
}