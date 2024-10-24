using DatingTelegramBot.Domain.Enum;
using Microsoft.EntityFrameworkCore;
using System.Drawing;

namespace DatingTelegramBot.Domain.Entity
{
    public class UserEntity
    {
        public int UserEntityId { get; set; }
        public int TgId { get; set; }
        public string UserName { get; set; }

        public int Age { get; set; }
        public string ImagePath { get; set; }
        public Coordinates Coordinates { get; set; }
        public string TgUserName { get; set; }
        public long TgChatId { get; set; }

        public Gender SearchInterest { get; set; }
        public Gender Gender { get; set; }
    }
    public class Coordinates
    {
        public int X { get; set; }
        public int Y { get; set; }
    }

}
