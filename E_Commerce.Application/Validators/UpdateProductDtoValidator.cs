
using E_Commerce.Application.DTOs.ProductDTOs;
using FluentValidation;

namespace E_Commerce.Application.Validators
{
    public class UpdateProductDtoValidator : AbstractValidator<UpdateProductDto>
    {
        public UpdateProductDtoValidator()
        {
            RuleFor(n => n.Name)
                .NotEmpty().WithMessage("Product  name is required.")
                .MaximumLength(200)
                .WithMessage("Name cannot exceed 200 characters.");

            RuleFor(d => d.Description)
                .MaximumLength(1000);

            RuleFor(p => p.Price)
                .GreaterThan(0)
                .WithMessage("Price must be greater than zero.");
        }
    }
}
