using AuthService.Core.HelperModels;
using AuthService.Database.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace AuthService.Core.Helpers
{
    public class GenerateTokenHelper
    {
        private readonly JwtSetting _jwtSettings;

        public GenerateTokenHelper(IOptions<JwtSetting> jwtSetting)
        {
            _jwtSettings = jwtSetting.Value;
        }

        public SymmetricSecurityKey GetTokenSecurityKey()
        {
            return _jwtSettings.GetSymmetricSecurityKey();
        }
        public string GenerateJwtToken(ApplicationUser user, string hashPassword, IList<string> userRoles)
        {
            var jwt = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.TokenExpiresMinutes),
                claims: GetIdentity(user, hashPassword, userRoles),
                signingCredentials: new SigningCredentials(_jwtSettings.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256)
            );

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            return encodedJwt;
        }

        public (string,int) GenerateJwtRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return (Convert.ToBase64String(randomNumber), _jwtSettings.RefreshTokenExpiresMinutes);
            }
        }

        private IEnumerable<Claim> GetIdentity(ApplicationUser user, string hashPassword, IList<string> userRoles)
        {

            //идентификация на основе логина, захешированного пароля
            //также на всякий добавляем пароль
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Hash, hashPassword),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };
            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            return claims;
        }

    }
}
