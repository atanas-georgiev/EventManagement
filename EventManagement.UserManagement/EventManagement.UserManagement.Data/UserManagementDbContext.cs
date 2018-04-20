namespace EventManagement.UserManagement.Data
{
    using EventManagement.UserManagement.Shared.Models;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    public class UserManagementDbContext : IdentityDbContext<User>
    {
        public UserManagementDbContext(DbContextOptions<UserManagementDbContext> options)
            : base(options)
        {
        }
    }
}