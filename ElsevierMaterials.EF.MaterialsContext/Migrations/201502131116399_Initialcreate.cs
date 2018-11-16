namespace ElsevierMaterials.EF.MaterialsContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initialcreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Classification",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LevelId = c.Int(nullable: false),
                        ParentId = c.Int(),
                        Name = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Material",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MaterialId = c.Int(nullable: false),
                        SubgroupId = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 100),
                        SourceText = c.String(maxLength: 100),
                        Standard = c.String(maxLength: 100),
                        Specification = c.String(maxLength: 100),
                        SourceId = c.Int(nullable: false),
                        ClassificationId = c.Int(nullable: false),
                        EquivalenceId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Classification", t => t.ClassificationId, cascadeDelete: true)
                .ForeignKey("dbo.Source", t => t.SourceId, cascadeDelete: true)
                .Index(t => t.SourceId)
                .Index(t => t.ClassificationId);
            
            CreateTable(
                "dbo.Source",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        ServiceName = c.String(nullable: false, maxLength: 100),
                        ServiceType = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SampleMaterial",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        ClassificationId = c.Int(nullable: false),
                        EquivalenceId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Classification", t => t.ClassificationId, cascadeDelete: true)
                .Index(t => t.ClassificationId);
            
            CreateTable(
                "dbo.FullTextSearch",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EquivalenceId = c.Int(nullable: false),
                        SearchText = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SampleMaterial", "ClassificationId", "dbo.Classification");
            DropForeignKey("dbo.Material", "SourceId", "dbo.Source");
            DropForeignKey("dbo.Material", "ClassificationId", "dbo.Classification");
            DropIndex("dbo.SampleMaterial", new[] { "ClassificationId" });
            DropIndex("dbo.Material", new[] { "ClassificationId" });
            DropIndex("dbo.Material", new[] { "SourceId" });
            DropTable("dbo.FullTextSearch");
            DropTable("dbo.SampleMaterial");
            DropTable("dbo.Source");
            DropTable("dbo.Material");
            DropTable("dbo.Classification");
        }
    }
}
