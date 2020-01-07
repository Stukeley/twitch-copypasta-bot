using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using TwitchCopypastaBot.Logging;
using TwitchCopypastaBot.Database;
using TwitchLib.Client;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Clients;
using TwitchLib.Communication.Models;

namespace TwitchCopypastaBot.Bot
{
	internal class TwitchChatBot
	{
		private static Logger _logger = new Logger("TwitchCopypastaBot Logs " + DateTime.Now.ToString());
		private static int _logId = 1;
		private const int MaxCapacity = 200;
		private const int MinCount = 15;

		private readonly ConnectionCredentials _credentials = new ConnectionCredentials(BotInfo.BotUsername, BotInfo.BotToken);

		private TwitchClient _client;

		private static List<Tuple<string, string>> _copypastas;
		private List<string> _allMessages;

		public TwitchChatBot()
		{
		}

		//Once _allMessages reaches MaxCapacity messages, evaluate and clear it
		private void EvaluateAllMessages()
		{
			for (int i = 0; i < _allMessages.Count; i++)
			{
				int count = 1;
				for (int j = i; j < _allMessages.Count;)
				{
					if (_allMessages[j].Contains(_allMessages[i]))
					{
						count++;
						_allMessages.RemoveAt(j);
					}
					else
					{
						j++;
					}
				}

				if (count >= MinCount)
				{
					_copypastas.Add(new Tuple<string, string>("", _allMessages[i]));
				}
			}

			_allMessages.Clear();
		}

		public void Connect()
		{
			_logger.Log(_logId++, "Connecting", DateTime.Now);

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
			_copypastas = new List<Tuple<string, string>>();

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

		private void _client_OnMessageReceived(object sender, TwitchLib.Client.Events.OnMessageReceivedArgs e)
		{
			var message = e.ChatMessage.Message;

			_allMessages.Add(message);

			if (_allMessages.Count >= MaxCapacity)
			{
				EvaluateAllMessages();
			}
		}

		public void Disconnect()
		{
			_logger.Log(_logId++, "Disconnecting", DateTime.Now);
			DatabaseOperations.UpdateDatabaseFromList(_copypastas);
			_client.Disconnect();
		}
	}
}