/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

using System;
using Concepts;
using Concepts.SignUps;
using Dolittle.Commands.Validation;
using FluentValidation;

namespace Domain.SignUps
{
    public class AskToJoinTenantInputValidator : CommandInputValidatorFor<AskToJoinTenant>
    {
        public AskToJoinTenantInputValidator()
        {
            RuleFor(_ => (Guid)_.Id)
                .NotEmpty().WithMessage("A id is required");

            RuleFor(_ => _.TenantOwnerEmail)
                .MustBeValidEmail();

            RuleFor(_=>_.UserId).MustBeValidUserId();

            RuleFor(_ => _.Id).MustBeValidSignUpId();
        }
    }
}