using System;
using System.IO;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Bot
{
    internal class Program
    {
        static string token;
        
        static TelegramBotClient botClient = new TelegramBotClient(GetToken(token));
        static string GetToken(string token)
        {
            using (StreamReader sr = new StreamReader("token.txt"))
            {
               return sr.ReadToEnd();
            }    
        }
    
        static void Main(string[] args)
        {
            Console.WriteLine($"Hello! I am {botClient.GetMeAsync().Result.FirstName}");
        }
           
    }
}
