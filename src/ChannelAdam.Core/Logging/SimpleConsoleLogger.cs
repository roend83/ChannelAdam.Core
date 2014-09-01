//-----------------------------------------------------------------------
// <copyright file="SimpleConsoleLogger.cs">
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

namespace ChannelAdam.Logging
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Writes a message out to the Console with a date/time prefix.
    /// </summary>
    public class SimpleConsoleLogger : ChannelAdam.Logging.ISimpleLogger
    {
        /// <summary>
        /// Writes the line.
        /// </summary>
        /// <param name="messageFormat">The message format.</param>
        /// <param name="arguments">The argument list for the message format.</param>
        public void Log(string messageFormat, params object[] arguments)
        {
            Console.WriteLine(GetMessagePrefix() + messageFormat, arguments); 
        }

         /// <summary>
        /// Writes a blank line.
        /// </summary>
        public void Log()
        {
            Console.WriteLine();
        }

        /// <summary>
        /// Writes the line.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Log(string message)
        {
            Console.WriteLine(GetMessagePrefix() + message);
        }

        /// <summary>
        /// Gets a message's prefix - the current date followed by a dash.
        /// </summary>
        /// <returns>The message prefix.</returns>
        private static string GetMessagePrefix()
        {
            return string.Format("{0:dd/MM/yy hh:mm:ss tt} - ", DateTime.Now);
        }
    }
}
