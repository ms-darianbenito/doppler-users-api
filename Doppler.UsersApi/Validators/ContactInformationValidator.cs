using Doppler.UsersApi.Model;
using FluentValidation;

namespace Doppler.UsersApi.Validators
{
    public class ContactInformationValidator : AbstractValidator<ContactInformation>
    {
        public ContactInformationValidator()
        {
            RuleFor(x => x.Firstname)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(x => x.Lastname)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(x => x.Address)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.City)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(x => x.Province)
                .NotEmpty()
                .MaximumLength(255);

            RuleFor(x => x.Country)
                .NotEmpty()
                .MaximumLength(255);

            RuleFor(x => x.ZipCode)
                .MaximumLength(100);

            RuleFor(x => x.Phone)
                .NotEmpty()
                .MaximumLength(25);

            // TODO: When task DAT-525 is deployed in prod, we need to add the next validator
            //RuleFor(x => x.AnswerSecurityQuestion)
            //    .NotEmpty()
            //    .MaximumLength(100);

            RuleFor(x => x.Company)
                .MaximumLength(50);
        }
    }
}
