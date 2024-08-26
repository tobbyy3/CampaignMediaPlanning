using CampaignBudgetAPI.Models;
using System;

namespace CampaignBudgetAPI.Services
{
    public class BudgetCalculator
    {
        private const int maxIterations = 1000;
        public BudgetResponse GoalSeek(BudgetRequest request)
        {
            double maxBudget = request.TotalBudget;
            double minBudget = 0;
            double targetBudget = maxBudget / 2;
            int iteration = 0;

            double checkAdSpend = request.AdBudgets.Where(ad => ad.AdName != request.TargetAd).Sum(ad => ad.Budget);

            if ((checkAdSpend) >= request.TotalBudget)
            {
                throw new ArgumentException("Sum of ad-budget exceeds the total budget.");
            } 

            var targetAdIndex = request.AdBudgets.FindIndex(ad => ad.AdName == request.TargetAd);

            if (targetAdIndex == -1)
            {
                throw new ArgumentException($"Target ad not found in the provided ad budgets.");
            }

            while ((maxBudget - minBudget) > request.tolerance && iteration <= maxIterations)
            {
                
                request.AdBudgets[targetAdIndex].Budget = targetBudget;

                double totalAdSpend = request.AdBudgets.Sum(ad => ad.Budget);
                
                double agencyFee = request.AgencyFee * totalAdSpend;
                
                double thirdPartyFee = request.AdBudgets
                                    .Where(ad => ad.UsesThirdPartyTool)
                                    .Sum(ad => ad.Budget) * request.ThirdPartyFee;

                double calculatedBudget = totalAdSpend 
                                        + agencyFee 
                                        + thirdPartyFee 
                                        + request.FixedCosts;

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
                    maxBudget = targetBudget;
                } 
                else
                {
                    minBudget = targetBudget;
                }

                iteration++;
                targetBudget = (maxBudget + minBudget) / 2;
            }

            return BudgetResponse.Failure("Max iterations reached without the value converging.");
        }
    }
}