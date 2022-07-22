using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;


namespace Bot
{
    internal class Program
    {
        static string token;
        
        static TelegramBotClient botClient = new TelegramBotClient(GetToken(token));

        static Files file = new Files();

        static string GetToken(string token)
        {
            using (StreamReader sr = new StreamReader("token.txt"))
            {
               return sr.ReadToEnd();
            }    
        }
        static async Task UpdateMessage (ITelegramBotClient botClient, Update update)
        {            
            var message = update.Message;
            if (message.Text.ToLower() == "/start")
            {
                await botClient.SendTextMessageAsync(message.Chat, "Добро пожаловать, я бот Хранитель, ознакомиться со списком команд можно набрав /help");
                return;
            }

            else if (message.Text.ToLower() == "/help")
            {
                await botClient.SendTextMessageAsync(message.Chat, "Список команд:" +
                    "\n/show - показать список сохранённых файлов" +
                    "\nДля скачивания файла введит его имя и расширение");
                return;
            }
            
            else if  (message.Text.ToLower() == "/show")
            {
                file.ShowFiles(botClient, update);
                return;
            }

            else if (System.IO.File.Exists($"D:\\downloaded\\{message.Text}"))
            {            
                file.UploadFile(botClient, update);
                return;
            }

            await botClient.SendTextMessageAsync(message.Chat, "Неизвестная команда");
        }
        public static async Task UpdateAsync (ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {            
            if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message && update.Message.Text != null)
            {
                UpdateMessage(botClient, update);
                return;
            }

            if (update.Message.Type == Telegram.Bot.Types.Enums.MessageType.Photo)
            {
                file.DownloadFilePhoto(botClient, update);
                return;
            }

            if (update.Message.Type == Telegram.Bot.Types.Enums.MessageType.Document)
            {
                file.DownloadFileDocument(botClient, update);
                return;
            }

            if (update.Message.Type == Telegram.Bot.Types.Enums.MessageType.Voice)
            {
                file.DownloadFileAudio(botClient, update);
                return;
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
