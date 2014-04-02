namespace BlackMesa.MyStack.Main.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addedcultureattributetouserentity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Identity_Users", "Culture", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Identity_Users", "Culture");
        }
    }
}
