namespace Game.Web.DTOs.Player;

public class PlayerProfileResponse
{
    public string Username { get; set; } = string.Empty;

    public int Coins { get; set; }

    public int BestScore { get; set; }

    public int? SelectedAvatarId { get; set; }
}