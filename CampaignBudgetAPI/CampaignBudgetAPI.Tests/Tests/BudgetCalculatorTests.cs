using Xunit;
using CampaignBudgetAPI.Services;
using CampaignBudgetAPI.Models;
using System.Collections.Generic;
using System.Reflection;

namespace CampaignBudgetAPI.Tests
{
    public class BudgetCalculatorTests
    {
        private readonly BudgetCalculator _calculator;

        public BudgetCalculatorTests()
        {
            _calculator = new BudgetCalculator(); 
        }

        [Fact]
        public void GoalSeek_ValidInput_ReturnsSuccessResponse()
        {
            var request = new BudgetRequest
            {
                TargetAd = "Ad1",
                TotalBudget = 1000,
                tolerance = 10f,
                AgencyFee = 1f,
                ThirdPartyFee = 0.00f,
                FixedCosts = 100,
                AdBudgets = new List<AdBudget>
                {
                    new AdBudget { AdName = "Ad1", Budget = 0, UsesThirdPartyTool = false }
                }
            };

            var response = _calculator.GoalSeek(request);
            
            Assert.True(response.IsSuccess);
            Assert.NotNull(response.CalculatedTotalBudget);
            Assert.Equal("Ad1", response.TargetAd);
        }

        [Fact]
        public void GoalSeek_MultipleAds_ReturnsSuccessResponse()
        {
            var request = new BudgetRequest
            {
                TargetAd = "Ad2",
                TotalBudget = 2000,
                tolerance = 1f,
                AgencyFee = 0.1f,
                ThirdPartyFee = 0.05f,
                FixedCosts = 100,
                AdBudgets = new List<AdBudget>
                {
                    new AdBudget { AdName = "Ad1", Budget = 500, UsesThirdPartyTool = false },
                    new AdBudget { AdName = "Ad2", Budget = 0, UsesThirdPartyTool = true },
                    new AdBudget { AdName = "Ad3", Budget = 300, UsesThirdPartyTool = false }
                }
            };

            var response = _calculator.GoalSeek(request);

            Assert.True(response.IsSuccess);
            Assert.NotNull(response.CalculatedTotalBudget);
            Assert.Equal("Ad2", response.TargetAd);
        }

        [Fact]
        public void GoalSeek_HighTolerance_ReturnsSuccessResponse()
        {
            var request = new BudgetRequest
            {
                TargetAd = "Ad1",
                TotalBudget = 1000,
                tolerance = 100f,
                AgencyFee = 0.1f,
                ThirdPartyFee = 0.05f,
                FixedCosts = 100,
                AdBudgets = new List<AdBudget>
                {
                    new AdBudget { AdName = "Ad1", Budget = 0, UsesThirdPartyTool = false }
                }
            };

            var response = _calculator.GoalSeek(request);

            Assert.True(response.IsSuccess);
            Assert.NotNull(response.CalculatedTotalBudget);
            Assert.InRange((double) response.CalculatedTotalBudget, 900, 1100);
        }

        [Fact]
        public void GoalSeek_ThirdPartyToolUsage_ReturnsSuccessResponse()
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
                    new AdBudget { AdName = "Ad1", Budget = 0, UsesThirdPartyTool = true }
                }
            };

            var response = _calculator.GoalSeek(request);

            Assert.True(response.IsSuccess);
            Assert.NotNull(response.CalculatedTotalBudget);
            Assert.True(response.ThirdPartyFeeSpend > 0);
        }

        [Fact]
        public void GoalSeek_AdBudgetsExceedTotalBudget_ThrowsArgumentException()
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
                    new AdBudget { AdName = "Ad1", Budget = 300, UsesThirdPartyTool = false },
                    new AdBudget { AdName = "Ad2", Budget = 400, UsesThirdPartyTool = false },
                    new AdBudget { AdName = "Ad3", Budget = 300, UsesThirdPartyTool = false },
                    new AdBudget { AdName = "Ad4", Budget = 600, UsesThirdPartyTool = false }
                }
            };

            Assert.Throws<ArgumentException>(() => _calculator.GoalSeek(request));
        }

        [Fact]
        public void GoalSeek_TargetAdNotInAdBudgets_ThrowsArgumentException()
        {
            var request = new BudgetRequest
            {
                TargetAd = "NotInList",
                TotalBudget = 1000,
                tolerance = 1f,
                AgencyFee = 0.1f,
                ThirdPartyFee = 0.05f,
                FixedCosts = 100,
                AdBudgets = new List<AdBudget>
                {
                    new AdBudget { AdName = "Ad1", Budget = 10, UsesThirdPartyTool = false },
                    new AdBudget { AdName = "Ad2", Budget = 10, UsesThirdPartyTool = false },
                    new AdBudget { AdName = "Ad3", Budget = 10, UsesThirdPartyTool = false },
                    new AdBudget { AdName = "Ad4", Budget = 10, UsesThirdPartyTool = false }
                }
            };

            Assert.Throws<ArgumentException>(() => _calculator.GoalSeek(request));
        }
    }
}