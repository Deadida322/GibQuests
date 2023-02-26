using AuthService.Database.Models;
using AuthService.DataContracts.User;
using AutoMapper;

namespace AuthService.Core.Mappers
{
    public class CoreMapperProfile : Profile
    {
        public CoreMapperProfile()
        {
            //----------------------------- User
            CreateMap<ApplicationUser, UserViewModel>();
            CreateMap<ApplicationUser, ShortUserViewModel>();
            CreateMap<UserViewModel, ApplicationUser>();
            CreateMap<UpdateUserContract, ApplicationUser>();
            CreateMap<CreateUserContract, ApplicationUser>();
        }
    }
}
