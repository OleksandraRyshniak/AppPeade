namespace AppPeade.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPyhaAndKylaline : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Kylalines", "PyhaId");
            AddForeignKey("dbo.Kylalines", "PyhaId", "dbo.Pyhas", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Kylalines", "PyhaId", "dbo.Pyhas");
            DropIndex("dbo.Kylalines", new[] { "PyhaId" });
        }
    }
}
