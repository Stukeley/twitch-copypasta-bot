﻿namespace TwitchCopypastaBot.Migrations
{
	using System.Data.Entity.Migrations;

	internal sealed class Configuration : DbMigrationsConfiguration<TwitchCopypastaBot.Database.CopypastaContext>
	{
		public Configuration()
		{
			AutomaticMigrationsEnabled = false;
			ContextKey = "TwitchCopypastaBot.Database.CopypastaContext";
		}

		protected override void Seed(TwitchCopypastaBot.Database.CopypastaContext context)
		{
			//  This method will be called after migrating to the latest version.

			//  You can use the DbSet<T>.AddOrUpdate() helper extension method
			//  to avoid creating duplicate seed data.
		}
	}
}
