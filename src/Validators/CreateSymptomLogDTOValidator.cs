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
                .NotEmpty().WithMessage("Note is required.");

            RuleFor(x => x.Severity)
                .InclusiveBetween(1, 10).WithMessage("Severity must be between 1 and 10.");

            RuleFor(x => x.SymptomId)
               .GreaterThan(0).WithMessage("Symptom ID must be greater than 0.");

            RuleFor(x => x.DurationMinutes)
                .GreaterThan(0).When(x => x.DurationMinutes.HasValue)
                .WithMessage("Duration in minutes must be greater than 0 if provided.");

            RuleFor(x => x.StressLevel)
                .InclusiveBetween(1, 10).WithMessage("Stress level must be between 1 and 10.");

            RuleFor(x => x.SleepHours)
                .InclusiveBetween(0, 24).WithMessage("Sleep hours must be between 0 and 24.");

            // Make flare required?
        }
    }
}
