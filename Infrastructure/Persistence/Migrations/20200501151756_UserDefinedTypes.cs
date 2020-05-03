using Microsoft.EntityFrameworkCore.Migrations;

namespace UniqueWords.Infrastructure.Persistence.Migrations
{
    public partial class UserDefinedTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sql = @"CREATE TYPE [dbo].[AddNewWordsInput] AS TABLE(
                            [RowId] [int] NOT NULL,
                            [Word] [nvarchar](255) NOT NULL,
                            PRIMARY KEY CLUSTERED ([RowId] ASC)
                        )
                        
                        CREATE TYPE [dbo].[AddNewWordsOutput] AS TABLE(
                            [RowId] [int] NOT NULL,
                            [WordId] [int] NOT NULL,
                            PRIMARY KEY CLUSTERED ([RowId] ASC)
                        )";
            migrationBuilder.Sql(sql);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var sql = @"DROP TYPE [dbo].[AddNewWordsOutput]             
                        DROP TYPE [dbo].[AddNewWordsInput]";
            migrationBuilder.Sql(sql);
        }
    }
}
