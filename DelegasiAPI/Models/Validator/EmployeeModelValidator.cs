using FluentValidation;

namespace DelegasiAPI.Models.Validator
{
    public class EmployeeModelValidator : AbstractValidator<EmployeeModel>
    {
        public EmployeeModelValidator()
        {
            RuleFor(x => x.EmployeeName)
                .NotEmpty();

          
        }

    }
}
