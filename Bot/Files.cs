using System;
using System.IO;
using System.Linq;
using Telegram.Bot;
using Telegram.Bot.Types;

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
            int count = 1;
            string fileName = "image";
            DateTime dateTime = DateTime.Now;
            string dateTimeFile = dateTime.ToShortDateString().ToString();
            var fileId = update.Message.Photo.Last().FileId;      
            string destinationFilePath =  default;
            while(System.IO.File.Exists($"D:\\downloaded\\{fileName}_{dateTimeFile}_{count}.jpg")) count++;        
            destinationFilePath = $"D:\\downloaded\\{fileName}_{dateTimeFile}_{count}.jpg";          
            DonwloadFile(botClient, fileId, destinationFilePath);
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
            string destinationFilePath = $"D:\\downloaded\\{fileName}";
            DonwloadFile(botClient, fileId, destinationFilePath);
        }

        /// <summary>
        /// Загрузка аудиосообщений
        /// </summary>
        /// <param name="botClient"></param>
        /// <param name="update"></param>
        public async void DownloadFileAudio (ITelegramBotClient botClient, Update update )
        {
            int count = 1;
            string fileName = "voice";
            DateTime dateTime = DateTime.Now;
            string dateTimeFile = dateTime.ToShortDateString().ToString();
            var fileId = update.Message.Voice.FileId;
            string destinationFilePath = default;
            while (System.IO.File.Exists($"D:\\downloaded\\{fileName}_{dateTimeFile}_{count}.mp3")) count++;
            destinationFilePath = $"D:\\downloaded\\{fileName}_{dateTimeFile}_{count}.mp3";
            DonwloadFile(botClient, fileId, destinationFilePath);
        }

        /// <summary>
        /// Загрузка на сервер
        /// </summary>
        /// <param name="botClient"></param>
        /// <param name="fileId"></param>
        /// <param name="fileName"></param>
        /// <param name="destinationFilePath"></param>
        private async void DonwloadFile(ITelegramBotClient botClient, string fileId,string destinationFilePath)
        {
            if (!System.IO.Directory.Exists($"D:\\downloaded")) Directory.CreateDirectory($"D:\\downloaded");
            var fileInfo = await botClient.GetFileAsync(fileId);
            var filePath = fileInfo.FilePath;          
            await using FileStream fileStream = System.IO.File.OpenWrite(destinationFilePath);
            await botClient.DownloadFileAsync(
                filePath: filePath,
                destination: fileStream);
        }
        /// <summary>
        /// Отправка файла пользователю
        /// </summary>
        /// <param name="botClient"></param>
        /// <param name="update"></param>
        public async void UploadFile(ITelegramBotClient botClient, Update update)
        {
           var message = update.Message;
           await using Stream stream = System.IO.File.OpenRead($"D:\\downloaded\\{message.Text}");
           await botClient.SendDocumentAsync(chatId: message.Chat, document: new Telegram.Bot.Types.InputFiles.InputOnlineFile(content: stream, fileName: message.Text));                        
        }

    }
}
