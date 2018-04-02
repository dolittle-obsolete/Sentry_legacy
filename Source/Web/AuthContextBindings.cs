/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using System.Threading;
using Concepts;
using Dolittle.DependencyInversion;
using Infrastructure;
using Read.Management;

namespace Web
{
    /// <summary>
    /// 
    /// </summary>
    public class AuthContextBindings : ICanProvideBindings
    {
        static AsyncLocal<AuthContext> _authContext = new AsyncLocal<AuthContext>();

        internal static AuthContext AuthContext
        { 
            get { return _authContext.Value; }
            set { _authContext.Value = value; }
        }

        /// <inheritdoc/>
        public void Provide(IBindingProviderBuilder builder)
        {
            builder.Bind<AuthContext>().To(()=> {
                var value = _authContext.Value;
                return value;
            });
        }
    }
}