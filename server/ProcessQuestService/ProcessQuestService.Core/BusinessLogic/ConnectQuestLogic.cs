using AutoMapper;
using CommonInfrastructure.Extension;
using CommonInfrastructure.Http;
using CommonInfrastructure.Http.Helpers;
using GenerateQuestsService.DataContracts.DataContracts;
using GenerateQuestsService.DataContracts.Enums;
using GenerateQuestsService.DataContracts.Interfaces;
using ProcessQuestDataContracts.DataContracts;
using ProcessQuestDataContracts.ViewModels;
using ProcessQuestService.Core.Helpers;
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
