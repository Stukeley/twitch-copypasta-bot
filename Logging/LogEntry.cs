using System;

namespace TwitchCopypastaBot.Logging
{
	internal class LogEntry
	{
		public int Id { get; internal set; }
		public string Message { get; internal set; }
		public DateTime CreatedTime { get; internal set; }
	}
}