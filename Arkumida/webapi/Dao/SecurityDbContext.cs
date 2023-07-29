using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using webapi.Models.Identity;

namespace webapi.Dao;

/// <summary>
/// Security database context, users and roles are living here
/// </summary>
public class SecurityDbContext : IdentityDbContext<User>
{
    public SecurityDbContext(DbContextOptions<SecurityDbContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Add custom stuff here
    }
}