using AuthService.Database.Models;
using AuthService.DataContracts.RefreshToken;
using AutoMapper;

namespace AuthService.Database.Mappers
{
    /// <summary>
    /// Класс конфигурации маппера
    /// </summary>
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //----------------------------- Refresh
            CreateMap<CreateRefreshTokenContract, RefreshToken>();
            CreateMap<RefreshToken, RefreshTokenViewModel>();
        }
    }

}
