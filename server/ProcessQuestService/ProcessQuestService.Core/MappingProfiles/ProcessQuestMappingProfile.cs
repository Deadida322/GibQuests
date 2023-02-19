using AutoMapper;
using GenerateQuestsService.DataContracts.DataContracts;
using ProcessQuestDataContracts.DataContracts;

namespace ProcessQuestService.Core.MappingProfiles
{
    public class ProcessQuestMappingProfile : Profile
    {
        public ProcessQuestMappingProfile() {
            CreateMap<StartQuestContract, GetQuestContract>();
        }
    }
}
