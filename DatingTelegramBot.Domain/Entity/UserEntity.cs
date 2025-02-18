using DatingTelegramBot.Domain.Enum;
using Telegram.Bot.Types;


namespace DatingTelegramBot.Domain.Entity;

public sealed class UserEntity
{
    public UserEntity(string? userName, string tgUserName,  int age, string? imagePath,
        Coordinates? coordinates,
        Gender gender,
        GenderSearch searchInterest, 
        string description,
        long tgChatId)
    {
        UserName = userName;
        Age = age;
        TgUserName = tgUserName;
        ImagePath = imagePath;
        Coordinates = coordinates;
        Gender = gender;
        SearchInterest = searchInterest;
        Description = description;
        TgChatId = tgChatId;
    }
    private UserEntity() { 
    }
    

    public int UserEntityId { get; set; }
    public string UserName { get; set; }
    public int Age { get; set; }
    public string ImagePath { get; set; }
    public Coordinates Coordinates { get; set; }
    public string TgUserName { get; set; }
    public long TgChatId { get; set; }
    public string? Description { get; set; }

    public GenderSearch SearchInterest { get; set; }
    public Gender Gender { get; set; }
}

public class Coordinates(double latitude, double longitude)
{
    public double Latitude { get; set; } = latitude;
    public double Longitude { get; set; } = longitude;

}
