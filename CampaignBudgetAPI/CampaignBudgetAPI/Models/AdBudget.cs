namespace CampaignBudgetAPI.Models
{
    public class AdBudget
    {
        public required string AdName {get; set;}
        public double Budget {get; set;}
        public bool UsesThirdPartyTool {get; set;}
    }
}