namespace database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Creation : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Countries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Genres",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MovieCountries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MovieId = c.Int(nullable: false),
                        Country_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Countries", t => t.Country_Id)
                .ForeignKey("dbo.Movies", t => t.MovieId, cascadeDelete: true)
                .Index(t => t.MovieId)
                .Index(t => t.Country_Id);
            
            CreateTable(
                "dbo.MovieGenres",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MovieId = c.Int(nullable: false),
                        Genre_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Genres", t => t.Genre_Id)
                .ForeignKey("dbo.Movies", t => t.MovieId, cascadeDelete: true)
                .Index(t => t.MovieId)
                .Index(t => t.Genre_Id);
            
            CreateTable(
                "dbo.Movies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Year = c.Int(nullable: false),
                        Poster = c.String(nullable: false),
                        Length = c.String(nullable: false),
                        ImdbLink = c.String(nullable: false),
                        Companies = c.String(nullable: false),
                        Director = c.String(nullable: false),
                        Actors = c.String(nullable: false),
                        Story = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MovieGenres", "MovieId", "dbo.Movies");
            DropForeignKey("dbo.MovieCountries", "MovieId", "dbo.Movies");
            DropForeignKey("dbo.MovieGenres", "Genre_Id", "dbo.Genres");
            DropForeignKey("dbo.MovieCountries", "Country_Id", "dbo.Countries");
            DropIndex("dbo.MovieGenres", new[] { "Genre_Id" });
            DropIndex("dbo.MovieGenres", new[] { "MovieId" });
            DropIndex("dbo.MovieCountries", new[] { "Country_Id" });
            DropIndex("dbo.MovieCountries", new[] { "MovieId" });
            DropTable("dbo.Movies");
            DropTable("dbo.MovieGenres");
            DropTable("dbo.MovieCountries");
            DropTable("dbo.Genres");
            DropTable("dbo.Countries");
        }
    }
}
