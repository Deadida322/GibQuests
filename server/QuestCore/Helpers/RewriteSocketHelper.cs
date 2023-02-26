using Microsoft.AspNetCore.Rewrite;

namespace QuestCore.Helpers
{
    public static class RewriteSocketHelper
    {
        public static void RewriteSocketRequests(RewriteContext context)
        {
            var path = context.HttpContext.Request.Path;
            var pathValue = path.Value; // запрошенный путь
            if (path.StartsWithSegments(new PathString("/room"), StringComparison.OrdinalIgnoreCase))
            {
                context.HttpContext.Request.Path = new PathString("/localhost:44325/room/1");
            }
            return;
        }
    }
}
