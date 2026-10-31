namespace Game.Web.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Username { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;

        public int Coins { get; set; }

        public int BestScore { get; set; }

        public int? SelectedAvatarId { get; set; }

        public Avatar? SelectedAvatar { get; set; }

        public ICollection<UserAvatar> UserAvatars { get; set; } = [];
    }
}
