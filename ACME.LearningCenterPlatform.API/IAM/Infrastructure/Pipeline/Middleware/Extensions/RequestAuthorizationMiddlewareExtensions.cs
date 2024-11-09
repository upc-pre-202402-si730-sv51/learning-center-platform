using ACME.LearningCenterPlatform.API.IAM.Infrastructure.Pipeline.Middleware.Components;

namespace ACME.LearningCenterPlatform.API.IAM.Infrastructure.Pipeline.Middleware.Extensions;

public static class RequestAuthorizationMiddlewareExtensions
{
    public static IApplicationBuilder UseRequestAuthorization(this IApplicationBuilder app)
    {
        return app.UseMiddleware<RequestAuthorizationMiddleware>();
    }
}