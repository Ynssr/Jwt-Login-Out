using Microsoft.EntityFrameworkCore;
using Jwt_LogIn_Out.Models;

namespace Jwt_LogIn_Out.Data;
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
}
