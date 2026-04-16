using FluentValidation;
using SymptomLogger.src.DTO.Patient;

namespace SymptomLogger.src.Validators
{
    public class CreatePatientDTOValidator : AbstractValidator<CreatePatientDTO>
    {
        // Validator for CreatePatientDTO (request)
        public CreatePatientDTOValidator() 
        {
            RuleFor(x => x.FirstName)
                .NotEqual("string", StringComparer.OrdinalIgnoreCase)
                .NotEmpty().WithMessage("First name is required.");

            RuleFor(x => x.LastName)
                .NotEqual("string", StringComparer.OrdinalIgnoreCase)
                .NotEmpty().WithMessage("Last name is required.");

            RuleFor(x => x.Age)
                .GreaterThan(0).WithMessage("Age must be greater than 0.");

        }
    }
}
