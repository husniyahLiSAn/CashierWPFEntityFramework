namespace B32.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddModelTrans : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tb_m_item",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Stock = c.Int(nullable: false),
                        Price = c.Int(nullable: false),
                        SupplierFK_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tb_m_supplier", t => t.SupplierFK_Id)
                .Index(t => t.SupplierFK_Id);
            
            CreateTable(
                "dbo.tb_m_supplier",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Email = c.String(),
                        CreateDate = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.tb_m_role",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.tb_m_transactionitem",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Quantity = c.Int(nullable: false),
                        SubTotal = c.Int(nullable: false),
                        ItemFK_Id = c.Int(),
                        TransactionFK_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tb_m_item", t => t.ItemFK_Id)
                .ForeignKey("dbo.tb_m_transaction", t => t.TransactionFK_Id)
                .Index(t => t.ItemFK_Id)
                .Index(t => t.TransactionFK_Id);
            
            CreateTable(
                "dbo.tb_m_transaction",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Total = c.Int(nullable: false),
                        Pay = c.Int(nullable: false),
                        OrderDate = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.tb_m_user",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Email = c.String(),
                        Password = c.String(),
                        ChangePassword = c.String(),
                        CreateDate = c.DateTimeOffset(nullable: false, precision: 7),
                        Role_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tb_m_role", t => t.Role_Id)
                .Index(t => t.Role_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tb_m_user", "Role_Id", "dbo.tb_m_role");
            DropForeignKey("dbo.tb_m_transactionitem", "TransactionFK_Id", "dbo.tb_m_transaction");
            DropForeignKey("dbo.tb_m_transactionitem", "ItemFK_Id", "dbo.tb_m_item");
            DropForeignKey("dbo.tb_m_item", "SupplierFK_Id", "dbo.tb_m_supplier");
            DropIndex("dbo.tb_m_user", new[] { "Role_Id" });
            DropIndex("dbo.tb_m_transactionitem", new[] { "TransactionFK_Id" });
            DropIndex("dbo.tb_m_transactionitem", new[] { "ItemFK_Id" });
            DropIndex("dbo.tb_m_item", new[] { "SupplierFK_Id" });
            DropTable("dbo.tb_m_user");
            DropTable("dbo.tb_m_transaction");
            DropTable("dbo.tb_m_transactionitem");
            DropTable("dbo.tb_m_role");
            DropTable("dbo.tb_m_supplier");
            DropTable("dbo.tb_m_item");
        }
    }
}
