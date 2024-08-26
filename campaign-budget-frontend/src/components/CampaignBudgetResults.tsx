import React from 'react';
import { Typography, Box, Paper, Table, TableBody, TableCell, TableContainer, TableRow } from '@mui/material';
import { CampaignBudgetResult } from '../types/adBudget';

interface CampaignBudgetResultsProps {
  result: CampaignBudgetResult | null;
}

const CampaignBudgetResults: React.FC<CampaignBudgetResultsProps> = ({ result }) => {
  if (!result) return null;

  return (
    <Box mt={4}>
      
      <TableContainer component={Paper}>
        <Table>
          <TableBody>
            <TableRow>
              <TableCell><strong>Status:</strong></TableCell>
              <TableCell style={{ color: result.isSuccess ? 'green' : 'red' }}>
                {result.isSuccess ? 'Success' : 'Failed'}
              </TableCell>
            </TableRow>
            <TableRow>
              <TableCell><strong>Message:</strong></TableCell>
              <TableCell>{result.message}</TableCell>
            </TableRow>
            <TableRow>
              <TableCell><strong>Target Ad Budget:</strong></TableCell>
              <TableCell>${result.budget?.toFixed(2)}</TableCell>
            </TableRow>
            <TableRow>
              <TableCell><strong>Target Ad:</strong></TableCell>
              <TableCell>{result.targetAd}</TableCell>
            </TableRow>
            <TableRow>
              <TableCell><strong>Total Ad Spend:</strong></TableCell>
              <TableCell>${result.totalAdSpend?.toFixed(2)}</TableCell>
            </TableRow>
            <TableRow>
              <TableCell><strong>Agency Fee Spend:</strong></TableCell>
              <TableCell>${result.agencyFeeSpend?.toFixed(2)}</TableCell>
            </TableRow>
            <TableRow>
              <TableCell><strong>Third Party Fee Spend:</strong></TableCell>
              <TableCell>${result.thirdPartyFeeSpend?.toFixed(2)}</TableCell>
            </TableRow>
            <TableRow>
              <TableCell><strong>Calculated Total Budget:</strong></TableCell>
              <TableCell>${result.calculatedTotalBudget?.toFixed(2)}</TableCell>
            </TableRow>
          </TableBody>
        </Table>
      </TableContainer>
    </Box>
  );
};

export default CampaignBudgetResults;
