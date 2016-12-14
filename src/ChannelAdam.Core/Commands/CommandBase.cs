﻿//-----------------------------------------------------------------------
// <copyright file="CommandBase.cs">
//     Copyright (c) 2016 Adam Craven. All rights reserved.
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

namespace ChannelAdam.Commands
{
    using ChannelAdam.Commands.Abstractions;

    public abstract class CommandBase : ICommand
    {
        #region Public Methods

        public void Execute()
        {
            if (this.CanExecute())
            {
                this.ExecuteCore();
            }
        }

        #endregion Public Methods

        #region Protected Methods

        protected virtual bool CanExecute()
        {
            return true;
        }

        protected abstract void ExecuteCore();

        #endregion Protected Methods
    }
}