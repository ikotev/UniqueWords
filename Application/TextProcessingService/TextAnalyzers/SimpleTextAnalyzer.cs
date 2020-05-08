using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniqueWords.Application.TextProcessing.TextAnalyzers
{
    public class SimpleTextAnalyzer : BaseTextAnalyzer
    {
        protected override string FilterChars(string text)
        {
            var sb = new StringBuilder(text.Length);

            foreach (var c in text)
            {
                if (!char.IsPunctuation(c))
                    sb.Append(c);
            }

            return sb.ToString();
        }

        protected override IEnumerable<string> TokenizeText(string text)
        {
            var tokens = text.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
            return tokens;
        }

        protected override IEnumerable<string> FilterTokens(IEnumerable<string> tokens)
        {
            return tokens.Select(t => t.ToLower());
        }
    }
}