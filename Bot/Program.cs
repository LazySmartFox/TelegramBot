using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;


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
        public static async Task UpdateAsync (ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
            {
                var message = update.Message;
                if (message.Text.ToLower() == "/start")
                {
                    await botClient.SendTextMessageAsync(message.Chat, "Добро пожаловать, я бот Хранитель, ознакомиться со списком команд можно набрав /help");
                    return;
                }
                if (message.Text.ToLower() == "/help")
                {
                    await botClient.SendTextMessageAsync(message.Chat, "Список команд:" +
                        "\n/showAll - показать список сохранённых файлов" +
                        "\n/download {имя файла} - скачать файл");
                }
                await botClient.SendTextMessageAsync(message.Chat, "Неизвестная команда");
            }

        }
        public static async Task ErrorAsync (ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
        }
    
        static void Main(string[] args)
        {
            Console.WriteLine($"Hello! I am {botClient.GetMeAsync().Result.FirstName}");
            var cts = new CancellationTokenSource();
            var cancellationToken = cts.Token;
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = { },
            };
            botClient.StartReceiving(
                UpdateAsync,
                ErrorAsync,
                receiverOptions,
                cancellationToken
                );
            Console.ReadLine();
            
            
        }
           
    }
}
