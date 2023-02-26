using Refit;

namespace AuthService.DataContracts.CommonContracts
{
    public class GetContract
    {
        [AliasAs("id")]
        public int Id { get; set; }
    }
}
