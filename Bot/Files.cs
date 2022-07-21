using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using static System.Net.WebRequestMethods;

namespace Bot
{
    internal class Files
    {
   
        /// <summary>
        /// Выводит список файлов
        /// </summary>
        /// <param name="botClient"></param>
        /// <param name="message"></param>
        public async void ShowFiles (ITelegramBotClient botClient, Update update)
        {
            var message = update.Message;
            string [] fileName = Directory.GetFiles($"D:\\downloaded");
            string showFile = default;
            for (int i = 0; i < fileName.Length; i++)
            {
                showFile += Path.GetFileName(fileName[i]) + "\n";
            }
            await botClient.SendTextMessageAsync(message.Chat, $"{showFile}");
            return;
        }
        /// <summary>
        /// Загрузка фотографии
        /// </summary>
        /// <param name="botClient"></param>
        /// <param name="update"></param>
        public async void DownloadFilePhoto (ITelegramBotClient botClient, Update update)
        {     
            var fileId = update.Message.Photo.Last().FileId;
            var fileInfo = await botClient.GetFileAsync (fileId);
            var filePath = fileInfo.FilePath;
            if (!System.IO.Directory.Exists ($"D:\\downloaded")) Directory.CreateDirectory($"D:\\downloaded");
            string destinationFilePath =  $"D:\\downloaded\\{fileId}.jpg";          
            await using FileStream fileStream = System.IO.File.OpenWrite(destinationFilePath);
            await botClient.DownloadFileAsync(
                filePath: filePath,
                destination: fileStream);
        }
        /// <summary>
        /// Загрузка документов
        /// </summary>
        /// <param name="botClient"></param>
        /// <param name="update"></param>
        public async void DownloadFileDocument (ITelegramBotClient botClient, Update update)
        {
            var fileName = update.Message.Document.FileName;
            var fileId = update.Message.Document.FileId;
            var fileInfo = await botClient.GetFileAsync(fileId);
            var filePath = fileInfo.FilePath;
            string destinationFilePath = $"D:\\downloaded\\{fileName}";
            await using FileStream fileStream = System.IO.File.OpenWrite(destinationFilePath);
            await botClient.DownloadFileAsync(
                filePath: filePath,
                destination: fileStream);
        }

    }
}
