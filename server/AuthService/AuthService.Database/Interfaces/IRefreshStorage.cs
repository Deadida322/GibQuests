using AuthService.DataContracts.CommonContracts;
using AuthService.DataContracts.RefreshToken;

namespace AuthService.Database.Interfaces
{
    public interface IRefreshStorage
    {
        Task<bool> AddRefreshToken(CreateRefreshTokenContract contract);

        Task<bool> DeleteRefreshToken(DeleteContract contract);

        Task<RefreshTokenViewModel> GetRefreshToken(GetContract contract);

        Task<IList<RefreshTokenViewModel>> GetUserRefreshTokens(GetRefreshesByUserContract contract);

        Task DeleteOldRefreshToken();

    }
}
