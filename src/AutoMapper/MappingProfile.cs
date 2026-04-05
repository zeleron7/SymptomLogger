using AutoMapper;
using SymptomLogger.src.DTO.Patient;
using SymptomLogger.src.DTO.Symptom;
using SymptomLogger.src.Entity;
using SymptomLogger.src.DTO.SymptomLog;

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
            CreateMap<Symptom, SymptomLog>(); 
            #endregion

            // Symptom log
            #region Symptom log
            CreateMap<CreateSymptomLogDTO, SymptomLog>();
            CreateMap<UpdateSymptomLogDTO, SymptomLog>();
            CreateMap<SymptomLog, UpdateSymptomLogDTO>(); // for patching, to convert entity back to DTO

            CreateMap<SymptomLog, ResponseSymptomLogDTO>();
            CreateMap<SymptomLog, Symptom>();
            #endregion
        }
    }
}
