namespace EventManagement.Registration.Data.Seed
{
    using System;

    using Microsoft.EntityFrameworkCore;

    public static class IntegrationSeed
    {
        public static Action<DbContext> Method { get; set; } = null;

        public static void Execute(DbContext context)
        {
            Method?.Invoke(context);
        }
    }
}
