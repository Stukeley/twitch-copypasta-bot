using System;
using System.IO;

namespace TwitchCopypastaBot.Logging
{
	internal class Logger
	{
		private readonly string _fileName;

		public Logger(string fileName)
		{
			_fileName = fileName;
		}

		private void InsertLog(LogEntry log)
		{
			if (!Directory.Exists(Models.Titles.LogsDirectoryName))
			{
				Directory.CreateDirectory(Models.Titles.LogsDirectoryName);
			}

			string filePath = Path.Combine(Models.Titles.LogsDirectoryName, _fileName);

			try
			{
				File.AppendAllText(filePath, $"{log.Id} {log.CreatedTime} {log.Message}" + Environment.NewLine);
			}
			catch (Exception)
			{
				//how to log an exception that occured during logging? xd
			}
		}

		public void Log(int id, string message, DateTime createdTime)
		{
			if (string.IsNullOrEmpty(message) || string.IsNullOrEmpty(Models.Titles.LogsDirectoryName) || string.IsNullOrEmpty(_fileName))
			{
				return;
			}

			var logEntry = new LogEntry() { Id = id, Message = message, CreatedTime = createdTime };

			InsertLog(logEntry);
		}
	}
}