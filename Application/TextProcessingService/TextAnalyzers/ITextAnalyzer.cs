namespace UniqueWords.Application.TextProcessing.TextAnalyzers
{
    using System.Collections.Generic;

    public interface ITextAnalyzer
    {
        IEnumerable<string> GetTokens(string text);
    }
}