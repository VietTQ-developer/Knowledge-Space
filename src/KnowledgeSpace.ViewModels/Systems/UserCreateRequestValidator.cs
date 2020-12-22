using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace KnowledgeSpace.ViewModels.Systems
{
    public class UserCreateRequestValidator : AbstractValidator<UserCreateRequest>
    {
        public UserCreateRequestValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("Username is required");
            
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required");
            
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required")
                .Matches(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$").WithMessage("Email format is not match");
            
            
            RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("Phone number is required");
           
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("First Name is required")
                .MaximumLength(50).WithMessage("First Name cant over limit 50 characters");
            
            RuleFor(x => x.LastName).NotEmpty().WithMessage("Last Name is required")
                .MaximumLength(50).WithMessage("First Name cant over limit 50 characters");
        }
    }
}
