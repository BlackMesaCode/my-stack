namespace BlackMesa.MyStack.Main.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedStringLengthcontraintforthefoldernameattribute : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.MyStack_Folders", "Name", c => c.String(nullable: false, maxLength: 50));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.MyStack_Folders", "Name", c => c.String(nullable: false));
        }
    }
}
