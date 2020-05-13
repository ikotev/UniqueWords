using System.Collections.Generic;
using System.Data;
using UniqueWords.Application.TextProcessing;
using UniqueWords.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using UniqueWords.Application.Models;
using UniqueWords.Domain.Entities;
using System.Linq;
using Microsoft.Data.SqlClient;

namespace UniqueWords.Infrastructure.Respositories
{
    public class WordsRepository : IWordsRepository
    {
        private readonly UniqueWordsDbContext _dbContext;

        public WordsRepository(UniqueWordsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<AddNewWordsOutput>> TryAddNewWordsAsync(List<string> words)
        {
            return await TryAddNewWordsAsync("dbo.AddNewWords", words).ConfigureAwait(false);            
        }

        public async Task<List<AddNewWordsOutput>> TryAddNewWordsWithNoSyncAsync(List<string> words)
        {
            return await TryAddNewWordsAsync("dbo.AddNewWordsWithNoSync", words).ConfigureAwait(false);          
        }

        public async Task<List<AddNewWordsOutput>> TryAddNewWordsAsync(string procedureName, List<string> words)
        {
            var parameter = CreateAddNewWordsInputParameter(words);
            var response = await _dbContext.AddNewWords
                .FromSqlRaw($"EXEC {procedureName} @Words", parameter)
                .ToListAsync();

            return response;
        }

        private SqlParameter CreateAddNewWordsInputParameter(List<string> words)
        {
            var table = new DataTable();
            table.Columns.Add("RowId", typeof(int));
            table.Columns.Add("Word", typeof(string));

            var i = 1;
            foreach (var word in words)
            {
                table.Rows.Add(i, word);
                i++;
            }

            var parameter = new SqlParameter("@Words", SqlDbType.Structured);
            parameter.Value = table;
            parameter.TypeName = "[dbo].[AddNewWordsInput]";

            return parameter;
        }

        public async Task AddWordsAsync(List<WordItem> words)
        {
            await _dbContext.Words.AddRangeAsync(words);
        }

        public async Task<List<WordItem>> FindAsync(List<string> words)
        {
            var query = from w in _dbContext.Words
                        where words.Contains(w.Word)
                        select w;

            var result = await query
                .AsNoTracking()
                .ToListAsync();

            return result;
        }
    }
}