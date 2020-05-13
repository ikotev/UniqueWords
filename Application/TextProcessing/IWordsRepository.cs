using System.Collections.Generic;
using System.Threading.Tasks;
using UniqueWords.Application.Models;
using UniqueWords.Domain.Entities;

namespace UniqueWords.Application.TextProcessing
{
    public interface IWordsRepository
    {        
        Task<List<AddNewWordsOutput>> TryAddNewWordsAsync(List<string> words);   

        Task<List<AddNewWordsOutput>> TryAddNewWordsWithNoSyncAsync(List<string> words);
        
        Task AddWordsAsync(List<WordItem> words);

        Task<List<WordItem>> FindAsync(List<string> words);
    }
}