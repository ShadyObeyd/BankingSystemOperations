using BankingSystemOperations.Data.Dtos;
using FluentValidation;

namespace BankingSystemOperations.Data.Validators;

public class TransactionsValidator : AbstractValidator<TransactionDto>
{
    public TransactionsValidator()
    {
        RuleFor(t => t.Direction)
            .NotEmpty()
            .NotNull()
            .Must(BeValidDirection)
            .WithMessage("Direction should be \"D\" or \"C\"");

        RuleFor(t => t.Amount)
            .InclusiveBetween(0m, decimal.MaxValue)
            .WithMessage("Amount must be a positive number.");


        RuleFor(t => t.Currency)
            .NotEmpty()
            .NotNull()
            .Length(3, 3)
            .WithMessage("Currency must be exaclty 3 letters long.");
        
        RuleFor(t => t.DeptorIBAN)
            .NotEmpty()
            .NotNull()
            .Length(15, 34)
            .WithMessage("Deptor IBAN must be betweem 15 and 34 symbols.");
        
        RuleFor(t => t.BeneficiaryIBAN)
            .NotEmpty()
            .NotNull()
            .Length(15, 34)
            .WithMessage("Beneficiary IBAN must be betweem 15 and 34 symbols.");
        
        RuleFor(t => t.ExternalId)
            .NotEmpty()
            .NotNull()
            .Length(2, 100)
            .WithMessage("External Id must be between 2 and 100 symbols.");
        
        RuleFor(t => t.Status)
            .NotEmpty()
            .NotNull()
            .WithMessage("Status must be either 0 (failed) or 1 (succeeded).)");
    }

    private bool BeValidDirection(char direction)
    {
        return direction == 'D' || direction == 'C';
    }
}