namespace EventManagement.UserManagement.Shared.Testing.Extensions
{
    using EventManagement.UserManagement.Shared.Testing.Middleware;

    using Microsoft.AspNetCore.Builder;

    public static class ApplicationBuilderExtensions
    {
        public static void UseTesting(this IApplicationBuilder app)
        {
            app.UseMiddleware<TestUserMiddleware>();
        }
    }
}
