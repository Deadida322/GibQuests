using AuthService.DataContracts.User;
using AutoMapper;
using GenerateQuestsService.DataContracts.DataContracts;
using GenerateQuestsService.DataContracts.Enums;
using GenerateQuestsService.DataContracts.Models;
using GenerateQuestsService.DataContracts.Models.Stages;
using ProcessQuestDataContracts.DataContracts;
using ProcessQuestDataContracts.Enums;
using ProcessQuestDataContracts.Models;
using ProcessQuestDataContracts.Models.Stages;
using ProcessQuestDataContracts.ProcessModels;
using ProcessQuestDataContracts.ViewModels;
using ProcessQuestService.Core.HelperModels;
using ProcessQuestService.ProcessQuestDatabase.Models;

namespace ProcessQuestService.Core.MappingProfiles
{
    public class ProcessQuestMappingProfile : Profile
    {
        public ProcessQuestMappingProfile() {
            CreateMap<StartQuestContract, GetQuestContract>();

            CreateMap<Stage, StageProcess>()
                .Include<MapStage, MapStageProcess>()
                .Include<QrCodeStage, QrCodeStageProcess>()
                .Include<TestStage, TestStageProcess>()
                .Include<TextStage, TextStageProcess>()
                .Include<VideoStage, VideoStageProcess>()
                .Include<FinalStage, FinalStageProcess>();

            CreateMap<StageProcess, Stage>()
                .Include<MapStageProcess, MapStage>()
                .Include<QrCodeStageProcess, QrCodeStage>()
                .Include<TestStageProcess, TestStage>()
                .Include<TextStageProcess, TextStage>()
                .Include<VideoStageProcess, VideoStage>()
                .Include<FinalStageProcess, FinalStage>();

            CreateMap<Coordinates, CoordinatesProcess>();
            CreateMap<Question, QuestionProcess>();
            CreateMap<MapStage, MapStageProcess>();
            CreateMap<QrCodeStage, QrCodeStageProcess>();
            CreateMap<TestStage, TestStageProcess>();
            CreateMap<TextStage, TextStageProcess>();
            CreateMap<VideoStage, VideoStageProcess>();
            CreateMap<FinalStage, FinalStageProcess>();

            CreateMap<CoordinatesProcess, Coordinates>();
            CreateMap<QuestionProcess, Question>();
            CreateMap<MapStageProcess, MapStage>();
            CreateMap<QrCodeStageProcess, QrCodeStage>();
            CreateMap<TestStageProcess, TestStage>();
            CreateMap<TextStageProcess, TextStage>();
            CreateMap<VideoStageProcess, VideoStage>();
            CreateMap<FinalStageProcess, FinalStage>();

            CreateMap<QuestViewModel, QuestProcessViewModel>();
            CreateMap<QuestProcessViewModel, QuestViewModel>();


            //база данных
            CreateMap<RegisterRoomContract, RoomEntity>();
            CreateMap<RegisterRoomContract, ProcessQuestDataContracts.ProcessModels.ProcessModel>();

            //теория
            CreateMap<QuestViewModel, FullQuestViewModel>();
            CreateMap<QuestProcessViewModel, FullQuestViewModel>();
            CreateMap<FullQuestViewModel, QuestProcessViewModel>();
            CreateMap<QuestPolicy, QuestPolicyProcess>();
            CreateMap<PolicyType, PolicyProcessType>();
            CreateMap<MemberType, MemberProcessType>();


            //Auth
            CreateMap<UserViewModel, UserDataViewModel>();
            CreateMap<ShortUserViewModel, UserDataViewModel>();

            CreateMap<RegisterProcessUserModel, ConnectToRoomViewModel>();
            CreateMap<ConnectToRoomViewModel, RegisterProcessUserModel>();
        }
    }
}
