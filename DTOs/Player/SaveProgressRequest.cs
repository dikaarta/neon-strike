namespace Game.Web.DTOs.Player;

public class SaveProgressRequest
{
    public int Coins { get; set; }

    public int BestScore { get; set; }

    public int? SelectedAvatarId { get; set; }
}