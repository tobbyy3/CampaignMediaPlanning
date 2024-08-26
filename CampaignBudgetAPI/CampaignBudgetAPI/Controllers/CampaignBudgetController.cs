using Microsoft.AspNetCore.Mvc;
using CampaignBudgetAPI.Models;
using CampaignBudgetAPI.Services;

namespace CampaignBudgetAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CampaignBudgetController : ControllerBase
    {
        private readonly BudgetCalculator _budgetCalculator;

        public CampaignBudgetController(BudgetCalculator budgetCalculator)
        {
            _budgetCalculator = budgetCalculator;
        }

        [HttpPost("calculate")]
        public IActionResult CalculateCampaignBudget(BudgetRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var response = _budgetCalculator.GoalSeek(request);
                return Ok(response);
            } 
            catch(ArgumentException ex)
            {
                return UnprocessableEntity(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
