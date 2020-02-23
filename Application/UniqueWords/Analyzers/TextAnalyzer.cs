namespace UniqueWords.Application.UniqueWords
{
    using System.Collections.Generic;
    using Interfaces;

    public abstract class TextAnalyzer : ITextAnalyzer
    {
        public IEnumerable<string> GetTokens(string text)
        {
            var filteredText = FilterChars(text);
            var tokens = TokenizeText(filteredText);
            return FilterTokens(tokens);
        }

        protected virtual string FilterChars(string text)
        {
            return text;
        }

        protected abstract IEnumerable<string> TokenizeText(string text);

        protected virtual IEnumerable<string> FilterTokens(IEnumerable<string> tokens)
        {
            return tokens;
        }
    }
}