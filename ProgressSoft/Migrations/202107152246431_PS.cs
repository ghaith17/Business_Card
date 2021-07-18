namespace ProgressSoft.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PS : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BusinessCards",
                c => new
                    {
                        CustomerId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Gender = c.String(),
                        DOB = c.DateTime(nullable: false),
                        Email = c.String(nullable: false),
                        Phone = c.String(nullable: false),
                        Photo = c.String(),
                        Address = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.CustomerId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.BusinessCards");
        }
    }
}
