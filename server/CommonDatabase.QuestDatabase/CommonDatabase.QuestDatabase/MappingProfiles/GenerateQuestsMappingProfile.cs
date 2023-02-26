using AutoMapper;
using CommonDatabase.QuestDatabase.Models;
using CommonDatabase.QuestDatabase.Models.Stages;
using GenerateQuestsService.DataContracts.DataContracts;
using GenerateQuestsService.DataContracts.Models;
using GenerateQuestsService.DataContracts.Models.Stages;

namespace CommonDatabase.QuestDatabase.MappingProfiles
{
    public class GenerateQuestsMappingProfile : Profile
    {
        public GenerateQuestsMappingProfile()
        {
            //----------------------------- Entity

           
            CreateMap<Stage, StageEntity>()
                .Include<MapStage, MapStageEntity>()
                .Include<QrCodeStage, QrCodeStageEntity>()
                .Include<TestStage, TestStageEntity>()
                .Include<TextStage, TextStageEntity>()
                .Include<VideoStage, VideoStageEntity>();

            CreateMap<StageEntity, Stage>()
                 .Include<MapStageEntity, MapStage>()
                .Include<QrCodeStageEntity, QrCodeStage>()
                .Include<TestStageEntity, TestStage>()
                .Include<TextStageEntity, TextStage>()
                .Include<VideoStageEntity, VideoStage>();

            CreateMap<Coordinates, CoordinatesEntity>();
            CreateMap<Question, QuestionEntity>();
            CreateMap<MapStage, MapStageEntity>();
            CreateMap<QrCodeStage, QrCodeStageEntity>();
            CreateMap<TestStage, TestStageEntity>();
            CreateMap<TextStage, TextStageEntity>();
            CreateMap<VideoStage, VideoStageEntity>();

            CreateMap<CoordinatesEntity, Coordinates>();
            CreateMap<QuestionEntity, Question>();
            CreateMap<MapStageEntity, MapStage>();
            CreateMap<QrCodeStageEntity, QrCodeStage>();
            CreateMap<TestStageEntity, TestStage>();
            CreateMap<TextStageEntity, TextStage>();
            CreateMap<VideoStageEntity, VideoStage>();

            CreateMap<CreateQuestContract, QuestEntity>()
                .ForMember(dest => dest.Stages, 
                    opt => opt.MapFrom(x => x.Stages))
                .ForMember(dest => dest.UserId,
                    opt => opt.MapFrom(x => x.RequestUserId));
            CreateMap<UpdateQuestContract, QuestEntity>()
                .ForMember(dest => dest.Stages,
                    opt => opt.MapFrom(x => x.Stages))
                .ForMember(dest => dest.UserId,
                    opt => opt.MapFrom(x => x.RequestUserId));
            CreateMap<QuestEntity, QuestViewModel>()
                .ForMember(dest => dest.Stages,
                    opt => opt.MapFrom(x => x.Stages));
            CreateMap<QuestEntity, ShortQuestViewModel>();
        }
    }
}
