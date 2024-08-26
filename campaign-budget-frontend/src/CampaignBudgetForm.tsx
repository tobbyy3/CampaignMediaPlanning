import React, { useState } from 'react';
import { Container, Typography, Grid } from '@mui/material';
import apiClient from './api/apiClient';
import axios from 'axios';
import { CampaignBudgetFormData, CampaignBudgetResult, AdBudget } from './types/adBudget';
import CampaignBudgetForm from './components/CampaignBudgetForm';
import CampaignBudgetResults from './components/CampaignBudgetResults';

const CampaignBudgetCalculator: React.FC = () => {
  const [formData, setFormData] = useState<CampaignBudgetFormData>({
    totalBudget: 0,
    agencyFee: 0,
    thirdPartyFee: 0,
    fixedCosts: 0,
    targetAd: '',
    adBudgets: [],
    tolerance: 0
  });
  const [result, setResult] = useState<CampaignBudgetResult | null>(null);
  const [isLoading, setIsLoading] = useState(false);

  // Handle form submission
  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setIsLoading(true);
    try {
      const response = await apiClient.post('/CampaignBudget/calculate', formData);
      setResult(response.data);
    } catch (error) {
      // Handle different types of errors
      if (axios.isAxiosError(error) && error.response) {
        const { status, data } = error.response;
        if (status === 422) {
          console.log(data);
          setResult({ isSuccess: false, message: data || 'Validation error occurred' });
        } else if (status === 400) {
          const errorMessage = Object.values(data.errors).flat().join(', ');
          setResult({ isSuccess: false, message: errorMessage || 'Bad request error occurred' });
        } else {
          setResult({ isSuccess: false, message: 'An unexpected error occurred' });
        }
      } else {
        setResult({ isSuccess: false, message: 'An unexpected error occurred' });
      }
    } finally {
      setIsLoading(false);
    }
  };

  // Add a new ad budget to the form
  const handleAddAdBudget = () => {
    setFormData(prevData => ({
      ...prevData,
      adBudgets: [...prevData.adBudgets, { adName: '', budget: 0, usesThirdPartyTool: false }]
    }));
  };

  // Remove an ad budget from the form
  const handleRemoveAdBudget = (index: number) => {
    setFormData(prevData => ({
      ...prevData,
      adBudgets: prevData.adBudgets.filter((_, i) => i !== index)
    }));
  };

  return (
    <Container maxWidth="lg">
      <Grid container spacing={4}>
        <Grid item xs={12} md={6}>
          <Typography variant="h4" component="h1" gutterBottom>
            Campaign Budget Calculator
          </Typography>
          <CampaignBudgetForm
            formData={formData}
            setFormData={setFormData}
            handleSubmit={handleSubmit}
            handleAddAdBudget={handleAddAdBudget}
            handleRemoveAdBudget={handleRemoveAdBudget}
            isLoading={isLoading}
          />
        </Grid>
        <Grid item xs={12} md={6}>
          <Typography variant="h4" component="h1" gutterBottom>
            Results
          </Typography>
          <CampaignBudgetResults result={result} />
        </Grid>
      </Grid>
    </Container>
  );
};

export default CampaignBudgetCalculator;