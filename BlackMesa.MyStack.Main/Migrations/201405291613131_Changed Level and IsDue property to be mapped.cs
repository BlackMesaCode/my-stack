namespace BlackMesa.MyStack.Main.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedLevelandIsDuepropertytobemapped : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MyStack_Cards", "Level", c => c.Int(nullable: false));
            AddColumn("dbo.MyStack_Cards", "IsDue", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.MyStack_Cards", "IsDue");
            DropColumn("dbo.MyStack_Cards", "Level");
        }
    }
}
