
using Swashbuckle.AspNetCore.Annotations;

namespace CommonInfrastructure.Http
{
    /// <summary>
    /// Общий вид HTTP запроса
    /// </summary>
    public class CommonHttpRequest
    {
        public CommonHttpRequest()
        {
            RequestId = Guid.NewGuid();
        }

        [Obsolete]
        public Guid? RequestId { get; set; }

        [Obsolete]
        public int? RequestUserId { get; set; } = null;

        [Obsolete]
        public string? RequestUserName { get; set; } = null;
    }

}
