using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
			var directory = Path.GetDirectoryName(_fileName);
			if (!Directory.Exists(directory))
			{
				Directory.CreateDirectory(directory);
			}

			try
			{
				File.AppendAllText(_fileName, $"{log.CreatedTime} {log.Id} {log.Message}" + Environment.NewLine);
			}
			catch (Exception)
			{
			}
		}

		public void Log(int id, string message, DateTime createdTime)
		{
			if (string.IsNullOrEmpty(message) || string.IsNullOrEmpty(_fileName))
			{
				return;
			}

			var logEntry = new LogEntry() { Id = id, Message = message, CreatedTime = createdTime };

			InsertLog(logEntry);
		}
	}
}