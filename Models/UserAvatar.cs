namespace Game.Web.Models
{
    public class UserAvatar
    {
        public int UserId { get; set; }

        public User User { get; set; } = null!;

        public int AvatarId { get; set; }

        public Avatar Avatar { get; set; } = null!;
    }
}
