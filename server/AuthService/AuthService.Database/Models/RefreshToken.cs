using CommonInfrastructure.Db;

namespace AuthService.Database.Models
{
    public class RefreshToken : BaseDbModel
    {
        public int UserId { get; set; }

        public string RefreshTokenValue { get; set; }

        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}
