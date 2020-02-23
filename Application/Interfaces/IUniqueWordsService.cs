﻿namespace UniqueWords.Application.Interfaces
{
    using Models;

    using System.Threading.Tasks;

    public interface IUniqueWordsService
    {
        Task<ProcessedTextResult> ProcessTextAsync(string text);
    }
}