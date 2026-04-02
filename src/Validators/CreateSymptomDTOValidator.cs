using FluentValidation;
using SymptomLogger.src.DTO.Symptom;

namespace SymptomLogger.src.Validators
{
    public class CreateSymptomDTOValidator : AbstractValidator<CreateSymptomDTO>
    {
        // Validator for CreateSymptomDTO (request)
        public CreateSymptomDTOValidator()
        {
            RuleFor(x => x.Category)
                .NotEqual("string", StringComparer.OrdinalIgnoreCase)
                .NotEmpty().WithMessage("Symptom Category is required.");

            RuleFor(x => x.Description)
                .NotEqual("string", StringComparer.OrdinalIgnoreCase)
                .NotEmpty().WithMessage("Symptom description is required.");

            RuleFor(x => x.Name)
                .NotEqual("string", StringComparer.OrdinalIgnoreCase)
                .NotEmpty().WithMessage("Symptom name is required.");

        }
    }
}
