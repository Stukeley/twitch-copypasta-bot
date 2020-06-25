﻿using System;
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
		// Not included in the solution - contains the sensitive bot info (like password and API key)
		private string BotInfoPath = @"C:\Programowanie\Stukeley\twitch-copypasta-bot\Bot\BotInfo.txt";

		// Channel name to join
		private string ChannelName = "Overpow";

		private static Logger _logger = new Logger("TwitchCopypastaBot Logs " + DateTime.Now.ToString());
		private static int _logId = 1;

		// How many messages before evaluating
		private const int MaxCapacity = 200;

		// Minimum count of duplicates for a message to be considered a "pasta"
		private const int MinCount = 15;

		private ConnectionCredentials _credentials;

		private TwitchClient _client;

		// Evaluated copypastas - no given size
		private static List<Copypasta> _copypastas;

		// All messages
		private List<string> _allMessages;

		public TwitchChatBot()
		{
		}

		//Once _allMessages reaches MaxCapacity messages, evaluate and clear it
		private void EvaluateAllMessages()
		{
			int currentCount = 0;

			for (int i = 0; i < _allMessages.Count; i++)
			{
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
					var pasta = new Copypasta()
					{
						Content = currentMessage,
						DateAdded = DateTime.Now
					};

					_copypastas.Add(pasta);
				}
			}
		}

		public void Connect()
		{
			_logger.Log(_logId++, "Connecting", DateTime.Now);

			string BotUsername, BotToken;

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
			_client.Initialize(_credentials, "overpow");

			_client.OnLog += this._client_OnLog;
			_client.OnJoinedChannel += this._client_OnJoinedChannel;
			_client.OnMessageReceived += this._client_OnMessageReceived;
			_client.OnConnected += this._client_OnConnected;

			_allMessages = new List<string>();
			_copypastas = new List<Copypasta>();

			_client.Connect();
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

			if (_allMessages.Count >= 200)
			{
				EvaluateAllMessages();
			}
		}

		// Disconnect the bot, leave the channel and update the database.
		public void Disconnect()
		{
			_logger.Log(_logId++, "Disconnecting", DateTime.Now);
			DatabaseOperations.UpdateDatabaseFromList(_copypastas);
			_client.Disconnect();
		}
	}
}