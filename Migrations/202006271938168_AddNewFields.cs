namespace TwitchCopypastaBot.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNewFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Copypastas", "ChannelFrom", c => c.String());
            AddColumn("dbo.Copypastas", "IsFavourite", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Copypastas", "IsFavourite");
            DropColumn("dbo.Copypastas", "ChannelFrom");
        }
    }
}
