using AngularASPNETCore2WebApiAuth.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AngularASPNETCore2WebApiAuth.Data
{
  public class ApplicationDbContext : IdentityDbContext<AppUser>
  {
    public ApplicationDbContext(DbContextOptions options)
        : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder builder)
    {
      base.OnModelCreating(builder);
      builder.Entity<UserFollower>()
      .HasOne(pt => pt.AppUser)
      .WithMany(p => p.UserFollowers)
      .HasForeignKey(pt => pt.AppUserId);

      builder.Entity<UserFollower>()
      .HasOne(pt => pt.Follower)
      .WithMany(t => t.FollowerUsers)
      .HasForeignKey(pt => pt.FollowerId);
    }

    public DbSet<Customer> Customers { get; set; }
    public DbSet<Recepie> Recepies { get; set; }
    public DbSet<Ingridient> Ingridients { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<UserRecpieVote> UserRecpieVotes { get; set; }
    public DbSet<UserFollower> UserFollowers { get; set; }
  }

}
