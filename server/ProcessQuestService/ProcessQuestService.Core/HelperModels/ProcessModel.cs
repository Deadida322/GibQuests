﻿namespace ProcessQuestService.Core.HelperModels
{
    public class ProcessModel
    {
        public ProcessModel() {
            UserProcessing = new Dictionary<int, ProcessUserModel>();
        }
        public string Key { get; set; }

        public string QuestId { get; set; }

        /// <summary>
        /// key - User Id
        /// ProcessUserModel - Auth Token and Stage number
        /// </summary>
        public IDictionary<int, ProcessUserModel> UserProcessing { get; set; }

        public bool IsHaveUser(int userId)
        {
            return UserProcessing.ContainsKey(userId);
        }
    }
}
