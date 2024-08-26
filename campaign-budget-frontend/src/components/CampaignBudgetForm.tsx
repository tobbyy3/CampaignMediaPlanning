import React from 'react';
import { Button, TextField, Typography, Box, FormControlLabel, Checkbox, Grid } from '@mui/material';
import { AdBudget, CampaignBudgetFormData } from '../types/adBudget';

interface CampaignBudgetFormProps {
  formData: CampaignBudgetFormData;
  setFormData: React.Dispatch<React.SetStateAction<CampaignBudgetFormData>>;
  handleSubmit: (event: React.FormEvent<HTMLFormElement>) => void;
  handleAddAdBudget: () => void;
  handleRemoveAdBudget: (index: number) => void;
  isLoading: boolean;
}

const CampaignBudgetForm: React.FC<CampaignBudgetFormProps> = ({
  formData,
  setFormData,
  handleSubmit,
  handleAddAdBudget,
  handleRemoveAdBudget,
  isLoading,
}) => {
  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value, type, checked } = e.target;
    setFormData(prevData => ({
      ...prevData,
      [name]: type === 'checkbox' ? checked : value,
    }));
  };

  const handleAdBudgetChange = (index: number, field: keyof AdBudget, value: string | boolean) => {
    setFormData(prevData => ({
      ...prevData,
      adBudgets: prevData.adBudgets.map((ad, i) =>
        i === index ? { ...ad, [field]: value } : ad
      ),
    }));
  };

  return (
    <form onSubmit={handleSubmit}>
      <Grid container spacing={2}>
        <Grid item xs={12}>
          <TextField
            fullWidth
            label="Total Budget"
            name="totalBudget"
            type="number"
            value={formData.totalBudget}
            onChange={handleInputChange}
            required
            InputProps={{ inputProps: { min: 0, step: "any" }, disableUnderline: true }}
            sx={{
              '& input[type=number]': {
                MozAppearance: 'textfield',
              }
            }}
          />
        </Grid>
        <Grid item xs={12}>
          <TextField
            fullWidth
            label="Agency Fee (%)"
            name="agencyFee"
            type="number"
            value={formData.agencyFee}
            onChange={handleInputChange}
            required
            InputProps={{
              inputProps: { min: 0, step: "any" },
              disableUnderline: true,
            }}
            sx={{
              '& input[type=number]': {
                MozAppearance: 'textfield',
              }
            }}
          />
        </Grid>
        <Grid item xs={12}>
          <TextField
            fullWidth
            label="Third Party Fee (%)"
            name="thirdPartyFee"
            type="number"
            value={formData.thirdPartyFee}
            onChange={handleInputChange}
            required
            InputProps={{ inputProps: { min: 0, step: "any" }, disableUnderline: true }}
            sx={{
              '& input[type=number]': {
                MozAppearance: 'textfield',
              }
            }}
          />
        </Grid>
        <Grid item xs={12}>
          <TextField
            fullWidth
            label="Fixed Costs"
            name="fixedCosts"
            type="number"
            value={formData.fixedCosts}
            onChange={handleInputChange}
            required
            InputProps={{ inputProps: { min: 0, step: "any" }, disableUnderline: true }}
            sx={{
              '& input[type=number]': {
                MozAppearance: 'textfield',
              }
            }}
          />
        </Grid>
        <Grid item xs={12}>
          <TextField
            fullWidth
            label="Target Ad"
            name="targetAd"
            value={formData.targetAd}
            onChange={handleInputChange}
            required
          />
        </Grid>
        <Grid item xs={12}>
          <TextField
            fullWidth
            label="Tolerance"
            name="tolerance"
            type="number"
            value={formData.tolerance}
            onChange={handleInputChange}
            required
            InputProps={{ inputProps: { min: 0, step: "any" }, disableUnderline: true }}
            sx={{
              '& input[type=number]': {
                MozAppearance: 'textfield',
              }
            }}
          />
        </Grid>
        <Grid item xs={12}>
          <Typography variant="h6" gutterBottom>Ad Budgets</Typography>
          {formData.adBudgets.map((adBudget, index) => (
            <Box key={index} mt={2} mb={2} display="flex" alignItems="center">
              <TextField
                label="Ad Name"
                value={adBudget.adName}
                onChange={(e) => handleAdBudgetChange(index, 'adName', e.target.value)}
                required
                style={{ marginRight: '10px' }}
              />
              <TextField
                label="Budget"
                type="number"
                value={adBudget.budget}
                onChange={(e) => handleAdBudgetChange(index, 'budget', e.target.value)}
                required
                style={{ marginRight: '10px' }}
                InputProps={{ inputProps: { min: 0, step: "any" }, disableUnderline: true }}
                sx={{
                  '& input[type=number]': {
                    MozAppearance: 'textfield',
                  }
                }}
              />
              <FormControlLabel
                control={
                  <Checkbox
                    checked={adBudget.usesThirdPartyTool}
                    onChange={(e) => handleAdBudgetChange(index, 'usesThirdPartyTool', e.target.checked)}
                  />
                }
                label="Third Party Tool"
                style={{ marginRight: '10px' }}
              />
              <Button onClick={() => handleRemoveAdBudget(index)} variant="outlined" color="secondary">
                Remove
              </Button>
            </Box>
          ))}
          <Button onClick={handleAddAdBudget} variant="outlined" color="primary">
            Add Ad Budget
          </Button>
        </Grid>
        <Grid item xs={12}>
          <Button type="submit" variant="contained" color="primary" disabled={isLoading}>
            {isLoading ? 'Calculating...' : 'Calculate Budget'}
          </Button>
        </Grid>
      </Grid>
    </form>
  );
};

export default CampaignBudgetForm;