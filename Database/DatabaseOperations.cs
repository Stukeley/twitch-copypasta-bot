using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using TwitchCopypastaBot.Bot;
using TwitchCopypastaBot.Models;

namespace TwitchCopypastaBot.Database
{
	internal static class DatabaseOperations
	{
		public static void WritePastasToTextFile(string filePath)
		{
			if (!Directory.Exists(filePath))
			{
				Directory.CreateDirectory(filePath);
			}

			using (var db = new CopypastaContext())
			{
				var query = from c in db.Copypastas select c;
				foreach (var item in query)
				{
					File.AppendAllText(filePath, $"{item.Id} {item.Title} {item.Content}");
				}
			}
		}

		public static void AddToDatabase(string title, string content)
		{
			//First check if this pasta is already there (title doesn't matter because it isn't key anyway, it can appear multiple times)

			using (var db = new CopypastaContext())
			{
				var query = from c in db.Copypastas where c.Content.Contains(content) select c;
				if (query.Any())
				{
					//this copypasta has already been added
					return;
				}

				var copypasta = new Copypasta()
				{
					Title = title,
					Content = content,
					DateAdded = DateTime.Now
				};
				db.Copypastas.Add(copypasta);
				db.SaveChanges();
			}
		}

		public static Dictionary<int, Tuple<string, string>> WritePastasToDictionary()
		{
			var dict = new Dictionary<int, Tuple<string, string>>();

			using (var db = new CopypastaContext())
			{
				var query = from c in db.Copypastas select c;
				foreach (var item in query)
				{
					dict.Add(item.Id, new Tuple<string, string>(item.Title, item.Content));
				}
			}

			return dict;
		}

		public static void UpdateDatabaseFromList(List<Tuple<string, string>> copypastas)
		{
			//check for copypastas that have already been added, then add the rest
			using (var db = new CopypastaContext())
			{
				for (int i = 0; i < copypastas.Count; i++)
				{
					var dbCopypastas = db.Copypastas.ToList();
					bool found = false;

					foreach (var dbCopypasta in dbCopypastas)
					{
						if (dbCopypasta.Content == copypastas[i].Item2)
						{
							found = true;
							break;
						}
					}

					if (found)
					{
						db.Copypastas.Add(new Copypasta() { Title = copypastas[i].Item1, Content = copypastas[i].Item2 });
					}
				}

				db.SaveChanges();
			}
		}

		public static void UpdateTitle(List<Tuple<string, string>> copypastas, string title, string content)
		{
			//check if the given pasta exists, then add a title to it (in the static list of copypastas), then update database

			for (int i = 0; i < copypastas.Count; i++)
			{
				if (copypastas[i].Item2 == content)
				{
					var updated = new Tuple<string, string>(title, content);
					copypastas[i] = updated;
					UpdateDatabaseFromList(copypastas);
					break;
				}
			}
		}
	}
}