using Xunit;
using FluentValidation.TestHelper;
using CampaignBudgetAPI.Validators;
using CampaignBudgetAPI.Models;
using System.Collections.Generic;

namespace CampaignBudgetAPI.Tests
{
    public class BudgetRequestValidatorTests
    {
        private readonly BudgetRequestValidator _validator;

        public BudgetRequestValidatorTests()
        {
            _validator = new BudgetRequestValidator();
        }

        [Fact]
        public void Validate_ValidRequest_PassesValidation()
        {
            var request = new BudgetRequest
            {
                TargetAd = "Ad1",
                TotalBudget = 1000,
                tolerance = 1f,
                AgencyFee = 0.1f,
                ThirdPartyFee = 0.05f,
                FixedCosts = 100,
                AdBudgets = new List<AdBudget>
                {
                    new AdBudget { AdName = "Ad1", Budget = 0, UsesThirdPartyTool = false }
                }
            };

            var result = _validator.TestValidate(request);
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validate_NonExistentTargetAd_FailsValidation()
        {
            var request = new BudgetRequest
            {
                TargetAd = "NonExistentAd",
                TotalBudget = 1000,
                tolerance = 1f,
                AgencyFee = 0.1f,
                ThirdPartyFee = 0.05f,
                FixedCosts = 100,
                AdBudgets = new List<AdBudget>
                {
                    new AdBudget { AdName = "Ad1", Budget = 0, UsesThirdPartyTool = false }
                }
            };

            var result = _validator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(r => r.TargetAd)
                  .WithErrorMessage("Target ad must exist in the AdBudgets list.");
        }

        [Fact]
        public void Validate_NegativeTotalBudget_FailsValidation()
        {
            var request = new BudgetRequest
            {
                TargetAd = "Ad1",
                TotalBudget = -1000,
                tolerance = 1f,
                AgencyFee = 0.1f,
                ThirdPartyFee = 0.05f,
                FixedCosts = 100,
                AdBudgets = new List<AdBudget>
                {
                    new AdBudget { AdName = "Ad1", Budget = 0, UsesThirdPartyTool = false }
                }
            };

            var result = _validator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(r => r.TotalBudget)
                  .WithErrorMessage("Total budget must be greater than zero.");
        }

        [Fact]
        public void Validate_NegativeTolerance_FailsValidation()
        {
            var request = new BudgetRequest
            {
                TargetAd = "Ad1",
                TotalBudget = 1000,
                tolerance = -1f,
                AgencyFee = 0.1f,
                ThirdPartyFee = 0.05f,
                FixedCosts = 100,
                AdBudgets = new List<AdBudget>
                {
                    new AdBudget { AdName = "Ad1", Budget = 0, UsesThirdPartyTool = false }
                }
            };

            var result = _validator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(r => r.tolerance)
                  .WithErrorMessage("Tolerance must be greater than or equal to zero.");
        }

        [Fact]
        public void Validate_ZeroTotalBudget_FailsValidation()
        {
            var request = new BudgetRequest
            {
                TargetAd = "Ad1",
                TotalBudget = 0,
                tolerance = 1f,
                AgencyFee = 0.1f,
                ThirdPartyFee = 0.05f,
                FixedCosts = 100,
                AdBudgets = new List<AdBudget>
                {
                    new AdBudget { AdName = "Ad1", Budget = 0, UsesThirdPartyTool = false }
                }
            };

            var result = _validator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(r => r.TotalBudget)
                  .WithErrorMessage("Total budget must be greater than zero.");
        }

        [Fact]
        public void Validate_EmptyAdBudgetList_FailsValidation()
        {
            var request = new BudgetRequest
            {
                TargetAd = "Ad1",
                TotalBudget = 1000,
                tolerance = 1f,
                AgencyFee = 0.1f,
                ThirdPartyFee = 0.05f,
                FixedCosts = 100,
                AdBudgets = new List<AdBudget>()
            };

            var result = _validator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(r => r.AdBudgets)
                  .WithErrorMessage("AdBudgets list cannot be empty.");
        }

        [Fact]
        public void Validate_NullAdBudgetList_FailsValidation()
        {
            var request = new BudgetRequest
            {
                TargetAd = "Ad1",
                TotalBudget = 1000,
                tolerance = 1f,
                AgencyFee = 0.1f,
                ThirdPartyFee = 0.05f,
                FixedCosts = 100,
                AdBudgets = null
            };

            var result = _validator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(r => r.AdBudgets)
                  .WithErrorMessage("AdBudgets list cannot be null.");
        }

        [Fact]
        public void Validate_EmptyTargetAdName_FailsValidation()
        {
            var request = new BudgetRequest
            {
                TargetAd = "",
                TotalBudget = 1000,
                tolerance = 1f,
                AgencyFee = 0.1f,
                ThirdPartyFee = 0.05f,
                FixedCosts = 100,
                AdBudgets = new List<AdBudget>
                {
                    new AdBudget { AdName = "Ad1", Budget = 0, UsesThirdPartyTool = false }
                }
            };

            var result = _validator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(r => r.TargetAd)
                  .WithErrorMessage("Target ad name cannot be empty.");
        }

        [Fact]
        public void Validate_NullTargetAdName_FailsValidation()
        {
            var request = new BudgetRequest
            {
                TargetAd = null,
                TotalBudget = 1000,
                tolerance = 1f,
                AgencyFee = 0.1f,
                ThirdPartyFee = 0.05f,
                FixedCosts = 100,
                AdBudgets = new List<AdBudget>
                {
                    new AdBudget { AdName = "Ad1", Budget = 0, UsesThirdPartyTool = false }
                }
            };

            var result = _validator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(r => r.TargetAd)
                  .WithErrorMessage("Target ad name cannot be null.");
        }

        [Fact]
        public void Validate_NegativeAgencyFee_FailsValidation()
        {
            var request = new BudgetRequest
            {
                TargetAd = "Ad1",
                TotalBudget = 1000,
                tolerance = 1f,
                AgencyFee = -0.1f,
                ThirdPartyFee = 0.05f,
                FixedCosts = 100,
                AdBudgets = new List<AdBudget>
                {
                    new AdBudget { AdName = "Ad1", Budget = 0, UsesThirdPartyTool = false }
                }
            };

            var result = _validator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(r => r.AgencyFee)
                  .WithErrorMessage("Agency fee must be between 0 and 1 inclusive.");
        }

        [Fact]
        public void Validate_NegativeThirdPartyFee_FailsValidation()
        {
            var request = new BudgetRequest
            {
                TargetAd = "Ad1",
                TotalBudget = 1000,
                tolerance = 1f,
                AgencyFee = 0.1f,
                ThirdPartyFee = -0.05f,
                FixedCosts = 100,
                AdBudgets = new List<AdBudget>
                {
                    new AdBudget { AdName = "Ad1", Budget = 0, UsesThirdPartyTool = false }
                }
            };

            var result = _validator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(r => r.ThirdPartyFee)
                  .WithErrorMessage("Third party fee must be between 0 and 1 inclusive.");
        }

        [Theory]
        [InlineData(0)]
        [InlineData(0.5)]
        [InlineData(1)]
        public void Validate_ValidAgencyFee_PassesValidation(float agencyFee)
        {
            var request = new BudgetRequest
            {
                TargetAd = "Ad1",
                TotalBudget = 1000,
                tolerance = 1f,
                AgencyFee = agencyFee,
                ThirdPartyFee = 0.05f,
                FixedCosts = 100,
                AdBudgets = new List<AdBudget>
                {
                    new AdBudget { AdName = "Ad1", Budget = 0, UsesThirdPartyTool = false }
                }
            };

            var result = _validator.TestValidate(request);
            result.ShouldNotHaveValidationErrorFor(r => r.AgencyFee);
        }

        [Theory]
        [InlineData(-0.1)]
        [InlineData(1.1)]
        public void Validate_InvalidAgencyFee_FailsValidation(float agencyFee)
        {
            var request = new BudgetRequest
            {
                TargetAd = "Ad1",
                TotalBudget = 1000,
                tolerance = 1f,
                AgencyFee = agencyFee,
                ThirdPartyFee = 0.05f,
                FixedCosts = 100,
                AdBudgets = new List<AdBudget>
                {
                    new AdBudget { AdName = "Ad1", Budget = 0, UsesThirdPartyTool = false }
                }
            };

            var result = _validator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(r => r.AgencyFee)
                  .WithErrorMessage("Agency fee must be between 0 and 1 inclusive.");
        }

        [Theory]
        [InlineData(0)]
        [InlineData(0.5)]
        [InlineData(1)]
        public void Validate_ValidThirdPartyFee_PassesValidation(float thirdPartyFee)
        {
            var request = new BudgetRequest
            {
                TargetAd = "Ad1",
                TotalBudget = 1000,
                tolerance = 1f,
                AgencyFee = 0.1f,
                ThirdPartyFee = thirdPartyFee,
                FixedCosts = 100,
                AdBudgets = new List<AdBudget>
                {
                    new AdBudget { AdName = "Ad1", Budget = 0, UsesThirdPartyTool = false }
                }
            };

            var result = _validator.TestValidate(request);
            result.ShouldNotHaveValidationErrorFor(r => r.ThirdPartyFee);
        }

        [Theory]
        [InlineData(-0.1)]
        [InlineData(1.1)]
        public void Validate_InvalidThirdPartyFee_FailsValidation(float thirdPartyFee)
        {
            var request = new BudgetRequest
            {
                TargetAd = "Ad1",
                TotalBudget = 1000,
                tolerance = 1f,
                AgencyFee = 0.1f,
                ThirdPartyFee = thirdPartyFee,
                FixedCosts = 100,
                AdBudgets = new List<AdBudget>
                {
                    new AdBudget { AdName = "Ad1", Budget = 0, UsesThirdPartyTool = false }
                }
            };

            var result = _validator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(r => r.ThirdPartyFee)
                  .WithErrorMessage("Third party fee must be between 0 and 1 inclusive.");
        }
    }
}