using System.Collections.Generic;
using System.IO;

namespace TwitchCopypastaBot.Models
{
	// This class contains a list of blacklisted messages/keywords (such as Twitch emotes) - these won't be noted as "copypastas"
	// Each line is one blacklisted message, emote, keyword, etc.
	// Only messages containing these AND NOTHING MORE will be filtered out
	internal static class Blacklist
	{
		public static List<string> BlacklistedMessages = new List<string>();

		static Blacklist()
		{
			using (var reader = new StreamReader(@"Blacklisted.txt"))
			{
				while (!reader.EndOfStream)
				{
					BlacklistedMessages.Add(reader.ReadLine());
				}
			}
		}
	}
}
