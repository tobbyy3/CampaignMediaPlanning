using FluentValidation;
using CampaignBudgetAPI.Models;

namespace CampaignBudgetAPI.Validators
{
    public class BudgetRequestValidator : AbstractValidator<BudgetRequest>
    {
        public BudgetRequestValidator()
        {
            RuleFor(x => x.TotalBudget)
                .GreaterThan(0)
                .WithMessage("Total budget must be greater than zero.");

            RuleFor(x => x.tolerance)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Tolerance must be greater than or equal to zero.");

            RuleFor(x => x.FixedCosts)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Fixed costs cannot be negative.");

            RuleFor(x => x.TargetAd)
                .NotEmpty().WithMessage("Target ad name cannot be empty.")
                .NotNull().WithMessage("Target ad name cannot be null.");

            RuleFor(x => x.AdBudgets)
                .NotNull().WithMessage("AdBudgets list cannot be null.")
                .Must(list => list != null && list.Any()).WithMessage("AdBudgets list cannot be empty.");

            RuleFor(x => x.TargetAd)
                .Must((request, targetAd) => request.AdBudgets != null && request.AdBudgets.Any(ad => ad.AdName == targetAd))
                .When(request => request.AdBudgets != null)
                .WithMessage("Target ad must exist in the AdBudgets list.");

            RuleFor(x => x.AgencyFee)
                .InclusiveBetween(0, 1)
                .WithMessage("Agency fee must be between 0 and 1 inclusive.");

            RuleFor(x => x.ThirdPartyFee)
                .InclusiveBetween(0, 1)
                .WithMessage("Third party fee must be between 0 and 1 inclusive.");
        }
    }
}