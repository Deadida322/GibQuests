using Microsoft.AspNetCore.Rewrite;

namespace QuestCore.Helpers
{
    public static class RewriteSocketHelper
    {
        public static void RewriteSocketRequests(RewriteContext context)
        {
            var path = context.HttpContext.Request.Path;
            var pathValue = path.Value; // запрошенный путь
            if (path.StartsWithSegments(new PathString("/check"), StringComparison.OrdinalIgnoreCase))
            {
                context.HttpContext.Request.Host = new HostString("localhost:65456");
                context.HttpContext.Request.Path = new PathString("/check/1");
            }
        }
    }
}
