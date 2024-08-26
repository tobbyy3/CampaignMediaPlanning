using CampaignBudgetAPI.Models;
using System;

namespace CampaignBudgetAPI.Services
{
    public class BudgetCalculator
    {
        // Maximum number of iterations for the goal-seeking algorithm
        private const int maxIterations = 1000;

        public BudgetResponse GoalSeek(BudgetRequest request)
        {
            double maxBudget = request.TotalBudget;
            double minBudget = 0;
            double targetBudget = maxBudget / 2;
            int iteration = 0;

            // Check if the sum of non-target ad budgets exceeds the total budget
            double checkAdSpend = request.AdBudgets.Where(ad => ad.AdName != request.TargetAd).Sum(ad => ad.Budget);

            if ((checkAdSpend) >= request.TotalBudget)
            {
                throw new ArgumentException("Sum of ad-budget exceeds the total budget.");
            } 

            // Find the index of the target ad in the AdBudgets list
            var targetAdIndex = request.AdBudgets.FindIndex(ad => ad.AdName == request.TargetAd);

            if (targetAdIndex == -1)
            {
                throw new ArgumentException($"Target ad not found in the provided ad budgets.");
            }

            // Goal-seeking algorithm: binary search to find the optimal budget
            while ((maxBudget - minBudget) > request.tolerance && iteration <= maxIterations)
            {
                // Set the budget for the target ad
                request.AdBudgets[targetAdIndex].Budget = targetBudget;

                // Calculate total ad spend
                double totalAdSpend = request.AdBudgets.Sum(ad => ad.Budget);
                
                // Calculate agency fee
                double agencyFee = request.AgencyFee * totalAdSpend;
                
                // Calculate third-party fee for ads using third-party tools
                double thirdPartyFee = request.AdBudgets
                                    .Where(ad => ad.UsesThirdPartyTool)
                                    .Sum(ad => ad.Budget) * request.ThirdPartyFee;

                // Calculate the total budget including all fees and fixed costs
                double calculatedBudget = totalAdSpend 
                                        + agencyFee 
                                        + thirdPartyFee 
                                        + request.FixedCosts;

                // Check if the calculated budget is within the tolerance of the target budget
                if (Math.Abs(calculatedBudget - request.TotalBudget) <= request.tolerance)
                {
                    return BudgetResponse.Success(
                                        budget: targetBudget,
                                        targetAd: request.TargetAd,
                                        totalAdSpend: totalAdSpend,
                                        agencyFeeSpend: agencyFee,
                                        thirdPartyFeeSpend: thirdPartyFee,
                                        calculatedTotalBudget: calculatedBudget);
                } 
                else if (calculatedBudget > request.TotalBudget)
                {
                    // If calculated budget is too high, adjust the upper bound
                    maxBudget = targetBudget;
                } 
                else
                {
                    // If calculated budget is too low, adjust the lower bound
                    minBudget = targetBudget;
                }

                iteration++;
                // Calculate new target budget as the midpoint between min and max
                targetBudget = (maxBudget + minBudget) / 2;
            }

            // If the algorithm doesn't converge within the maximum iterations, return a failure response
            return BudgetResponse.Failure("Max iterations reached without the value converging.");
        }
    }
}