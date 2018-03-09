namespace database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IntermediateTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MovieCountries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MovieId = c.Int(nullable: false),
                        CountryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MovieGenres",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MovieId = c.Int(nullable: false),
                        GenreId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.MovieGenres");
            DropTable("dbo.MovieCountries");
        }
    }
}
