using AutoMapper;
using CommonDatabase.QuestDatabase.Interfaces;
using CommonDatabase.QuestDatabase.Models;
using CommonDatabase.QuestDatabase.Models.Stages;
using GenerateQuestsService.DataContracts.DataContracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics.Contracts;

namespace CommonDatabase.QuestDatabase.Implements
{
    public class GenerateQuestStorage : IGenerateQuestStorage
    {
        private QuestContext _questContext;
        private IMapper _mapper;

        public GenerateQuestStorage(QuestContext questContext, IMapper mapper)
        {
            _questContext = questContext;
            _mapper = mapper;
        }

        public async Task CreateQuestAsync(CreateQuestContract contract)
        {
            var quest = _mapper.Map<QuestEntity>(contract);
            await _questContext.Quests.AddAsync(quest);
            await _questContext.SaveChangesAsync();
        }

        public async Task<bool> UpdateQuestAsync(UpdateQuestContract contract)
        {
            var dbQuest = await _questContext.Quests.Where(q =>
                q.IsDeleted == false &&
                q.Id == contract.Id).Include(x => x.Stages)
                    .FirstOrDefaultAsync();

            if(dbQuest == null)
            {
                return false;
            }
            _mapper.Map(contract, dbQuest);
            _questContext.Entry(dbQuest).State = EntityState.Modified;
            await _questContext.SaveChangesAsync();
            return true;
        }

        public async Task<QuestViewModel> GetQuestAsync(GetQuestContract contract)
        {
            var result = await _questContext.Quests.Where(q =>
                q.IsDeleted == false &&
                q.Id == contract.Id).Include(x => x.Stages)
                    .FirstOrDefaultAsync();

            if(result == null)
            {
                return null;
            }
            //дополнительно проходимся во вложенным сущностям
            foreach(var stage in result.Stages)
            {
                if(stage is MapStageEntity)
                {
                    var mapStage = (stage as MapStageEntity);
                    var cords = await _questContext.Coordinates.Where(x =>
                    x.IsDeleted == false && 
                    x.MapStage.Id == mapStage.Id)
                        .FirstOrDefaultAsync();
                    mapStage.Coords = cords;
                }
                else if (stage is TestStageEntity)
                {
                    var textStage = (stage as TestStageEntity);
                    var questions = await _questContext.Questions.Where(x =>
                    x.IsDeleted == false &&
                    x.TestStage.Id == textStage.Id)
                        .ToListAsync();
                    textStage.Questions = questions;
                }
            }
            return _mapper.Map<QuestViewModel>(result);
        }

        public async Task<IList<ShortQuestViewModel>> GetFilteredQuestsAsync(GetFilteredQuestsContract contract)
        {
            var quests = await _questContext.Quests
                .Where(q => q.IsDeleted == false)
                .ToListAsync();

            var result = new List<ShortQuestViewModel>();
            foreach (var quest in quests)
            {
               result.Add(_mapper.Map<ShortQuestViewModel>(quest));
            }
            return result;
        }

        public async Task<bool> DeleteQuestAsync(DeleteQuestContract contract)
        {
            var elem = _questContext.Quests
                .Where(x =>
                x.IsDeleted == false &&
                    x.Id == contract.Id).FirstOrDefault();

            if (elem != null)
            {
                //если это квест того юзера
                if (contract.RequestUserId.HasValue && contract.RequestUserId == elem.UserId)
                {
                    elem.IsDeleted = true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
            await _questContext.SaveChangesAsync();

            return true;
        }


    }
}
