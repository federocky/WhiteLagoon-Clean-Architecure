using Microsoft.EntityFrameworkCore;

namespace WhiteLagoon.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
        {
        }
    }
}
