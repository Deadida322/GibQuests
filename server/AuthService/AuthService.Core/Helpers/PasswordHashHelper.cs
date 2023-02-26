using AuthService.Database.HelperModels;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Options;
using System.Text;

namespace AuthService.Core.Helpers
{
    public class PasswordHashHelper
    {
        private SecretSetting _secretSetting { get; set; }
        public PasswordHashHelper(IOptions<SecretSetting> secretSetting)
        {
            _secretSetting = secretSetting.Value;
        }

        public string ComputeHashPassword(string password)
        {
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password!,
                salt: Encoding.UTF8.GetBytes(_secretSetting.PasswordHash),
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 1000,
                numBytesRequested: 256 / 8));
        }
    }
}
