using Microsoft.EntityFrameworkCore.Migrations;

namespace UniqueWords.Infrastructure.Persistence.Migrations
{
    public partial class SP_AddNewWordsWithNoSync : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var addNewWords = @"CREATE PROCEDURE [dbo].[AddNewWordsWithNoSync]
( 	
	@Words [dbo].[AddNewWordsInput] READONLY
)
AS
BEGIN	
	SET NOCOUNT ON;

	DECLARE @Result AddNewWordsOutput;					
	DECLARE @Inserted AS TABLE(
		[Id] [int] NOT NULL,
		[Word] [nvarchar](512) NOT NULL);

	INSERT INTO dbo.Words 
		OUTPUT INSERTED.* INTO @Inserted
	SELECT inputWords.Word 
	FROM @Words AS inputWords
	WHERE NOT EXISTS (SELECT NULL FROM dbo.Words AS existingWords WHERE inputWords.Word = existingWords.Word)
	
	INSERT INTO @Result
	SELECT words.RowId, inserted.Id
	FROM @Words words INNER JOIN @Inserted inserted on words.Word = inserted.Word

	SELECT RowId, WordId FROM @Result;
END";

            migrationBuilder.Sql(addNewWords);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var sql = @"DROP PROCEDURE [dbo].[AddNewWordsWithNoSync];";
            migrationBuilder.Sql(sql);
        }
    }
}
