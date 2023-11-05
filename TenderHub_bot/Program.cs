using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TenderHub_bot;

namespace TelegramBotExperiments
{
	class Program
	{
		static List<Card> cards = new List<Card>();
		static bool isReview = false;
		static ITelegramBotClient bot = new TelegramBotClient("6318044476:AAHK7XujdXxeKawqfEFREeA2ioDRTrHcwy4");

		public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
		{
			Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(update));

			if (update.Type == UpdateType.Message)
			{
				var message = update.Message;
				string response = "";

				switch (message.Text.ToLower())
				{
					case "/start":
						response = "Добро пожаловать на TenderHub!❤\nДанный бот представляет собой Осведомителя.📢\nПодпишитесь на категорию вашей деятельности чтобы получать уведомление о новых тендерах.";
						break;
					case "/ztovar":
						response = "Вы успешно подписались на обновления по закупке товаров!👍\nИспользуйте /showtovar для просмотра последних <b>Закупок товаров</b>";
						break;
					case "/services":
						response = "Вы успешно подписались на обновления в сфере услуг!👍\nИспользуйте /showserv для просмотра последних <b>Закупок услуг</b>";
						break;
					case "/showserv":
						response = GetProcurementDetails("Услуги");
						break;
					case "/showtovar":
						response = GetProcurementDetails("Товары");
						break;
					case "/review":
						response = "Опишите свою проблему, или оставьте отзыв";
						isReview = true;
						break;
					default:
						if (isReview)
						{
							response = "Спасибо за ваш отзыв, мы обязательно его обработаем!";
							isReview = false;

							DBConnect.UpdateDBTableReviews(update.Id.ToString() , message.Text.ToLower());
						}
						else
						{
							response = "Неопознанная команда!";
						}
						break;
				}

				await botClient.SendTextMessageAsync(message.Chat, response, parseMode: ParseMode.Html);
			}

			string GetProcurementDetails(string procurementType)
			{
				var relevantItems = cards.Where(item => item.ProcurementType == procurementType);

				if (relevantItems.Any())
				{
					var response = string.Join("\n", relevantItems.Select(item =>
						"\n" +
						$"<b>Название : </b> {item.OrganizationName}\n" +
						$"<b>Описание : </b> {item.Description}\n" +
						$"<b>Вид закупок : </b> {item.ProcurementType}\n" +
						$"<b>Наименование закупки : </b> {item.PurchaseName}\n" +
						$"<b>Сумма : </b> {item.Summa}\n" +
						$"<b>Дата опубликования : </b> {item.PublicationDate}\n" +
						$"<b>Дедлайн : </b> {item.Deadline}"
					));
					return response;
				}
				return "Нет доступных данных о закупках в данной категории.";
			}
		}
		public static async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
		{
			// Некоторые действия
			Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
		}
		static void Main(string[] args)
		{
			Console.WriteLine("Запущен бот " + bot.GetMeAsync().Result.FirstName);
			cards = DBConnect.GetDBTable();

			var cts = new CancellationTokenSource();
			var cancellationToken = cts.Token;
			var receiverOptions = new ReceiverOptions
			{
				AllowedUpdates = { },
			};

			bot.StartReceiving(
				HandleUpdateAsync,
				HandleErrorAsync,
				receiverOptions,
				cancellationToken
			);

			Timer();
			Console.ReadLine();

		}
		static async void Timer()
		{
			while (true)
			{
				Console.WriteLine("Таблица обновлена");
				Thread.Sleep(150000);
				cards = DBConnect.GetDBTable();
			}
		}
	}
}