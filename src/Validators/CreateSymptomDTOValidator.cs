using FluentValidation;
using SymptomLogger.src.DTO.Symptom;

namespace SymptomLogger.src.Validators
{
    public class CreateSymptomDTOValidator : AbstractValidator<CreateSymptomDTO>
    {
        // Validator for CreateSymptomDTO
        public CreateSymptomDTOValidator()
        {
            RuleFor(x => x.Type)
                .NotEqual("string", StringComparer.OrdinalIgnoreCase)
                .NotEmpty().WithMessage("Symptom type is required.");

            RuleFor(x => x.Description)
                .NotEqual("string", StringComparer.OrdinalIgnoreCase)
                .NotEmpty().WithMessage("Symptom description is required.");

            RuleFor(x => x.Name)
                .NotEqual("string", StringComparer.OrdinalIgnoreCase)
                .NotEmpty().WithMessage("Symptom name is required.");

            RuleFor(x => x.Severity)
                .InclusiveBetween(1, 10).WithMessage("Severity must be between 1 and 10.");
        }
    }
}
