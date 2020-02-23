using Microsoft.EntityFrameworkCore.Migrations;

namespace UniqueWords.Infrastructure.Persistence.Migrations
{
    public partial class SeedWatchlist : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "WatchList",
                columns: new[] { "Id", "Word" },
                values: new object[] { 1, "horse" });

            migrationBuilder.InsertData(
                table: "WatchList",
                columns: new[] { "Id", "Word" },
                values: new object[] { 2, "zebra" });

            migrationBuilder.InsertData(
                table: "WatchList",
                columns: new[] { "Id", "Word" },
                values: new object[] { 3, "dog" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "WatchList",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "WatchList",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "WatchList",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
