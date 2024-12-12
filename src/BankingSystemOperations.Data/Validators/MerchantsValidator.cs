using System.Text.RegularExpressions;
using BankingSystemOperations.Data.Dtos;
using FluentValidation;

namespace BankingSystemOperations.Data.Validators;

public class MerchantsValidator : AbstractValidator<MerchantDto>
{
    public MerchantsValidator()
    {
        RuleFor(m => m.Name)
            .NotNull()
            .NotEmpty()
            .Length(2, 100)
            .WithMessage("Name must be between 2 and 100 letters.");

        RuleFor(m => m.Url)
            .MaximumLength(2048)
            .Must(BeValidUrl)
            .WithMessage("Url must be a valid and no more than 2048 characters long.");
        
        
        RuleFor(m => m.Country)
            .NotEmpty()
            .NotNull()
            .Length(2, 100)
            .WithMessage("Country must be between 2 and 100 letters.");
        
        RuleFor(m => m.FirstAddress)
            .NotEmpty()
            .NotNull()
            .Length(2, 100)
            .WithMessage("First Address must be between 2 and 100 letters.");
        
        RuleFor(m => m.SecondAddress)
            .Length(2, 100)
            .WithMessage("Second Address must be between 2 and 100 letters.");
    }

    private bool BeValidUrl(string url)
    {
        string pattern = @"^(https?:\/\/)?([\w\-]+(\.[\w\-]+)+)(:\d+)?(\/[\w\-.,@?^=%&:/~+#]*)?$";
        return Regex.IsMatch(url, pattern);
    }
}