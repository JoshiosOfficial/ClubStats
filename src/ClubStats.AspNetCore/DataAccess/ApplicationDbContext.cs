using ClubStats.AspNetCore.DataAccess.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ClubStats.AspNetCore.DataAccess;

public class ApplicationDbContext : IdentityDbContext<User, IdentityRole, string>
{
    
    public DbSet<Organization> Organizations { get; set; }
    public DbSet<OrganizationMember> OrganizationMembers { get; set; }
    public DbSet<Meeting> Meetings { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<Attendee> Attendees { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}