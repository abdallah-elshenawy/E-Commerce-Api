using E_Commerce.Application.DTOs.AuthDTOs;
using FluentValidation;

namespace E_Commerce.Application.Validators
{
    public class RegisterCustomerDtoValidator : AbstractValidator<RegisterCustomerDto>
    {
        public RegisterCustomerDtoValidator()
        {
            RuleFor(fn => fn.FirstName)
                .NotEmpty()
                .WithMessage("Customer first name is required.")
                .MaximumLength(100)
                .WithMessage("Customer first name cannot exceed 100.");

            RuleFor(ln => ln.LastName)
                .NotEmpty()
                .WithMessage("Customer last name is required.")
                .MaximumLength(100)
                .WithMessage("Customer last name cannot exceed 100.");

            RuleFor(e => e.Email)
                .NotEmpty()
                .WithMessage("Customer email is required.")
                .EmailAddress()
                .MaximumLength(256)
                .WithMessage("Customer email cannot exceed 256 characters.");

            RuleFor(e => e.Password)
                .NotEmpty()
                .WithMessage("Password cannot be empty!")
                .MinimumLength(8)
                .Matches("[a-z]").WithMessage("Must contain lowercase.")
                .Matches("[A-Z]").WithMessage("Must contain uppercase.")
                .Matches("[0-9]").WithMessage("Must contain digit.")
                .Matches("[!@$%+_#*]").WithMessage("Must contain a special character (! @ $ % + _ # *)");
        }
    }
}
