using FluentValidation;
using Marketplace.Orders.Application.DTOs;

namespace Marketplace.Orders.Application.Validators;

public class CreateOrderItemDtoValidator : AbstractValidator<CreateOrderItemDto>
{
    public CreateOrderItemDtoValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("ProductId не должен быть пустым.");

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Количество товара должно быть больше 0.");
    }
}