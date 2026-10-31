using Game.Web.Models;

namespace Game.Web.Data;

public static class SeedData
{
    public static async Task InitializeAsync(GameDbContext dbContext)
    {
        if (!dbContext.Avatars.Any())
        {
            dbContext.Avatars.AddRange(
                new Avatar
                {
                    Id = 1,
                    Name = "Default",
                    Cost = 0
                },
                new Avatar
                {
                    Id = 2,
                    Name = "Red",
                    Cost = 100
                },
                new Avatar
                {
                    Id = 3,
                    Name = "Gold",
                    Cost = 500
                });

            await dbContext.SaveChangesAsync();
        }
    }
}