﻿using CommonInfrastructure.Http;

namespace ProcessQuestDataContracts.DataContracts
{
    public class GetProcessQuestContract : CommonHttpRequest
    {
        public int Id { get; set; }
    }
}