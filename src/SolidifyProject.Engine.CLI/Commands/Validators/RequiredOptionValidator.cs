using System.ComponentModel.DataAnnotations;
using McMaster.Extensions.CommandLineUtils;
using McMaster.Extensions.CommandLineUtils.Validation;

namespace SolidifyProject.Engine.CLI.Commands.Validators
{
    public class RequiredOptionValidator: IOptionValidator
    {
        public ValidationResult GetValidationResult(CommandOption option, ValidationContext context)
        {
            if (option.HasValue())
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult($"Option {context.DisplayName} is required");
            }
        }
    }
}