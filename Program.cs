using Telegram.Bot.Polling;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using System;
using System.Runtime.CompilerServices;

internal class Program
{
    private static void Main(string[] args)
    {
        var client = new TelegramBotClient("7098327735:AAGyF-7GIkYFbXlwwXNZCSLsUN7LbG5TAL4");
        client.StartReceiving(UpdateHandler, Error); /*метод, который выводит бот*/
        Console.ReadLine();
    }
    private static async Task MessageHeandler(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        switch (update.Type)
        {
            case UpdateType.Message:
                {
                    var message = update.Message;
                    var user = message.From;
                    var chat = message.Chat;

                    switch (message.Type)
                    {
                        case MessageType.Text:
                            {
                                if (message.Text == null)
                                {
                                    return;

                                }
                                if (message.Text == "/start")
                                {
                                    //создание кнопок в строке
                                    InlineKeyboardMarkup inlineKeyboard = new(new[]
                                    {
                                        new []
                                        {
                                            InlineKeyboardButton.WithCallbackData(text: "МЕНЮ", callbackData: "меню"),
                                        }
                                       
                                    });

                                    Message sentMessage = await botClient.SendTextMessageAsync(
                                        chatId: chat.Id,
                                        text: "A message with an inline keyboard markup",
                                        replyMarkup: inlineKeyboard,
                                        cancellationToken: cancellationToken);

                                    return;
                                }
                                return;
                            }
                    }
                    return;
                }
        }

    }
    private static async Task CallBack(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        //вывод в зависимости от выбранной кнопки
        if (update != null && update.CallbackQuery != null)
        {
            string answer = update.CallbackQuery.Data;
            long chatid = update.CallbackQuery.Message.Chat.Id;
            switch (answer)
            {
                case "меню":
                    InlineKeyboardMarkup inlineKeyboard = new(new[]
                    {
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData(text: "КАТАЛОГ", callbackData: "каталог"),
                    },
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData(text: "КОНТАКТЫ", callbackData: "контакты"),
                    },
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData(text: "МЕСТОПОЛОЖЕНИЕ", callbackData: "местоположение"),
                    },
                    });
                   await botClient.SendTextMessageAsync(
                                       chatId: chatid,
                                       text: "A message with an inline keyboard markup",
                                       replyMarkup: inlineKeyboard,
                                       cancellationToken: cancellationToken);
                break; 

                case "каталог":
                       await botClient.SendPhotoAsync(
                          chatId: chatid,
                          photo: InputFile.FromUri("https://github.com/TelegramBots/book/raw/master/src/docs/photo-ara.jpg"),
                          caption: "<b>Ara bird</b>. <i>Source</i>: <a href=\"https://pixabay.com\">Pixabay</a>",
                          parseMode: ParseMode.Html,
                          cancellationToken: cancellationToken);
                break;

                case "контакты":
                     await botClient.SendContactAsync(
                          chatId: chatid,
                          phoneNumber: "+1234567890",
                          firstName: "Han",
                          lastName: "Solo",
                          cancellationToken: cancellationToken);

                break;

                case "местоположение":
                     await botClient.SendLocationAsync(
                          chatId: chatid,
                         latitude: 33.747252f,
                         longitude: -112.633853f,
                         cancellationToken: cancellationToken);

                break;

            }


        }


            //InlineKeyboardMarkup inlineKeyboard = update.CallbackQuery.Message.ReplyMarkup!;
            //var inlines = inlineKeyboard.InlineKeyboard;
            //foreach (var item1 in inlines)
            //{
            //    foreach (var item2 in item1)
            //    {

            //       
            //    }
            //}
    }
  

    private static Task Error(ITelegramBotClient arg1, Exception arg2, CancellationToken arg3)
    {
        throw new NotImplementedException();
    }
    private static async Task UpdateHandler(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        await CallBack(botClient, update, cancellationToken);
        await MessageHeandler(botClient, update, cancellationToken);

    }
}