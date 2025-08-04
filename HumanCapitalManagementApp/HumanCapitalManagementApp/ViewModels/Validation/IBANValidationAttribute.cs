using IbanNet;
using System.ComponentModel.DataAnnotations;
using ValidationResult = System.ComponentModel.DataAnnotations.ValidationResult;
namespace HumanCapitalManagementApp.ViewModels.Validation
{
    public class IBANValidationAttribute : ValidationAttribute
    {
        private static readonly IIbanValidator validator = new IbanValidator();

        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            if (value is not string iban || string.IsNullOrWhiteSpace(iban))
            {
                return new ValidationResult("IBAN is required.");
            }

            var result = validator.Validate(iban);
            return result.IsValid
                ? ValidationResult.Success
                : new ValidationResult("Invalid IBAN format.");
        }
    }
}
