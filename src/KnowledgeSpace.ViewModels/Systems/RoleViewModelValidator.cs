using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace KnowledgeSpace.ViewModels.Systems
{
    public class RoleViewModelValidator : AbstractValidator<RoleCreateRequest>
    {
        public RoleViewModelValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Id value is required")
                .MaximumLength(50).WithMessage("Role Id cant over limit 50 characters!");

            RuleFor(x => x.Name).NotEmpty().WithMessage("Name value is required");
        }
    }
}
