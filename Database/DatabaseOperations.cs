using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

		public static void AddToDatabase(Copypasta copypasta)
		{
			//First check if this pasta is already there (title doesn't matter because it isn't key anyway, it can appear multiple times)

			using (var db = new CopypastaContext())
			{
				var query = from c in db.Copypastas where c.Content.Contains(copypasta.Content) select c;
				if (query.Any())
				{
					//this copypasta has already been added
					return;
				}

				db.Copypastas.Add(copypasta);
				db.SaveChanges();
			}
		}

		public static List<Copypasta> WritePastasToList()
		{
			var lst = new List<Copypasta>();

			using (var db = new CopypastaContext())
			{
				var query = from c in db.Copypastas select c;
				foreach (var item in query)
				{
					lst.Add(item);
				}
			}

			return lst;
		}

		// Returns the amount of pastas added
		public static int UpdateDatabaseFromList(List<Copypasta> copypastas)
		{
			int amount = 0;
			using (var db = new CopypastaContext())
			{
				for (int i = 0; i < copypastas.Count; i++)
				{
					//first check if the copypasta is not blacklisted
					if (Blacklist.BlacklistedMessages.Contains(copypastas[i].Content))
					{
						continue;
					}

					//check for copypastas that have already are in the database
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
						amount++;
					}
				}

				db.SaveChanges();
			}

			return amount;
		}

		// Updates a given Copypasta - we assume the Id stays the same and other parameters can be chagned
		public static void UpdateCopypasta(Copypasta newPasta)
		{
			using (var db = new CopypastaContext())
			{
				var result = (from c in db.Copypastas where c.Id == newPasta.Id select c).SingleOrDefault();
				if (result != null)
				{
					result.Title = newPasta.Title;
					result.Content = newPasta.Content;
					result.DateAdded = newPasta.DateAdded;
					result.IsFavourite = newPasta.IsFavourite;
					result.ChannelFrom = newPasta.ChannelFrom;

					db.SaveChanges();
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
				count = (from c in db.Copypastas where c.Title == null select c).Count();
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