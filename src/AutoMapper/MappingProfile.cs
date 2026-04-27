//using AutoMapper;
using SymptomLogger.src.DTO.User;
using SymptomLogger.src.DTO.Symptom;
using SymptomLogger.src.Entity;
using SymptomLogger.src.DTO.SymptomLog;
using SymptomLogger.src.DTO.Condition;

namespace SymptomLogger.src.AutoMapper
{
    /*public class MappingProfile : Profile
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

            // Condition
            #region Condition
            CreateMap<CreateConditionDTO, Condition>();
            CreateMap<UpdateConditionDTO, Condition>();
            CreateMap<Condition, UpdateConditionDTO>(); // for patching, to convert entity back to DTO

            CreateMap<Condition, ResponseConditionDTO>();
            #endregion 
        }
    }*/
}
