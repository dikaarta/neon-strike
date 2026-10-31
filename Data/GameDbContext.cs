using Game.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace Game.Web.Data
{
    public class GameDbContext : DbContext
    {
        public GameDbContext(DbContextOptions<GameDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();

        public DbSet<Avatar> Avatars => Set<Avatar>();

        public DbSet<Achievement> Achievements => Set<Achievement>();

        public DbSet<UserAvatar> UserAvatars => Set<UserAvatar>();

        public DbSet<UserAchievement> UserAchievements => Set<UserAchievement>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User
            modelBuilder.Entity<User>()
                .HasKey(x => x.Id);

            // Avatar
            modelBuilder.Entity<Avatar>()
                .HasKey(x => x.Id);

            // Achievement
            modelBuilder.Entity<Achievement>()
                .HasKey(x => x.Id);

            // UserAvatar (many-to-many)
            modelBuilder.Entity<UserAvatar>()
                .HasKey(x => new
                {
                    x.UserId,
                    x.AvatarId
                });

            // UserAchievement (many-to-many)
            modelBuilder.Entity<UserAchievement>()
                .HasKey(x => new
                {
                    x.UserId,
                    x.AchievementId
                });
        }
    }
}
