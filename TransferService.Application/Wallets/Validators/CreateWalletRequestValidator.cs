using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransferService.Application.Wallets.Validators
{
    public class CreateWalletRequestValidator : AbstractValidator<CreateWalletRequest>
    {
        public CreateWalletRequestValidator()
        {
            RuleFor(x => x.DocumentId)
                .NotEmpty().WithMessage("An identity document is mandatory..")
                .MaximumLength(20).WithMessage("The document id cannot exceed 20 characters.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is mandatory.")
                .MaximumLength(120).WithMessage("Name cannot exceed 20 characters.");

            RuleFor(x => x.InitialBalance)
                .GreaterThanOrEqualTo(0).WithMessage("The initial balance cannot be negative.");
        }
    }
}
