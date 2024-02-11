﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessQuestDataContracts.ProcessModels
{
    public class ProcessModel
    {
        public ProcessModel()
        {
            UserProcessing = new Dictionary<int, ProcessUserModel>();
        }
        public Guid Key { get; set; }

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
