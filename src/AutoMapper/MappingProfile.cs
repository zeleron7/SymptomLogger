using AutoMapper;
using SymptomLogger.src.DTO.Patient;
using SymptomLogger.src.DTO.Symptom;
using SymptomLogger.src.Entity;

namespace SymptomLogger.src.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Patient
            CreateMap<CreatePatientDTO, Patient>();
             
            // Symptom
            #region Symptom
            CreateMap<CreateSymptomDTO, Symptom>();
            CreateMap<UpdateSymptomDTO, Symptom>();
            CreateMap<Symptom, UpdateSymptomDTO>(); // for patching, to convert entity back to DTO

            CreateMap<Symptom, ResponseSymptomDTO>(); 
            #endregion 
        }
    }
}
