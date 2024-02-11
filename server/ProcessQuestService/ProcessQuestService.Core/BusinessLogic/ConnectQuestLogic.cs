using AutoMapper;
using CommonInfrastructure.Extension;
using CommonInfrastructure.Http;
using CommonInfrastructure.Http.Helpers;
using GenerateQuestsService.DataContracts.DataContracts;
using GenerateQuestsService.DataContracts.Enums;
using GenerateQuestsService.DataContracts.Interfaces;
using Microsoft.AspNetCore.Http;
using ProcessQuestDataContracts.DataContracts;
using ProcessQuestDataContracts.Models.Stages;
using ProcessQuestDataContracts.ViewModels;
using ProcessQuestService.ProcessQuestDatabase.Interfaces;
using System.Net;

namespace ProcessQuestService.Core.BusinessLogic
{
    public class ConnectQuestLogic
    {
        private IGenerateQuestsApi _generateQuestsApi;
        private IMapper _mapper;
        private IProcessQuestStorage _processQuestStorage;
        private IProcessCacheStorage _cacheStorage;

        public ConnectQuestLogic(IGenerateQuestsApi generateQuestsApi,
            IMapper mapper, IProcessQuestStorage processQuestStorage, IProcessCacheStorage processCacheStorage)
        {
            _generateQuestsApi = generateQuestsApi;
            _mapper = mapper;
            _processQuestStorage = processQuestStorage;
            _cacheStorage = processCacheStorage;
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
                        return CommonHttpHelper.BuildErrorResponse<IList<UserProcessingQuestViewModel>>(
                            extErrors: questRes.Errors.ToList());
                    }
                    var quest = questRes.Data;

                    string roomKey = processModel.Key;
                    int userStagePrev = processModel.UserProcessing[userId].Stage;
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
                return CommonHttpHelper.BuildErrorResponse<IList<UserProcessingQuestViewModel>>(
                   HttpStatusCode.InternalServerError,
                   ex.ToExceptionDetails(),
                   $"Ошибка выполнения метода {nameof(GetUserQuestsProcessingAsync)} ReqId : {contract.RequestId}");
            }
        }

        public async Task<CommonHttpResponse<RegisterRoomViewModel>> RegisterRoomAsync(RegisterRoomContract contract)
        {
            try
            {
                //получаем квест из кеша
                var quest = await _cacheStorage.GetQuestAsync(new GetProcessQuestContract
                {
                    Id = contract.QuestId,
                });
                //если его нет, то получаем его и устанавливаем в кеш
                if (quest == null)
                {
                    var questRes = await _generateQuestsApi.GetQuestAsync(
                        new GetQuestContract
                        {
                            Id = contract.QuestId
                        }
                     );
                    if (!questRes.Success || questRes.Data == null)
                    {
                        return CommonHttpHelper.BuildErrorResponse<RegisterRoomViewModel>(
                           initialError: "Не удалось получить квест");
                    }
                    var questViewModel = questRes.Data;
                    bool isSetQuest = await _cacheStorage.SetQuestAsync(questViewModel);

                    if (isSetQuest)
                    {
                        return CommonHttpHelper.BuildErrorResponse<RegisterRoomViewModel>(
                      initialError: "Не удалось установить квест");
                    }
                    quest = questViewModel;
                }

                //в случае "жесткой" политики блокирум доступ к квесту
                if ((quest.Policy.PolicyType == PolicyType.Private || quest.Policy.PolicyType == PolicyType.Link) && contract.RequestUserId != quest.UserId)
                {                                                                                                                                   
                    return CommonHttpHelper.BuildErrorResponse<RegisterRoomViewModel>(
                            statusCode: HttpStatusCode.Forbidden,
                            initialError: "Нет доступа");
                }


                if (contract.RequestUserId.HasValue)
                {
                    //получаем номер комнаты
                    var roomKey = await GenerateNumberRoom();
                    if (roomKey == null)
                    {
                        return CommonHttpHelper.BuildErrorResponse<RegisterRoomViewModel>(
                            initialError: "Не удалось создать комнату");
                    }

                    //создаем комнату в бд и в кеше
                    contract.Key = roomKey;
                    await _processQuestStorage.CreateRoomAsync(contract);
                    await _cacheStorage.CreateRoomAsync(contract);

                    var result = new RegisterRoomViewModel
                    {
                        Key = roomKey.Value,
                        Quest = _mapper.Map<QuestProcessViewModel>(quest)
                    };

                    return CommonHttpHelper.BuildSuccessResponse(result, HttpStatusCode.OK);
                }
                else
                {
                    return CommonHttpHelper.BuildErrorResponse<RegisterRoomViewModel>(
                        initialError: "Нет пользователя в запросе", statusCode: HttpStatusCode.Unauthorized);
                }
            }
            catch (Exception ex)
            {
                return CommonHttpHelper.BuildErrorResponse<RegisterRoomViewModel>(
                   HttpStatusCode.InternalServerError,
                   ex.ToExceptionDetails(),
                   $"Ошибка выполнения метода {nameof(RegisterRoomAsync)} ReqId : {contract.RequestId}");

            }
        }

        private async Task<Guid?> GenerateNumberRoom()
        {
            int attempt = 0;
            while (attempt < 10)
            {
                var value = Guid.NewGuid();
                bool isDbExist = await _processQuestStorage.IsRoomExist(value);
                bool isCacheExist = await _cacheStorage.IsRoomExist(value);
                if (isCacheExist && isDbExist)
                {
                    return value;
                }
                attempt++;
            }
            return null;
        }
    }
}
