//-----------------------------------------------------------------------
// <copyright file="StringExtensions.cs">
//     Copyright (c) 2017 Adam Craven. All rights reserved.
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

namespace System
{
    using System.Text.RegularExpressions;

    public static class StringExtensions
    {
        #region Private Fields

        private static readonly Regex PlaceholderRegExpression = new Regex(@"\{([^\}]+)\}", RegexOptions.Compiled | RegexOptions.Multiline);

        #endregion Private Fields

        #region Public Methods

        public static string FormatNamedPlaceholdersWith(this string format, params object[] args)
        {
            return string.Format(ConvertNamedPlaceholdersToIndexes(format), args);
        }

        public static string FormatWith(this string format, params object[] args)
        {
            return string.Format(format, args);
        }

        #endregion Public Methods

        #region Private Methods

        public static string ConvertNamedPlaceholdersToIndexes(string template)
        {
            int index = 0;
            return PlaceholderRegExpression.Replace(template, (match) => $"{{{index++}}}");
        }

        #endregion Private Methods
    }
}
