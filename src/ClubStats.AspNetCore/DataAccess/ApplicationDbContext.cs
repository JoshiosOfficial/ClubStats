using Microsoft.EntityFrameworkCore;

namespace ClubStats.AspNetCore.DataAccess;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
}