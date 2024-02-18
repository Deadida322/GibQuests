using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ProcessQuestDataContracts.ProcessModels;
using ProcessQuestService.Core.HelperModels;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;


namespace ProcessQuestService.Core.Helpers
{
    public class ProcessIdentityManager
    {
        private readonly JwtSetting _jwtSettings;

        public ProcessIdentityManager(IOptions<JwtSetting> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }

        public string GenerateJwtToken(UserDataViewModel user, Guid room)
        {
            var jwt = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: GetIdentity(user, room),
                signingCredentials: new SigningCredentials(_jwtSettings.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256)
            );

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            return encodedJwt;
        }

        private static IEnumerable<Claim> GetIdentity(UserDataViewModel user, Guid room)
        {

            //заносим в claim информацию по пользователю:
            //ID, логин, комнату
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim("room", room.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            return claims;
        }
    }
}
