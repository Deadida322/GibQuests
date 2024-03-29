﻿using AuthService.DataContracts.CommonContracts;
using AuthService.DataContracts.Interfaces;
using AuthService.DataContracts.User;
using AutoMapper;
using CommonDatabase.QuestDatabase.Interfaces;
using CommonInfrastructure.Extension;
using CommonInfrastructure.Http;
using CommonInfrastructure.Http.Helpers;
using GenerateQuestsService.DataContracts.DataContracts;
using GenerateQuestsService.DataContracts.Enums;
using GenerateQuestsService.DataContracts.Models;
using GenerateQuestsService.DataContracts.Models.Stages;
using System.Diagnostics.Contracts;
using System.Net;


namespace GenerateQuestsService.Core.BusinessLogic
{
    public class GenerateQuestLogic
    {
        private IGenerateQuestStorage _generateQuestStorage;
        private IMapper _mapper;
        private IAuthApi _authApi;

        public GenerateQuestLogic(IGenerateQuestStorage generateQuestStorage, IMapper mapper, IAuthApi authApi)
        {
            _generateQuestStorage = generateQuestStorage;
            _mapper = mapper;
            _authApi = authApi;
        }

        private bool IsOrderedStage(IList<Stage> stages)
        {
            var orderStages = stages.OrderBy(el => el.Order).ToList();
            int orderCount = 0;
            foreach (var stage in orderStages)
            {
                if (stage.Order == orderCount)
                {
                    orderCount++;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        private bool IsPolicyCorrect(QuestPolicy policy)
        {
            if (policy == null) return false;
            //если значение политики неуместные
            if (policy.PolicyType == PolicyType.Unknown || policy.MemberType == MemberType.Unknown)
            {
                return false;
            }
            //если политика приватная и групповая - то ошибка
            if (policy.PolicyType == PolicyType.Private && policy.MemberType == MemberType.Group)
            {
                return false;
            }
            return true;
        }

        public async Task<CommonHttpResponse> CreateQuestAsync(CreateQuestContract quest)
        {
            if(quest == null)
            {
                return CommonHttpHelper.BuildErrorResponse(initialError: "Пустой запрос");
            }
            if (quest.RequestUserId == null)
            {
                return CommonHttpHelper.BuildNotFoundErrorResponse(initialError: "Нет автора квеста");
            }
            if(quest.Stages.Count == 0)
            {
                return CommonHttpHelper.BuildErrorResponse(initialError: "Добавьте этапы в квест");
            }
            if(!IsOrderedStage(quest.Stages))
            {
                return CommonHttpHelper.BuildErrorResponse(initialError: "В этапах квеста нет порядка (order) нужно {0,1,2 ...}");
            } 
            if(!IsPolicyCorrect(quest.Policy))
            {
                return CommonHttpHelper.BuildErrorResponse(initialError: "Политика квеста неправильная" +
                    ", нельзя создать приватный групповой квест");
            }
            try
            {
                await _generateQuestStorage.CreateQuestAsync(quest);
                return CommonHttpHelper.BuildSuccessResponse(HttpStatusCode.OK);
            }
            catch(Exception ex)
            {
                return CommonHttpHelper.BuildErrorResponse(
                    HttpStatusCode.InternalServerError,
                                    ex.ToExceptionDetails(),
                    $"Ошибка выполнения метода {nameof(CreateQuestAsync)} ReqId : ");
            }
        }

        public async Task<CommonHttpResponse<bool>> UpdateQuestAsync(UpdateQuestContract quest)
        {
            if (quest == null)
            {
                return CommonHttpHelper.BuildErrorResponse<bool>(initialError: "Пустой запрос");
            }
            if (quest.RequestUserId == null)
            {
                return CommonHttpHelper.BuildNotFoundErrorResponse<bool>(initialError: "Нет автора квеста");
            }
            if (quest.Stages.Count == 0)
            {
                return CommonHttpHelper.BuildErrorResponse<bool>(initialError: "Добавьте этапы в квест");
            }
            if (!IsOrderedStage(quest.Stages))
            {
                return CommonHttpHelper.BuildErrorResponse<bool>(initialError: "В этапах квеста нет порядка (order) нужно {0,1,2 ...}");
            }
            try
            {
                var res = await _generateQuestStorage.UpdateQuestAsync(quest);
                if(!res)
                {
                    return CommonHttpHelper.BuildErrorResponse<bool>(initialError: "У вас нет такого квеста");
                }
                return CommonHttpHelper.BuildSuccessResponse(res);
            }
            catch (Exception ex)
            {
                return CommonHttpHelper.BuildErrorResponse<bool>(
                    HttpStatusCode.InternalServerError,
                                    ex.ToExceptionDetails(),
                    $"Ошибка выполнения метода {nameof(UpdateQuestAsync)} ReqId : ");
            }
        }

        public async Task<CommonHttpResponse<QuestViewModel>> GetQuestAsync(GetQuestContract contract)
        {
            if (contract == null)
            {
                return CommonHttpHelper.BuildErrorResponse< QuestViewModel>(initialError: "Пустой запрос");
            }
            try
            {
                var result = await _generateQuestStorage.GetQuestAsync(contract);
                if(result == null)
                {
                    return CommonHttpHelper.BuildNotFoundErrorResponse<QuestViewModel>(initialError: "Нет такого квеста");
                }
                var authorRes = await _authApi.GetUserByIdAsync(result.UserId);
                if(!authorRes.Success || (authorRes.Success && authorRes.Data.UserName.IsNullOrEmpty()))
                {
                    result.UserName = "";
                }
                else
                {
                    result.UserName = authorRes.Data.UserName;
                }
                return CommonHttpHelper.BuildSuccessResponse(_mapper.Map<QuestViewModel>(result), HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return CommonHttpHelper.BuildErrorResponse<QuestViewModel>(
                    HttpStatusCode.InternalServerError,
                                    ex.ToExceptionDetails(),
                    $"Ошибка выполнения метода {nameof(GetQuestAsync)} ReqId : ");
            }
        }

        public async Task<CommonHttpResponse<IList<ShortQuestViewModel>>> GetFilteredQuestsAsync(GetFilteredQuestsContract contract)
        {
            if (contract == null)
            {
                return CommonHttpHelper.BuildErrorResponse<IList<ShortQuestViewModel>>(initialError: "Пустой запрос");
            }
            try
            {
                //получаем отфильтрованные квесты
                var result = await _generateQuestStorage.GetFilteredQuestsAsync(contract);
                if (result == null)
                {
                    return CommonHttpHelper.BuildErrorResponse<IList<ShortQuestViewModel>>(initialError: "Ошибка при возвращении");
                }
                //заводим словарь авторов
                Dictionary<int, string> authors = new();

                //заносим пустые значения
                foreach (var item in result)
                {
                    authors.Add(item.Id, "");
                }
                //получаем авторов из сервиса пользователей
                var authorRes = await _authApi.GetFilteredUsersAsync(new GetFilteredUsersContract
                {
                    Ids = authors.Keys.ToList(),
                });
                
                if (!authorRes.Success)
                {
                    return CommonHttpHelper.BuildErrorResponse<IList<ShortQuestViewModel>>(
                   HttpStatusCode.InternalServerError,
                    new List<string>() { "Ошибка получения авторов"},
                   $"Ошибка выполнения метода {nameof(GetFilteredQuestsAsync)} ReqId : ");
                }
                //в случае успешного запроса заполняем словарь
                else
                {
                    foreach(var a in authorRes.Data)
                    {
                        if(authors.ContainsKey(a.Id))
                        {
                            authors[a.Id] = a.UserName;
                        }
                    }
                    foreach(var q in result )
                    {
                        if (authors.ContainsKey(q.UserId))
                        {
                            q.UserName = authors[q.UserId];
                        }
                        else
                        {
                            q.UserName = "";
                        }
                    }
                    return CommonHttpHelper.BuildSuccessResponse(result);
                }
            }
            catch (Exception ex)
            {
                return CommonHttpHelper.BuildErrorResponse<IList<ShortQuestViewModel>>(
                    HttpStatusCode.InternalServerError,
                                    ex.ToExceptionDetails(),
                    $"Ошибка выполнения метода {nameof(GetFilteredQuestsAsync)} ReqId : ");
            }
        }

        public async Task<CommonHttpResponse<bool>> DeleteQuestAsync(DeleteQuestContract contract)
        {
            if (contract == null || contract.Id < 0)
            {
                return CommonHttpHelper.BuildErrorResponse<bool>(initialError: "Пустой запрос");
            }
            try
            {
                var result = await _generateQuestStorage.DeleteQuestAsync(contract);
                if (!result)
                {
                    return CommonHttpHelper.BuildNotFoundErrorResponse<bool>(initialError: "Нет такого квеста");
                }
                return CommonHttpHelper.BuildSuccessResponse(result);
            }
            catch (Exception ex)
            {
                return CommonHttpHelper.BuildErrorResponse<bool>(
                    HttpStatusCode.InternalServerError,
                                    ex.ToExceptionDetails(),
                    $"Ошибка выполнения метода {nameof(DeleteQuestAsync)} ReqId : ");
            }
        }

    }
}
