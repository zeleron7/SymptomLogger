using FluentValidation;
using SymptomLogger.src.DTO.Condition;

namespace SymptomLogger.src.Validators
{
    public class CreateConditionDTOValidator : AbstractValidator<CreateConditionDTO>
    {
        public CreateConditionDTOValidator() 
        { 
            RuleFor(x => x.Name)
                .NotEqual("string", StringComparer.OrdinalIgnoreCase)
                .NotEmpty().WithMessage("Condition name is required.");
        }
    }
}
