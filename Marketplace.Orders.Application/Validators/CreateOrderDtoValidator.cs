using FluentValidation;
using Marketplace.Orders.Application.DTOs;

namespace Marketplace.Orders.Application.Validators;

public class CreateOrderDtoValidator : AbstractValidator<CreateOrderDto>
{
    public CreateOrderDtoValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId не должен быть пустым.");

        RuleFor(x => x.Items)
            .NotEmpty().WithMessage("Список товаров (Items) не может быть пустым.");

        RuleForEach(x => x.Items)
            .SetValidator(new CreateOrderItemDtoValidator());
    }
}