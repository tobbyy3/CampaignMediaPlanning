
namespace CampaignBudgetAPI.Models
{
    public class BudgetResponse
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
        public double? Budget { get; set; }
        public string? TargetAd { get; set; }
        public double? TotalAdSpend { get; set; }
        public double? AgencyFeeSpend { get; set; }
        public double? ThirdPartyFeeSpend { get; set; }
        public double? CalculatedTotalBudget { get; set; }

        public static BudgetResponse Success(double budget, 
                                            string targetAd, 
                                            double totalAdSpend,
                                            double agencyFeeSpend,
                                            double thirdPartyFeeSpend, 
                                            double calculatedTotalBudget)
        {
            return new BudgetResponse
            {
                IsSuccess = true,
                Budget = budget,
                TargetAd = targetAd,
                TotalAdSpend = totalAdSpend,
                AgencyFeeSpend = agencyFeeSpend,
                ThirdPartyFeeSpend = thirdPartyFeeSpend,
                CalculatedTotalBudget = calculatedTotalBudget,
                Message = "Budget calculated successfully."
            };
        }

        public static BudgetResponse Failure(string errorMessage)
        {
            return new BudgetResponse
            {
                IsSuccess = false,
                Message = errorMessage
            };
        }

        public override string ToString()
        {
            if (!IsSuccess)
            {
                return $"Failure: {Message}";
            }

            return $"Success: Budget for {TargetAd} = €{Budget:F2}\n" +
                $"Total Ad Spend: €{TotalAdSpend:F2}\n" +
                $"Agency Fee Spend: €{AgencyFeeSpend:F2}\n" +
                $"Third Party Fee Spend: €{ThirdPartyFeeSpend:F2}\n" +
                $"Calculated Total Budget: €{CalculatedTotalBudget:F2}";
        }
    }
}