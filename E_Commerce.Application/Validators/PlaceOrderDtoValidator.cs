
using E_Commerce.Application.DTOs.OrderDTOs;
using FluentValidation;

namespace E_Commerce.Application.Validators
{
    public class PlaceOrderDtoValidator : AbstractValidator<PlaceOrderDto>
    {
        public PlaceOrderDtoValidator()
        {
            RuleFor(items => items.Items)
                .NotEmpty();

            RuleForEach(items => items.Items).ChildRules(item =>
            {
                item.RuleFor(p => p.ProductId)
                    .GreaterThan(0);

                item.RuleFor(q => q.Quantity)
                    .GreaterThan(0)
                    .WithMessage("Quantity must be greater than zero.");
            });
        }
    }
}
