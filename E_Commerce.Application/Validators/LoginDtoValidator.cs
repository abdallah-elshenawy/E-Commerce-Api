

using E_Commerce.Application.DTOs.AuthDTOs;
using FluentValidation;

namespace E_Commerce.Application.Validators
{
    public class LoginDtoValidator : AbstractValidator<LoginDto>
    {
        public LoginDtoValidator()
        {
            RuleFor(e => e.Email)
                .NotEmpty()
                .EmailAddress();
            RuleFor(p => p.Password)
                .NotEmpty()
                .MinimumLength(8);
        }
    }
}
