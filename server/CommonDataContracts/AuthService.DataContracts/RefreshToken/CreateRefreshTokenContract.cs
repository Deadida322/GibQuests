namespace AuthService.DataContracts.RefreshToken
{
    public class CreateRefreshTokenContract
    {
        public int UserId { get; set; }

        public string RefreshTokenValue { get; set; }

        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}
