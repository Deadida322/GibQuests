using AuthService.DataContracts.Interfaces;
using AutoMapper;
using CommonInfrastructure.Extension;
using CommonInfrastructure.Http;
using CommonInfrastructure.Http.Helpers;
using GenerateQuestsService.DataContracts.DataContracts;
using GenerateQuestsService.DataContracts.Interfaces;
using Microsoft.AspNetCore.SignalR;
using ProcessQuestDataContracts.DataContracts;
using ProcessQuestDataContracts.Enums;
using ProcessQuestDataContracts.ProcessModels;
using ProcessQuestDataContracts.ViewModels;
using ProcessQuestService.Core.HelperModels.SocketErrors;
using ProcessQuestService.Core.Helpers;
using ProcessQuestService.Core.Hubs;
using ProcessQuestService.ProcessQuestDatabase.Interfaces;
using System.Net;

namespace ProcessQuestService.Core.BusinessLogic
{
    public class ConnectQuestLogic
    {
        private IGenerateQuestsApi _generateQuestsApi;
        private IAuthApi _authApi;
        private IMapper _mapper;
        private IProcessQuestStorage _processQuestStorage;
        private IProcessCacheStorage _cacheStorage;
        private IHubContext<ProcessHub> _processHub;
        private ProcessIdentityManager _processIdentityManager;
        public ConnectQuestLogic(IGenerateQuestsApi generateQuestsApi,
            IAuthApi authApi,
            IMapper mapper, IProcessQuestStorage processQuestStorage,
            IProcessCacheStorage processCacheStorage, IHubContext<ProcessHub> processHub, ProcessIdentityManager processIdentityManager)
        {
            _generateQuestsApi = generateQuestsApi;
            _authApi = authApi;
            _mapper = mapper;
            _processQuestStorage = processQuestStorage;
            _cacheStorage = processCacheStorage;
            _processHub = processHub;
            _processIdentityManager = processIdentityManager;
        }

        //public async Task<CommonHttpResponse<IList<UserProcessingQuestViewModel>>> GetUserQuestsProcessingAsync(CommonHttpRequest contract)
        //{
        //    try
        //    {
        //        IList<UserProcessingQuestViewModel> result = new List<UserProcessingQuestViewModel>();

        //        var userId = contract.RequestUserId.Value;
        //        var processModels = await _cacheHelper.GetAllUserQuestProcessModelsAsync(userId);

        //        foreach (var processModel in processModels)
        //        {
        //            var questRes = await _generateQuestsApi.GetQuestAsync(
        //                new GetQuestContract
        //                {
        //                    Id = Int32.Parse(processModel.QuestId),
        //                    RequestId = contract.RequestId,
        //                    RequestUserId = contract.RequestUserId,
        //                    RequestUserName = contract.RequestUserName
        //                }
        //            );
        //            if (!questRes.Success || questRes.Data == null)
        //            {
        //                return CommonHttpHelper.BuildErrorResponse<IList<UserProcessingQuestViewModel>>(
        //                    extErrors: questRes.Errors.ToList());
        //            }
        //            var quest = questRes.Data;

        //            string roomKey = processModel.Key;
        //            int userStagePrev = processModel.UserProcessing[userId].Stage;
        //            var currentStage = quest.Stages[userStagePrev];
        //            result.Add(new UserProcessingQuestViewModel
        //            {
        //                QuestId = quest.Id,
        //                QuestName = quest.Title,
        //                Room = processModel.Key,
        //                Stage = _mapper.Map<StageProcess>(currentStage)

        //            });
        //        }
        //        return CommonHttpHelper.BuildSuccessResponse(result, HttpStatusCode.OK);
        //    }
        //    catch (Exception ex)
        //    {
        //        return CommonHttpHelper.BuildErrorResponse<IList<UserProcessingQuestViewModel>>(
        //           HttpStatusCode.InternalServerError,
        //           ex.ToExceptionDetails(),
        //           $"Ошибка выполнения метода {nameof(GetUserQuestsProcessingAsync)} ReqId : {contract.RequestId}");
        //    }
        //}

        public async Task<CommonHttpResponse<RegisterRoomViewModel>> RegisterRoomAsync(RegisterRoomContract contract)
        {
            try
            {
                //получаем квест
                var quest = await GetQuestAsync(new GetProcessQuestContract
                {
                    Id = contract.QuestId
                });
                if (quest == null)
                {
                    return CommonHttpHelper.BuildErrorResponse<RegisterRoomViewModel>(
                       initialError: "Не удалось получить квест");
                }

                if (!contract.RequestUserId.HasValue)
                {

                    return CommonHttpHelper.BuildErrorResponse<RegisterRoomViewModel>(
                        initialError: "Нет пользователя в запросе", statusCode: HttpStatusCode.Unauthorized);
                }

                //в случае "жесткой" политики блокирум доступ к квесту
                if ((quest.Policy.PolicyType == PolicyProcessType.Private || quest.Policy.PolicyType == PolicyProcessType.Link)
                    && contract.RequestUserId.Value != quest.UserId)
                {
                    return CommonHttpHelper.BuildErrorResponse<RegisterRoomViewModel>(
                            statusCode: HttpStatusCode.Forbidden,
                            initialError: "Нет доступа");
                }
                //получаем номер комнаты
                var roomKey = await GenerateNumberRoom();
                if (roomKey == null)
                {
                    return CommonHttpHelper.BuildErrorResponse<RegisterRoomViewModel>(
                        initialError: "Не удалось создать комнату");
                }

                //создаем комнату в бд и в кеше
                contract.Key = roomKey.Value;

                await _processQuestStorage.CreateRoomAsync(contract);
                await _cacheStorage.CreateRoomAsync(contract);

                var result = new RegisterRoomViewModel
                {
                    Key = roomKey.Value,
                    Quest = _mapper.Map<QuestProcessViewModel>(quest)
                };

                return CommonHttpHelper.BuildSuccessResponse(result, HttpStatusCode.OK);
               
            }
            catch (Exception ex)
            {
                return CommonHttpHelper.BuildErrorResponse<RegisterRoomViewModel>(
                   HttpStatusCode.InternalServerError,
                   ex.ToExceptionDetails(),
                   $"Ошибка выполнения метода {nameof(RegisterRoomAsync)} ReqId : {contract.RequestId}");

            }
        }

        public async Task<CommonHttpResponse<ConnectToRoomViewModel>> ConnectToRoomAsync(CommonRoomContract contract)
        {
            try
            {
                //получаем комнату из кеша
                var processRoom = await _cacheStorage.GetRoomAsync(new CommonRoomContract
                {
                    Key = contract.Key,
                });
                if (processRoom == null)
                {
                    return CommonHttpHelper.BuildErrorResponse<ConnectToRoomViewModel>(
                        initialError: "Нет комнаты прохождения");
                }

                //получаем квест
                var quest = await GetQuestAsync(new GetProcessQuestContract
                {
                    Id = processRoom.QuestId
                });
                if (quest == null)
                {
                    return CommonHttpHelper.BuildErrorResponse<ConnectToRoomViewModel>(
                       initialError: "Ошибка при получении квеста");
                }

                //получаем пользователя
                var userRes = await _authApi.GetUserByIdAsync(contract.RequestUserId.Value);
                if (!userRes.Success || userRes.Data == null)
                {
                    return CommonHttpHelper.BuildErrorResponse<ConnectToRoomViewModel>(
                      initialError: "Нет такого пользователя!");
                }
                var user = _mapper.Map<UserDataViewModel>(userRes.Data);

                //в случае приватной политики блокирум доступ к квесту
                if (quest.Policy.PolicyType == PolicyProcessType.Private
                    && user.Id != quest.UserId)
                {
                    return CommonHttpHelper.BuildErrorResponse<ConnectToRoomViewModel>(
                            statusCode: HttpStatusCode.Forbidden,
                            initialError: "Нет доступа");
                }
                var userToken = _processIdentityManager.GenerateJwtToken(user, contract.Key);

                var result = await _cacheStorage.ConnectToRoomAsync(contract, user, userToken);

                if (result == null)
                {
                    return CommonHttpHelper.BuildErrorResponse<ConnectToRoomViewModel>(
                        initialError: "Ошибка при подключении к комнате");
                }
                result.Quest = _mapper.Map<QuestProcessViewModel>(quest);

                return CommonHttpHelper.BuildSuccessResponse(result, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return CommonHttpHelper.BuildErrorResponse<ConnectToRoomViewModel>(
                   HttpStatusCode.InternalServerError,
                   ex.ToExceptionDetails(),
                   $"Ошибка выполнения метода {nameof(ConnectToRoomAsync)} ReqId : {contract.RequestId}");
            }
        }

        public async Task<CommonHttpResponse> StartRoomAsync(CommonRoomContract contract)
        {
            try
            {
                //получаем комнату из кеша
                var processRoom = await _cacheStorage.GetRoomAsync(new CommonRoomContract
                {
                    Key = contract.Key,
                });
                if (processRoom == null)
                {
                    return CommonHttpHelper.BuildErrorResponse<ConnectToRoomViewModel>(
                        initialError: "Нет комнаты прохождения");
                }

                //получаем квест
                var quest = await GetQuestAsync(new GetProcessQuestContract
                {
                    Id = processRoom.QuestId
                });
                if (quest == null)
                {
                    return CommonHttpHelper.BuildErrorResponse<ConnectToRoomViewModel>(
                       initialError: "Ошибка при получении квеста");
                }

                //в случае "жесткой" политики блокирум доступ к квесту
                if ((quest.Policy.PolicyType == PolicyProcessType.Private || quest.Policy.PolicyType == PolicyProcessType.Link)
                    && contract.RequestUserId.Value != quest.UserId)
                {
                    return CommonHttpHelper.BuildErrorResponse<RegisterRoomViewModel>(
                            statusCode: HttpStatusCode.Forbidden,
                            initialError: "Нет доступа");
                }
                //эмит события всем участникам комнаты



                return CommonHttpHelper.BuildSuccessResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return CommonHttpHelper.BuildErrorResponse<ConnectToRoomViewModel>(
                   HttpStatusCode.InternalServerError,
                   ex.ToExceptionDetails(),
                   $"Ошибка выполнения метода {nameof(StartRoomAsync)} ReqId : {contract.RequestId}");
            }
        }


        public async Task<UserDataViewModel> ValidateConnectUserToRoomAsync(ProcessConnectContract contract)
        {
            //получаем комнату из кеша
            var processRoom = await _cacheStorage.GetRoomAsync(new CommonRoomContract
            {
                Key = contract.Key,
            });

            if (processRoom == null)
            {
                throw new BadRequestException("Нет комнаты прохождения");
            }
            //получаем пользователя
            var userProcessing = processRoom.UserProcessing.Where(u => u.Value.Token == contract.Token).FirstOrDefault();
            //в случае если не нашли пользователя
            if (userProcessing.Key <= 0)
            {
                throw new UnauthorizeException("Пользователь не подключился к комнате");
            }
           
            //получаем квест
            var quest = await GetQuestAsync(new GetProcessQuestContract
            {
                Id = processRoom.QuestId
            });
            if (quest == null)
            {
                throw new BadRequestException("Ошибка при получении квеста");
            }

            //эмит события всем участникам комнаты
            return userProcessing.Value.UserData;
        }
        private async Task<FullQuestViewModel?> GetQuestAsync(GetProcessQuestContract contract)
        {

            //получаем квест из кеша
            var quest = await _cacheStorage.GetQuestAsync(contract);

            //если его нет, то получаем его и устанавливаем в кеш
            if (quest == null)
            {
                var questRes = await _generateQuestsApi.GetQuestAsync(
                    new GetQuestContract
                    {
                        Id = contract.Id
                    }
                 );
                if (!questRes.Success || questRes.Data == null)
                {
                    return null;
                }
                var questViewModel = _mapper.Map<FullQuestViewModel>(questRes.Data);
                bool isSetQuest = await _cacheStorage.SetQuestAsync(questViewModel);

                if (!isSetQuest)
                {
                    return null;
                }
                quest = questViewModel;
            }
            return quest;
        }

        private async Task<Guid?> GenerateNumberRoom()
        {
            int attempt = 0;
            while (attempt < 10)
            {
                var value = Guid.NewGuid();
                bool isDbExist = await _processQuestStorage.IsRoomExist(value);
                bool isCacheExist = await _cacheStorage.IsRoomExist(value);
                if (!(isCacheExist || isDbExist))
                {
                    return value;
                }
                attempt++;
            }
            return null;
        }
    }
}
