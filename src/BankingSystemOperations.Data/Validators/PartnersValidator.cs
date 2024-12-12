using BankingSystemOperations.Data.Dtos;
using FluentValidation;

namespace BankingSystemOperations.Data.Validators;

public class PartnersValidator : AbstractValidator<PartnerDto>
{
    public PartnersValidator()
    {
        RuleFor(p => p.Name)
            .NotEmpty()
            .NotNull()
            .Length(2, 100)
            .WithMessage("Name must be between 2 and 100 letters.");
    }
}