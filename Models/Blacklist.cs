using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchCopypastaBot.Models
{
	// This class contains a list of blacklisted messages/keywords (such as Twitch emotes) - these won't be noted as "copypastas"
	// Each line is one blacklisted message, emote, keyword, etc.
	// Only messages containing these AND NOTHING MORE will be filtered out
	// TODO: store this in a database too - insert each keyword once
	static class Blacklist
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
