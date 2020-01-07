using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchCopypastaBot.Models;

namespace TwitchCopypastaBot.Database
{
	internal class CopypastaContext : DbContext
	{
		public DbSet<Copypasta> Copypastas { get; set; }
	}
}