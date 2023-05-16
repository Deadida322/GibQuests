using AutoMapper;
using CommonInfrastructure.Extension;
using CommonInfrastructure.Http;
using CommonInfrastructure.Http.Helpers;
using GenerateQuestsService.DataContracts.DataContracts;
using GenerateQuestsService.DataContracts.Enums;
using GenerateQuestsService.DataContracts.Interfaces;
using GenerateQuestsService.DataContracts.Models.Stages;
using ProcessQuestDataContracts.DataContracts;
using ProcessQuestDataContracts.Models.Stages;
using ProcessQuestDataContracts.ViewModels;
using ProcessQuestService.Core.Helpers;
using System.Collections.Generic;
using System.Net;

namespace ProcessQuestService.Core.BusinessLogic
{
    public class ConnectQuestLogic
    {
        private IGenerateQuestsApi _generateQuestsApi;
        private IMapper _mapper;
        private Random _random;
        private ProcessQuestCacheHelper _cacheHelper;

        public ConnectQuestLogic(IGenerateQuestsApi generateQuestsApi,
            IMapper mapper,
            ProcessQuestCacheHelper cacheHelper)
        {
            _generateQuestsApi = generateQuestsApi;
            _mapper = mapper;
            _random = new Random();
            _cacheHelper = cacheHelper;
        }

        public async Task<CommonHttpResponse<IList<UserProcessingQuestViewModel>>> GetUserQuestsProcessingAsync(CommonHttpRequest contract)
        {
            try
            {
                IList<UserProcessingQuestViewModel> result = new List<UserProcessingQuestViewModel>();

                var userId = contract.RequestUserId.Value;
                var processModels = await _cacheHelper.GetAllUserQuestProcessModelsAsync(userId);

                foreach (var processModel in processModels)
                {
                    var questRes = await _generateQuestsApi.GetQuestAsync(
                        new GetQuestContract
                        {
                            Id = Int32.Parse(processModel.QuestId),
                            RequestId = contract.RequestId,
                            RequestUserId = contract.RequestUserId,
                            RequestUserName = contract.RequestUserName
                        }
                    );
                    if (!questRes.Success || questRes.Data == null)
                    {
                        return CommonHttpHelper.BuildErrorResponse <IList<UserProcessingQuestViewModel>> (
                            extErrors: questRes.Errors.ToList());
                    }
                    var quest = questRes.Data;

                    string roomKey = processModel.Key;
                    int userStagePrev = processModel.UserProcessing[userId];
                    var currentStage = quest.Stages[userStagePrev];
                    result.Add(new UserProcessingQuestViewModel
                    {
                        QuestId = quest.Id,
                        QuestName = quest.Title,
                        Room = processModel.Key,
                        Stage = _mapper.Map<StageProcess>(currentStage)

                    });
                }
                return CommonHttpHelper.BuildSuccessResponse(result, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return CommonHttpHelper.BuildErrorResponse<IList<UserProcessingQuestViewModel>> (
                   HttpStatusCode.InternalServerError,
                   ex.ToExceptionDetails(),
                   $"Ошибка выполнения метода {nameof(GetUserQuestsProcessingAsync)} ReqId : {contract.RequestId}");
            }
        }

        public async Task<CommonHttpResponse<StartQuestViewModel>> ConnectToQuestAsync(StartQuestContract contract)
        {
            try
            {
                var questRes = await _generateQuestsApi.GetQuestAsync(
                    _mapper.Map<GetQuestContract>(contract)
                );
                if (!questRes.Success || questRes.Data == null)
                {
                    return CommonHttpHelper.BuildErrorResponse<StartQuestViewModel>(
                        extErrors: questRes.Errors.ToList());
                }
                var quest = questRes.Data;

                //проверяем, есть ли текущие прохождения у юзера
                var existProcessing = await _cacheHelper.GetUserQuestProcessModelAsync(quest.Id.ToString(), contract.RequestUserId.Value);
                if(existProcessing != null)
                {
                    var result = new StartQuestViewModel()
                    {
                        Room = existProcessing.Key,
                        Quest = _mapper.Map<QuestProcessViewModel>(quest)
                    };

                    return CommonHttpHelper.BuildSuccessResponse(result, HttpStatusCode.OK);
                }
                //если политика квеста публичная
                if (quest.Policy.PolicyType == PolicyType.Public)
                {
                    //генерируем номер комнаты
                    string roomKey = await GenerateNumberRoom();

                    if(roomKey.IsNullOrEmpty()) {
                        return CommonHttpHelper.BuildErrorResponse<StartQuestViewModel>(
                        initialError: "Не удалось создать комнату прохождения");
                    }

                    //устанавливаем квест в кеш на время прохождения квеста
                    await _cacheHelper.SetQuestAsync(quest);
                    
                    //регистрируем прохождение
                    if(contract.RequestUserId.HasValue)
                    {
                        await _cacheHelper.RegisterProcessingAsync(
                                roomKey, 
                                contract.RequestUserId.Value, 
                                quest.Id.ToString()
                            );
                    }
                    else
                    {
                        return CommonHttpHelper.BuildErrorResponse<StartQuestViewModel>(
                            initialError: "Нет пользователя в запросе", statusCode: HttpStatusCode.Unauthorized);
                    }

                    var result = new StartQuestViewModel()
                    {
                        Room = roomKey,
                        Quest = _mapper.Map<QuestProcessViewModel>(quest)
                    };

                    return CommonHttpHelper.BuildSuccessResponse(result, HttpStatusCode.OK);
                }
                //если политика квеста закрытая - то надо позволить проходить
                //только самому владельцу квеста
                else
                {
                    return CommonHttpHelper.BuildErrorResponse<StartQuestViewModel>(
                       initialError: "Такой тип квеста еще в разработке (можно только публичный)");
                }
            }
            catch (Exception ex)
            {
                return CommonHttpHelper.BuildErrorResponse<StartQuestViewModel>(
                   HttpStatusCode.InternalServerError,
                   ex.ToExceptionDetails(),
                   $"Ошибка выполнения метода {nameof(ConnectToQuestAsync)} ReqId : {contract.RequestId}");

            }
        }

        private async Task<string> GenerateNumberRoom()
        {
            int attempt = 0;
            while (attempt < 10)
            {
                string value = _random.Next(int.MaxValue).ToString();

                if (await _cacheHelper.IsSetRoomKeyAsync(value))
                {
                    return value;
                }
                attempt++;
            }
            return null;
        }
    }
}
