using FluentValidation;
using SymptomLogger.src.DTO.SymptomLog;

namespace SymptomLogger.src.Validators
{
    public class CreateSymptomLogDTOValidator : AbstractValidator<CreateSymptomLogDTO>
    {
        // Validator for CreateSymptomLogDTO (request)
        public CreateSymptomLogDTOValidator()
        {
            RuleFor(x => x.Notes)
                .NotEqual("string", StringComparer.OrdinalIgnoreCase)
                .NotEmpty().WithMessage("Symptom log note is required.");

            RuleFor(x => x.Severity)
                .InclusiveBetween(1, 10).WithMessage("Symptom log severity must be between 1 and 10.");
            RuleFor(x => x.SymptomId)
               .GreaterThan(0).WithMessage("Symptom ID must be greater than 0.");
        }
    }
}
