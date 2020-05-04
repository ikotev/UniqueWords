using System.Collections.Generic;
using System.Threading.Tasks;
using UniqueWords.Application.Models;
using UniqueWords.Domain.Entities;

namespace UniqueWords.Application.Words
{
    public interface IWordsRepository
    {        
        Task<List<AddNewWordsOutput>> AddNewWordsV2Async(List<string> words);   

        Task AddWordsAsync(List<WordItem> words);

        Task<List<WordItem>> FindAsync(List<string> words);
    }
}