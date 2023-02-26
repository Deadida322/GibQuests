namespace AuthService.DataContracts.RefreshToken
{
    public class RefreshTokenViewModel
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string RefreshTokenValue { get; set; }

        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}
