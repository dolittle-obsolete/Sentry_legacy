/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using Dolittle.Commands.Validation;
using FluentValidation;

namespace Domain.Studio.Registration
{
    /// <summary>
    /// Represents the validation for <see cref="RequestAccessWithEmail"/>
    /// </summary>
    public class RequestAccessWithEmailInputValidator : CommandInputValidatorFor<RequestAccessWithEmail>
    {
        /// <summary>
        /// Initializes a new instance of <see cref="RequestAccessWithEmailInputValidator"/>
        /// </summary>
        public RequestAccessWithEmailInputValidator()
        {
            RuleFor(_ => _.Tenant).NotNull().WithMessage("Must specify tenant");
            RuleFor(_ => _.Application).NotNull().WithMessage("Must specify application");
            RuleFor(_ => _.Email).EmailAddress().WithMessage("Must be a valid email");
        }
    }
}