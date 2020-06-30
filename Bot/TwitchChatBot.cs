using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using TwitchCopypastaBot.Logging;
using TwitchCopypastaBot.Database;
using TwitchCopypastaBot.Models;
using TwitchLib.Client;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Clients;
using TwitchLib.Communication.Models;

namespace TwitchCopypastaBot.Bot
{
	internal class TwitchChatBot
	{
		//! SETUP

		// Not included in the solution - contains the sensitive bot info (username and API key)
		// Please never share these anywhere! Keep them out of the solution
		public static string BotInfoPath = @"C:\Programowanie\Stukeley\twitch-copypasta-bot\Bot\BotInfo.txt";

		// Channel name to join
		public static string ChannelName = "Overpow";

		// How many messages before evaluating
		public static int MaxCapacity = 100;

		// Minimum count of duplicates for a message to be considered a "pasta"
		public static int MinCount = 5;

		//! END SETUP

		//? For other classes

		public bool IsActive = false;
		public DateTime DateStarted;

		//? End for other classes

		//? Singleton

		private static TwitchChatBot _instance = null;

		public static TwitchChatBot Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new TwitchChatBot();
				}
				return _instance;
			}
		}

		//? End singleton

		private static Logger _logger;
		private static int _logId = 1;

		private ConnectionCredentials _credentials;

		private TwitchClient _client;

		// All messages
		private List<string> _allMessages;

		// Evaluated copypastas - no given size
		private static List<Copypasta> _copypastas;

		static TwitchChatBot()
		{
			string fileName = "TwitchCopypastaBot Log" + DateTime.Now.ToString().Replace(':', '.') + ".txt";

			Titles.CurrentLogFileName = fileName;

			_logger = new Logger(fileName);
		}

		public TwitchChatBot()
		{
		}

		//Once _allMessages reaches MaxCapacity messages, evaluate and clear it
		private void EvaluateAllMessages()
		{
			for (int i = 0; i < _allMessages.Count; i++)
			{
				int currentCount = 0;
				var currentMessage = _allMessages[i];

				for (int j = i + 1; j < _allMessages.Count; j++)
				{
					if (_allMessages[j].Contains(currentMessage))
					{
						currentCount++;
						_allMessages.RemoveAt(j);
						j--;
					}
				}

				if (currentCount >= MinCount)
				{
					//check if the message is not blacklisted
					//! important note - only messages *exactly the same* as the blacklisted will not get added
					//! this means that a message having other content than just these blacklisted (emotes?), or a combination of blacklisted keywords, will get added

					if (Blacklist.BlacklistedMessages.Contains(currentMessage))
					{
						continue;
					}

					var pasta = new Copypasta()
					{
						Content = currentMessage,
						DateAdded = DateTime.Now
					};

					_copypastas.Add(pasta);
				}
			}

			_allMessages.Clear();
		}

		public void Connect()
		{
			_logger.Log(_logId++, "Connecting", DateTime.Now);

			string BotUsername, BotToken;

			//	Username
			//	AccessToken
			//	End of file

			using (var reader = new StreamReader(BotInfoPath))
			{
				BotUsername = reader.ReadLine();
				BotToken = reader.ReadLine();
			}

			_credentials = new ConnectionCredentials(BotUsername, BotToken);


			var clientOptions = new ClientOptions()
			{
				MessagesAllowedInPeriod = 75,
				ThrottlingPeriod = TimeSpan.FromSeconds(30)
			};
			var customClient = new WebSocketClient(clientOptions);

			_client = new TwitchClient(customClient);
			_client.Initialize(_credentials, ChannelName);

			_client.OnLog += this._client_OnLog;
			_client.OnJoinedChannel += this._client_OnJoinedChannel;
			_client.OnMessageReceived += this._client_OnMessageReceived;
			_client.OnConnected += this._client_OnConnected;

			_allMessages = new List<string>();
			_copypastas = new List<Copypasta>();

			_client.Connect();

			IsActive = true;
			DateStarted = DateTime.Now;
		}

		private void _client_OnConnected(object sender, TwitchLib.Client.Events.OnConnectedArgs e)
		{
			_logger.Log(_logId++, "Bot connected", DateTime.Now);
		}

		private void _client_OnJoinedChannel(object sender, TwitchLib.Client.Events.OnJoinedChannelArgs e)
		{
			_logger.Log(_logId++, $"Joined channel {e.Channel}", DateTime.Now);
		}

		private void _client_OnLog(object sender, TwitchLib.Client.Events.OnLogArgs e)
		{
			_logger.Log(_logId++, e.Data, e.DateTime);
		}

		// Register a message in the All Messages List
		private void _client_OnMessageReceived(object sender, TwitchLib.Client.Events.OnMessageReceivedArgs e)
		{
			// Remove useless characters at the start and end
			var message = e.ChatMessage.Message.Trim();

			_allMessages.Add(message);

			if (_allMessages.Count >= MaxCapacity)
			{
				EvaluateAllMessages();
			}
		}

		// Disconnect the bot, leave the channel, force evaluation and update the database.
		public void Disconnect()
		{
			_logger.Log(_logId++, "Disconnecting", DateTime.Now);

			EvaluateAllMessages();

			int added = 0;

			if (_copypastas.Count > 0)
			{
				// Update the database
				added = DatabaseOperations.UpdateDatabaseFromList(_copypastas);
			}

			_logger.Log(_logId++, $"Added {added} new copypastas", DateTime.Now);

			_copypastas.Clear();

			_client.Disconnect();
			IsActive = false;
		}
	}
}