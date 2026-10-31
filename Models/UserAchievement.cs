namespace Game.Web.Models
{
    public class UserAchievement
    {
        public int UserId { get; set; }

        public User User { get; set; } = null!;

        public int AchievementId { get; set; }

        public Achievement Achievement { get; set; } = null!;

        public DateTime UnlockedAt { get; set; }
    }
}
