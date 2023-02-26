using AuthService.Database.Interfaces;
using AuthService.Database.Models;
using AuthService.DataContracts.CommonContracts;
using AuthService.DataContracts.RefreshToken;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Database.Implements
{
    public class RefreshStorage : IRefreshStorage
    {
        private AuthDbContext _context;

        private readonly IMapper _mapper;
        public RefreshStorage(AuthDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> AddRefreshToken(CreateRefreshTokenContract contract)
        {
            var refresh = _mapper.Map<RefreshToken>(contract);

            await _context.RefreshTokens.AddAsync(refresh);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteRefreshToken(DeleteContract contract)
        {
            var refreshToken = _context.RefreshTokens
                .Where(el => el.IsDeleted == false
                        && el.Id == contract.Id).FirstOrDefault();

            if (refreshToken == null)
            {
                return false;
            }

            refreshToken.IsDeleted = true;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task DeleteOldRefreshToken()
        {
            var refreshTokens = await _context.RefreshTokens
                .Where(el => el.IsDeleted == true
                        || DateTime.UtcNow > el.RefreshTokenExpiryTime).ToListAsync();

            if (refreshTokens != null)
            {
                foreach(var token in refreshTokens)
                {
                    _context.RefreshTokens.Remove(token);
                }
                await _context.SaveChangesAsync();
            }
        }

        public async Task<RefreshTokenViewModel> GetRefreshToken(GetContract contract)
        {
            var refreshToken = await _context.RefreshTokens
                .Where(el => el.IsDeleted == false
                        && el.Id == contract.Id).FirstOrDefaultAsync();

            return _mapper.Map<RefreshTokenViewModel>(refreshToken);
        }

        public async Task<IList<RefreshTokenViewModel>> GetUserRefreshTokens(GetRefreshesByUserContract contract)
        {
            //сразу же проверяем удаленность токена и срок его жизни
            var refreshTokens = await _context.RefreshTokens
                .Where(el => el.IsDeleted == false
                        && el.UserId == contract.UserId
                        && DateTime.UtcNow < el.RefreshTokenExpiryTime).ToListAsync();

            IList<RefreshTokenViewModel> result = new List<RefreshTokenViewModel>();
            foreach(var token in refreshTokens)
            {
                result.Add(_mapper.Map<RefreshTokenViewModel>(token));
            }
            return result;
        }
    }
}
