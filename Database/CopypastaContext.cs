using System.Data.Entity;
using TwitchCopypastaBot.Models;

namespace TwitchCopypastaBot.Database
{
	internal class CopypastaContext : DbContext
	{
		public DbSet<Copypasta> Copypastas { get; set; }
	}
}