using Microsoft.IdentityModel.Tokens;

namespace QuestCore.TokenHelpers
{
    public static class QuestCoreLifetimeValidator
    {
        public static bool CheckLifeTime(DateTime? notBefore, DateTime? expires, SecurityToken securityToken, TokenValidationParameters validationParameters)
        {
            if (expires != null)
            {
                if (DateTime.UtcNow < expires.Value.ToUniversalTime())
                {
                    return true; // Still valid
                }
            }
            return false; // Expired
        }
    }
}
