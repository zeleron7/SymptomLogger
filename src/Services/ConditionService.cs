using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using SymptomLogger.src.DTO.Condition;
using SymptomLogger.src.DTO.Symptom;
using SymptomLogger.src.DTO.SymptomLog;
using SymptomLogger.src.Entity;
using SymptomLogger.src.Repositories;
using SymptomLogger.src.Repositories.Interfaces;
using SymptomLogger.src.Services.Interfaces;
using SymptomLogger.src.Utils;

namespace SymptomLogger.src.Services
{
    public class ConditionService : IConditionService
    {
        private readonly IConditionRepository _conditionRepository;
        private readonly ILogger<ConditionService> _logger;
        
        public ConditionService(IConditionRepository conditionRepository, ILogger<ConditionService> logger)
        {
            _conditionRepository = conditionRepository;
            _logger = logger;
        }

        // Get all Conditions
        #region GetConditions
        public IQueryable<ResponseConditionDTO> GetConditions()
        {
            return _conditionRepository.GetConditions()
                .Select(c => new ResponseConditionDTO // manual mapping 
                {
                    Name = c.Name,
                    System = c.System,
                    IcdCode = c.IcdCode // handle null value ? 
                });
        }
        #endregion

        // Get Condition by ID
        #region GetConditionByIdAsync
        public async Task<ResponseConditionDTO> GetConditionByIdAsync(int id)
        {
            var conditionById = await _conditionRepository.GetConditionById(id);

            // null check to prevent null reference exception
            if (conditionById == null)
            {
                _logger.LogWarning("Condition with ID {ConditionId} not found", id);
                throw new NotFoundException($"Condition with ID {id} not found.");
            }
            // map entity to response dto
            var responseDto = new ResponseConditionDTO
            {
                Name = conditionById.Name,
                System = conditionById.System,
                IcdCode = conditionById.IcdCode // handle null value ?
            };

            return responseDto;
        }
        #endregion

        // Add new Condition
        #region AddConditionAsync
        public async Task<ResponseConditionDTO> AddConditionAsync(CreateConditionDTO dto)
        {
            // null check to prevent null reference exception
            if (dto == null)
            {
                _logger.LogInformation("CreateConditionDTO is null.");
                throw new ArgumentNullException(nameof(dto), "Condition cannot be null.");
            }

            // map dto to entity
            var condition = new Condition
            {
                Name = dto.Name,
                System = dto.System,
                IcdCode = dto.IcdCode
            };

            var createdCondition = await _conditionRepository.AddCondition(condition);

            // map created condition to response dto
            var responseDto = new ResponseConditionDTO
            {
                IcdCode = condition.IcdCode,
                System = condition.System,
                Name = dto.Name,
            };

            return responseDto;
        }
        #endregion

        // Update existing Condition
        #region UpdateConditionAsync
        public async Task<ResponseConditionDTO> UpdateConditionAsync(int id, JsonPatchDocument<UpdateConditionDTO> patchDoc)
        {
            if (patchDoc == null)
            {
                _logger.LogWarning("Patch document is null for condition with ID {ConditionId}.", id);
                throw new ArgumentNullException("Patch document cannot be null.");
            }

            var conditionToUpdate = await _conditionRepository.GetConditionById(id);
            if (conditionToUpdate == null)
            {
                _logger.LogWarning("Condition with ID {ConditionId} not found for update", id);
                throw new NotFoundException($"Condition with ID {id} not found.");
            }

            // create a dto to apply the patch document to
            var updatedDto = new UpdateConditionDTO
            {
                Name = conditionToUpdate.Name,
                System = conditionToUpdate.System,
                IcdCode = conditionToUpdate.IcdCode // handle null value ?
            };

            patchDoc.ApplyTo(updatedDto);

            // map updated dto back to entity
            var updatedEntity = new Condition
            {
                Name = updatedDto.Name,
                System = updatedDto.System,
                IcdCode = updatedDto.IcdCode
            };
            var updatedCondition = await _conditionRepository.UpdateCondition(updatedEntity);

            // map updated entity to response dto
            var responseDto = new ResponseConditionDTO
            {
                Name = updatedCondition.Name,
                System = updatedCondition.System,
                IcdCode = updatedCondition.IcdCode // handle null value ?
            };

            return responseDto;
        }
        #endregion 

        // Delete symptom by ID
        #region DeleteSymptomAsync
        public async Task<bool> DeleteConditionAsync(int id)
        {
            var condition = await _conditionRepository.GetConditionById(id);
            
            // null check to prevent null reference exception
            if (condition == null)
            {
                return false;
            }
            return await _conditionRepository.DeleteCondition(id);
        }
        #endregion 
    }

}
