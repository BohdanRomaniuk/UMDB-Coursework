namespace database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FKRequired : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.MovieCountries", "Country_Id", "dbo.Countries");
            DropForeignKey("dbo.MovieGenres", "Genre_Id", "dbo.Genres");
            DropIndex("dbo.MovieCountries", new[] { "Country_Id" });
            DropIndex("dbo.MovieGenres", new[] { "Genre_Id" });
            AlterColumn("dbo.MovieCountries", "Country_Id", c => c.Int(nullable: false));
            AlterColumn("dbo.MovieGenres", "Genre_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.MovieCountries", "Country_Id");
            CreateIndex("dbo.MovieGenres", "Genre_Id");
            AddForeignKey("dbo.MovieCountries", "Country_Id", "dbo.Countries", "Id", cascadeDelete: true);
            AddForeignKey("dbo.MovieGenres", "Genre_Id", "dbo.Genres", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MovieGenres", "Genre_Id", "dbo.Genres");
            DropForeignKey("dbo.MovieCountries", "Country_Id", "dbo.Countries");
            DropIndex("dbo.MovieGenres", new[] { "Genre_Id" });
            DropIndex("dbo.MovieCountries", new[] { "Country_Id" });
            AlterColumn("dbo.MovieGenres", "Genre_Id", c => c.Int());
            AlterColumn("dbo.MovieCountries", "Country_Id", c => c.Int());
            CreateIndex("dbo.MovieGenres", "Genre_Id");
            CreateIndex("dbo.MovieCountries", "Country_Id");
            AddForeignKey("dbo.MovieGenres", "Genre_Id", "dbo.Genres", "Id");
            AddForeignKey("dbo.MovieCountries", "Country_Id", "dbo.Countries", "Id");
        }
    }
}
