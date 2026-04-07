using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using SymptomLogger.src.DTO.Condition;
using SymptomLogger.src.DTO.Symptom;
using SymptomLogger.src.Services;
using SymptomLogger.src.Services.Interfaces;
using SymptomLogger.src.Utils;

namespace SymptomLogger.src.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ConditionsController : ControllerBase
    {
        private readonly IConditionService _conditionService;
        private readonly ILogger<ConditionsController> _logger;
        
        public ConditionsController(IConditionService conditionService, ILogger<ConditionsController> logger)
        {
            _conditionService = conditionService;
            _logger = logger;
        }

        // GET: api/Conditions
        #region Get all conditions
        //[Authorize] Should be authorized for Admin only, but for testing purposes, we will leave it open for now.
        [HttpGet]
        public async Task<ActionResult<IQueryable<ResponseConditionDTO>>> GetAllConditions([FromQuery] PageParameters pageParameters)
        {
            _logger.LogInformation("Retrieving all conditions.");
            var conditions = _conditionService.GetConditionsAsync();

            // Implement pagination using the PagedList utility class
            var pagedConditions = await PagedList<ResponseConditionDTO>.CreateAsync(conditions, pageParameters.PageNumber, pageParameters.PageSize);

            if (!conditions.Any())
            {
                _logger.LogWarning("No conditions found.");
                return NotFound();
            }
            _logger.LogInformation("conditions retrieved successfully. Count: {ConditionCount}.", conditions.Count());

            return Ok(pagedConditions);

        }
        #endregion

        // GET: api/Conditions/5
        #region Get condition by ID
        //[Authorize] Should be authorized for Admin only, but for testing purposes, we will leave it open for now.
        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseConditionDTO>> GetCondition(int id)
        {
            _logger.LogInformation("Retrieving condition with ID {ConditionId}.", id);
            //var patientId = RetrievePatientId.GetPatientId(User);

            var condition = await _conditionService.GetConditionByIdAsync(id);
            if (condition == null)
            {
                _logger.LogWarning("Condition with ID {ConditionId} not found.", id);
                return NotFound();
            }
            _logger.LogInformation("Condition with ID {ConditionId} retrieved successfully.", id);
            return Ok(condition);
        }
        #endregion

        // Patch: api/Conditions/5 
        // JSON format [{"op": "replace", "path": "/Name", "value": "Updated name"}]
        #region Update condition
        //[Authorize] Should be authorized for Admin only, but for testing purposes, we will leave it open for now.
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateSymptom(int id, [FromBody] JsonPatchDocument<UpdateConditionDTO> patchDoc)
        {
            _logger.LogInformation("Updating condition with ID {ConditionId}.", id);

            if (patchDoc == null)
            {
                _logger.LogWarning("Patch document is null for condition with ID {ConditionId}.", id);
                return BadRequest("Patch document cannot be null.");
            }

            // Retrieve patient ID from the authenticated user's claims and pass it to the service layer
            //var patientId = RetrievePatientId.GetPatientId(User);

            var updatedCondition = await _conditionService.UpdateConditionAsync(id, patchDoc);
            _logger.LogInformation("Symptom with ID {SymptomId} updated successfully.", id);

            return Ok(updatedCondition);

        }
        #endregion

        // POST: api/Conditions
        #region Create new condition
        //[Authorize] Should be authorized for Admin only, but for testing purposes, we will leave it open for now.
        [HttpPost]
        public async Task<ActionResult<ResponseConditionDTO>> AddCondition(CreateConditionDTO dto)
        {
            _logger.LogInformation("Creating new condition");

            //var patientId = RetrievePatientId.GetPatientId(User); 
            var createdCondition = await _conditionService.AddConditionAsync(dto);
            _logger.LogInformation("Condition created successfully with Name {Name}.", createdCondition.Name);

            return Ok(createdCondition);
        }
        #endregion

        // DELETE: api/Symptoms/5
        #region Delete condition
        [HttpDelete("{id}")]
        //[Authorize] Should be authorized for Admin only, but for testing purposes, we will leave it open for now.
        public async Task<IActionResult> DeleteCondition(int id)
        {
            _logger.LogInformation("Deleting condition with ID {ConditionId}.", id);

            //var patientId = RetrievePatientId.GetPatientId(User);
            var success = await _conditionService.DeleteConditionAsync(id);

            if (!success)
            {
                _logger.LogWarning("Condition with ID {ConditionId} not found for deletion.", id);
                return NotFound();
            }
            _logger.LogInformation("Condition with ID {ConditionId} deleted successfully.", id);
            return Ok(new { message = "Condition deleted successfully" });
        }
        #endregion 
    }
}
