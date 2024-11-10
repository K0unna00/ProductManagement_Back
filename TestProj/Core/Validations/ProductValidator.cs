using FluentValidation;
using TestProj.API.DTOs;

namespace TestProj.Core.Validations;

public class ProductValidator : AbstractValidator<ProductDto>
{
    public ProductValidator()
    {
        RuleFor(p => p.Name).NotEmpty().WithMessage("Boş ola bilməz").
            Length(3, 20).WithMessage("3 və 20 simvol aralığında olmalıdır");

        RuleFor(p => p.Price).NotEmpty().WithMessage("Boş ola bilməz").
            GreaterThanOrEqualTo(1).WithMessage("0 və ya 0dan kiçik ola bilməz");

        RuleFor(p => p.Description).NotEmpty().WithMessage("Boş ola bilməz").
            Length(3, 40).WithMessage("3 və 40 simvol aralığında olmalıdır");

        RuleFor(p => p.Image).NotNull().WithMessage("Boş ola bilməz");
    }
}
