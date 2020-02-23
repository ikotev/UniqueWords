namespace UniqueWords.Application.Interfaces
{
    using System.Collections.Generic;

    public interface ITextAnalyzer
    {
        IEnumerable<string> GetTokens(string text);
    }
}