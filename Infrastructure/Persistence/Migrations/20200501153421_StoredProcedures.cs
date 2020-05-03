using Microsoft.EntityFrameworkCore.Migrations;

namespace UniqueWords.Infrastructure.Persistence.Migrations
{
    public partial class StoredProcedures : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var addNewWord = @"CREATE PROCEDURE [dbo].[AddNewWord]
            ( 	
                @Word nvarchar(512),
                @WordId int OUTPUT  
            )
            AS
            BEGIN
                SET XACT_ABORT, NOCOUNT ON;
                        
                DECLARE @Resource nvarchar(528) = N'dbo.Words.LOCK.' + @Word;
                DECLARE @Result int;				

                BEGIN TRAN			

                EXEC @Result = sp_getapplock @Resource = @Resource, @LockMode = 'Exclusive';

                IF (@Result < 0) 
                BEGIN
                    DECLARE @msg nvarchar(1024) = FORMATMESSAGE(N'Exclusive lock failed to acquire. The result of the sp_getapplock for resource ''%s'' is %i', @Resource, @Result);
                    THROW 51000, @msg, 1;			
                END
                    
                IF(NOT EXISTS(SELECT 1 FROM dbo.Words WHERE Word = @Word))
                BEGIN
                    INSERT INTO dbo.Words(Word) VALUES (@Word)		
                    SET @WordId = SCOPE_IDENTITY();
                END		
                ELSE
                    SET @WordId = 0;

                EXEC sp_releaseapplock @Resource = @Resource;
                COMMIT TRAN	
            END";

            migrationBuilder.Sql(addNewWord);

            var addNewWords = @"CREATE PROCEDURE [dbo].[AddNewWords]
            ( 	
                @Words [dbo].[AddNewWordsInput] READONLY
            )
            AS
            BEGIN	
                SET NOCOUNT ON;

                DECLARE @WordId int;	
                DECLARE @RowId int;	
                DECLARE @Word nvarchar(512);	
                DECLARE @NewWords as AddNewWordsInput;
                DECLARE @Result AddNewWordsOutput;			

                INSERT INTO @NewWords
                SELECT words.RowId, words.Word 
                FROM @Words words
                WHERE NOT EXISTS (SELECT NULL FROM [dbo].[Words] existingWords WHERE words.Word = existingWords.Word)	

                WHILE(1 = 1)
                BEGIN
                    SELECT TOP 1 @RowId = RowId, @Word = word FROM @NewWords;

                    IF @@ROWCOUNT = 0 
                        BREAK;

                    EXEC dbo.AddNewWord @Word = @Word, @WordId = @WordId OUTPUT;

                    IF(@WordId > 0)		
                        INSERT INTO @Result VALUES(@RowId, @WordId);	

                    DELETE FROM @NewWords WHERE Word = @Word;
                end	
                
                SELECT RowId, WordId FROM @Result;
            END";

            migrationBuilder.Sql(addNewWords);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var sql = @"DROP PROCEDURE [dbo].[AddNewWords]                        
                        DROP PROCEDURE [dbo].[AddNewWord]";
            migrationBuilder.Sql(sql);
        }
    }
}
