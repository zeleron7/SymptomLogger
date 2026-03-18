using AutoMapper;
using SymptomLogger.src.DTO;
using SymptomLogger.src.Models;

namespace SymptomLogger.src.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // input > entity
            CreateMap<CreateSymptomDTO, Symptom>();
            CreateMap<UpdateSymptomDTO, Symptom>();

            // entity > response
            CreateMap<Symptom, ResponseSymptomDTO>();

        }
    }
}
