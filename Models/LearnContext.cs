using Microsoft.EntityFrameworkCore;
using learn.Models;

namespace learn.Models;

public class LearnContext : DbContext
{
    public LearnContext(DbContextOptions<LearnContext> options) : base(options)
    { }
    public DbSet<learn.Models.UserModel>? User { get; set; }

}