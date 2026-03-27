using FluentValidation;
using SymptomLogger.src.DTO.Patient;

namespace SymptomLogger.src.Validators
{
    public class CreatePatientDTOValidator : AbstractValidator<CreatePatientDTO>
    {
        // Validator for CreatePatientDTO
        public CreatePatientDTOValidator() 
        {
            RuleFor(x => x.FirstName)
                .NotEqual("string", StringComparer.OrdinalIgnoreCase)
                .NotEmpty().WithMessage("First name is required.");

            RuleFor(x => x.LastName)
                .NotEqual("string", StringComparer.OrdinalIgnoreCase)
                .NotEmpty().WithMessage("Last name is required.");
        }
    }
}
