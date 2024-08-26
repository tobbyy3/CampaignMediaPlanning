using System.Collections.Generic;

namespace CampaignBudgetAPI.Models
{
    public class BudgetRequest
    {
        public double TotalBudget {get; set;}
        public float AgencyFee {get; set;}
        public float ThirdPartyFee {get; set;}
        public double FixedCosts {get; set;}
        public required string TargetAd {get; set;}
        public required List<AdBudget> AdBudgets {get; set;}
        public float tolerance {get; set;}
    }
}