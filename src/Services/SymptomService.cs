using AutoMapper;
using Azure;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using SymptomLogger.src.Data;
using SymptomLogger.src.DTO.Symptom;
using SymptomLogger.src.Entity;
using SymptomLogger.src.Repositories.Interfaces;
using System.Net;
using System.Security.Claims;
using AutoMapper.QueryableExtensions;
using SymptomLogger.src.Services.Interfaces;
using SymptomLogger.src.Utils;

namespace SymptomLogger.src.Services
{
    public class SymptomService : ISymptomService
    {
        private readonly ISymptomRepository _symptomRepository;
        private readonly ILogger<SymptomService> _logger;

        public SymptomService(ISymptomRepository symptomRepository, ILogger<SymptomService> logger)
        {
            _symptomRepository = symptomRepository;
            _logger = logger;
        }

        // Get all symptoms
        #region GetSymptoms
        public IQueryable<ResponseSymptomDTO> GetSymptoms()
        {
            return _symptomRepository.GetSymptoms()
                .Select(c => new ResponseSymptomDTO
                {
                    SymptomId = c.SymptomId,
                    Category = c.Category,
                    Description = c.Description ?? string.Empty,
                    Name = c.Name,
                });
        }
        #endregion

        // Get symptom by ID
        #region GetSymptomByIdAsync
        public async Task<ResponseSymptomDTO> GetSymptomByIdAsync(int id, string userId)
        {
            var symptomById = await _symptomRepository.GetSymptomById(id);

            if (symptomById == null)
            {
                _logger.LogWarning("Symptom with ID {SymptomId} not found.", id);
                throw new NotFoundException($"Symptom with ID {id} not found.");
            }

            // validate user ID to ensure the symptom being accessed belongs to the authenticated user
            if (symptomById.UserId != userId)
            {
                _logger.LogWarning("User tried to retrieve symptom ID {SymptomId}", id);
                throw new UnauthorizedAccessException();
            }

            // map the symptom entity to the response DTO
            var responseDto = new ResponseSymptomDTO
            {
                Category = symptomById.Category,
                Description = symptomById.Description ?? string.Empty,
                Name = symptomById.Name,
            };

            return responseDto;
        }
        #endregion 

        // Add new symptom
        #region AddSymptomAsync
        public async Task<ResponseSymptomDTO> AddSymptomAsync(CreateSymptomDTO dto, string userId)
        {
            if (dto == null)
            {
                _logger.LogInformation("CreateSymptomDTO is null.");
                throw new ArgumentNullException(nameof(dto), "Symptom cannot be null.");
            }

            // map DTO to entity
            var symptom = new Symptom
            {
                Name = dto.Name,
                Description = dto.Description,
                Category = dto.Category,
                BodySystem = dto.BodySystem,
            };

            // set the userId passed from the controller to ensure the symptom is associated with the correct user
            symptom.UserId = userId;

            var CreatedSymptom = await _symptomRepository.AddSymptom(symptom, userId);

            // map the created symptom entity to the response DTO
            var responseDto = new ResponseSymptomDTO
            {
                Name = CreatedSymptom.Name,
                Description = CreatedSymptom.Description ?? string.Empty,
                Category = CreatedSymptom.Category,
                BodySystem = CreatedSymptom.BodySystem
            };

            return responseDto;
        }
        #endregion

        // Update symptom
        #region UpdateSymptomAsync
        public async Task<ResponseSymptomDTO> UpdateSymptomAsync(int id, JsonPatchDocument<UpdateSymptomDTO> patchDoc, string userId)
        {
            if (patchDoc == null)
            {
                _logger.LogInformation("JsonPatchDocument is null.");
                throw new ArgumentNullException(nameof(patchDoc), "Patch document cannot be null.");
            }

            var symptomToUpdate = await _symptomRepository.GetSymptomById(id);

            if (symptomToUpdate.UserId != userId)
            {
                _logger.LogWarning("Unknown user tried to update symptom with ID {SymptomId}", id);
                throw new UnauthorizedAccessException();
            }

            if (symptomToUpdate == null)
            {
                _logger.LogInformation("Symptom with ID {SymptomId} not found for update.", id);
                throw new KeyNotFoundException($"Symptom with ID {id} not found.");
            }

            // create a DTO to apply the patch document to
            #region ApplyPatch
            var updatedDto = new UpdateSymptomDTO
            {
                Name = symptomToUpdate.Name,
                Description = symptomToUpdate.Description ?? string.Empty,
                Category = symptomToUpdate.Category,
                BodySystem = symptomToUpdate.BodySystem ?? string.Empty,
            };
            #endregion 

            patchDoc.ApplyTo(updatedDto);

            // Copy the updated values from the DTO back to the symptom entity
            #region copyValues
            symptomToUpdate.Name = updatedDto.Name;
            symptomToUpdate.Category = updatedDto.Category;
            symptomToUpdate.BodySystem = updatedDto.BodySystem;
            symptomToUpdate.Description = updatedDto.Description;
            #endregion 

            symptomToUpdate.UserId = userId; // ensure the userId is set to the authenticated user's ID to prevent unauthorized updates

            var UpdatedSymptom = await _symptomRepository.UpdateSymptom(symptomToUpdate, userId);
            //return _mapper.Map<ResponseSymptomDTO>(UpdatedSymptom);
            var responseDto = new ResponseSymptomDTO
            {
                Name = UpdatedSymptom.Name,
                Description = UpdatedSymptom.Description ?? string.Empty,
                Category = UpdatedSymptom.Category,
            };

            return responseDto;
        }
        #endregion

        // Delete symptom by ID
        #region DeleteSymptomAsync
        public async Task<bool> DeleteSymptomAsync(int id, string userId)
        {
            var symptom = await _symptomRepository.GetSymptomById(id);
            if (symptom.UserId != userId)
            {
                _logger.LogWarning("Unknown user tried to delete symptom with ID {SymptomId}", id);
                throw new UnauthorizedAccessException();
            }

            // null check to prevent null reference exception
            if (symptom == null) 
            {
                return false;
            }
            symptom.UserId = userId;
            
            return await _symptomRepository.DeleteSymptom(id);
        }
        #endregion 

    }
}
