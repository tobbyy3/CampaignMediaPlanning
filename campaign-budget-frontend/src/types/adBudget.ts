export interface AdBudget {
  adName: string;
  budget: number;
  usesThirdPartyTool: boolean;
}

export interface CampaignBudgetFormData {
  totalBudget: number;
  agencyFee: number;
  thirdPartyFee: number;
  fixedCosts: number;
  targetAd: string;
  adBudgets: AdBudget[];
  tolerance: number;
}

export interface CampaignBudgetResult {
  isSuccess: boolean;
  message: string;
  budget?: number;
  targetAd?: string;
  totalAdSpend?: number;
  agencyFeeSpend?: number;
  thirdPartyFeeSpend?: number;
  calculatedTotalBudget?: number;
}
