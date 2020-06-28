using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Diagnostics.PerformanceData;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;
using TwitchCopypastaBot.Bot;
using TwitchCopypastaBot.Models;

namespace TwitchCopypastaBot.Database
{
	internal static class DatabaseOperations
	{
		public static void WritePastasToTextFile(string filePath)
		{
			//if (!Directory.Exists(filePath))
			//{
			//	Directory.CreateDirectory(filePath);
			//}

			using (var db = new CopypastaContext())
			{
				if (db.Copypastas.Count() == 0)
				{
					File.AppendAllText(filePath, $"Brak copypast w bazie danych!");
				}
				else
				{
					var query = from c in db.Copypastas select c;
					foreach (var item in query)
					{
						File.AppendAllText(filePath, $"{item.Id} {item.Title} {item.Content}" + Environment.NewLine);
					}
				}
			}
		}

		//! WARNING - DEBUG ONLY
		public static void ClearDatabase()
		{
			using (var db = new CopypastaContext())
			{
				db.Copypastas.RemoveRange(db.Copypastas);
				db.SaveChanges();
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

		public static void UpdateDatabaseFromList(List<Copypasta> copypastas)
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
						if (dbCopypasta.Content == copypastas[i].Content)
						{
							found = true;
							break;
						}
					}

					if (!found)
					{
						db.Copypastas.Add(copypastas[i]);
					}
				}

				db.SaveChanges();
			}
		}

		public static void UpdateTitle(List<Copypasta> copypastas, string title, string content)
		{
			//check if the given pasta exists, then add a title to it (in the static list of copypastas), then update database

			for (int i = 0; i < copypastas.Count; i++)
			{
				if (copypastas[i].Content == content)
				{
					var updated = new Copypasta()
					{
						Title = title,
						Content = content
					};
					copypastas[i] = updated;
					UpdateDatabaseFromList(copypastas);
					break;
				}
			}
		}

		public static int GetCopypastaCount()
		{
			// Get the total count of copypastas currently in the database
			int count;

			using (var db = new CopypastaContext())
			{
				count = (from c in db.Copypastas select c).Count();
			}

			return count;
		}

		public static int GetUnnamedCopypastaCount()
		{
			// Get the amount of copypastas in the database that do not have a title
			int count;

			using (var db = new CopypastaContext())
			{
				count = (from c in db.Copypastas where c.Title == "" select c).Count();
			}

			return count;
		}

		public static DateTime GetLastCopypastaDate()
		{
			// Returns the DateTime for the most recently added copypasta
			DateTime date;

			using (var db = new CopypastaContext())
			{
				var latest = (from c in db.Copypastas orderby c.DateAdded descending select c).FirstOrDefault();

				// TODO: temporary fix, change later
				if (latest == default)
				{
					return DateTime.Now;
				}

				date = latest.DateAdded;
			}

			return date;
		}
	}
}