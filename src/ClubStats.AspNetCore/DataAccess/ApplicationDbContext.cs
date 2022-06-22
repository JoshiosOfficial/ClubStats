using ClubStats.AspNetCore.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace ClubStats.AspNetCore.DataAccess;

public class ApplicationDbContext : DbContext
{
    
    public DbSet<Organization> Organizations { get; set; }
    public DbSet<OrganizationMember> OrganizationMembers { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Meeting> Meetings { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<Attendee> Attendees { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }
}