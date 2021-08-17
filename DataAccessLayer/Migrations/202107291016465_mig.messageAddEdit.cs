namespace DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migmessageAddEdit : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Messages", "MessageContent", c => c.String());
            DropColumn("dbo.Messages", "MessageConrent");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Messages", "MessageConrent", c => c.String());
            DropColumn("dbo.Messages", "MessageContent");
        }
    }
}
