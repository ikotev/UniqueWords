using System.Collections.Generic;

namespace UniqueWords.Application.TextProcessing.TextAnalyzers
{    
    public interface ITextAnalyzer
    {
        IEnumerable<string> GetTokens(string text);
    }
}