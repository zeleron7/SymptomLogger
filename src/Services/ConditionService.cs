using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.JsonPatch;
using SymptomLogger.src.DTO.Condition;
using SymptomLogger.src.DTO.Symptom;
using SymptomLogger.src.DTO.SymptomLog;
using SymptomLogger.src.Entity;
using SymptomLogger.src.Repositories;
using SymptomLogger.src.Repositories.Interfaces;
using SymptomLogger.src.Services.Interfaces;

namespace SymptomLogger.src.Services
{
    public class ConditionService : IConditionService
    {
        private readonly IConditionRepository _conditionRepository;
        private readonly ILogger<ConditionService> _logger;
        private readonly IMapper _mapper;

        public ConditionService(IConditionRepository conditionRepository, ILogger<ConditionService> logger, IMapper mapper)
        {
            _conditionRepository = conditionRepository;
            _logger = logger;
            _mapper = mapper;
        }

        // Get all Conditions
        #region GetConditions
        public IQueryable<ResponseConditionDTO> GetConditionsAsync()
        {
            return _conditionRepository.GetConditions()
                .ProjectTo<ResponseConditionDTO>(_mapper.ConfigurationProvider);
        }
        #endregion

        // Get Condition by ID
        #region GetConditionByIdAsync
        public async Task<ResponseConditionDTO> GetConditionByIdAsync(int id)
        {
            var conditionById = await _conditionRepository.GetConditionById(id);
            if (conditionById == null)
            {
                _logger.LogWarning("Condition with ID {ConditionId} not found", id);
                return null;
            }
            return _mapper.Map<ResponseConditionDTO>(conditionById);
        }
        #endregion

        // Add new Condition
        #region AddConditionAsync
        public async Task<ResponseConditionDTO> AddConditionAsync(CreateConditionDTO dto)
        {
            try
            {
                if (dto == null)
                {
                    _logger.LogInformation("CreateConditionDTO is null.");
                    throw new ArgumentNullException(nameof(dto), "Condition cannot be null.");
                }
                // Map dto to entity
                var condition = _mapper.Map<Condition>(dto);
                
                var createdCondition = await _conditionRepository.AddCondition(condition);
                return _mapper.Map<ResponseConditionDTO>(condition);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a new condition. DTO: {@CreateConditionDTO}", dto);
                throw new Exception("An error occurred while adding the condition.", ex);
            }
        }
        #endregion

        // Update existing Condition
        #region UpdateConditionAsync
        public async Task<ResponseConditionDTO> UpdateConditionAsync(int id, JsonPatchDocument<UpdateConditionDTO> patchDoc)
        {
            var conditionToUpdate = await _conditionRepository.GetConditionById(id);
            if (conditionToUpdate == null)
            {
                _logger.LogWarning("Condition with ID {ConditionId} not found for update", id);
                throw new KeyNotFoundException($"Condition with ID {id} not found.");
            }
            var dto = _mapper.Map<UpdateConditionDTO>(conditionToUpdate);

            patchDoc.ApplyTo(dto);
            _mapper.Map(dto, conditionToUpdate);

            var updatedCondition = await _conditionRepository.UpdateCondition(conditionToUpdate);
            return _mapper.Map<ResponseConditionDTO>(updatedCondition);
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
