namespace UniqueWords.Application.UniqueWords
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class SimpleTextAnalyzer : TextAnalyzer
    {
        protected override string FilterChars(string text)
        {
            var charFilters = new[] { '.', ',', ';' };
            var sb = new StringBuilder(text.Length);
            
            foreach (var c in text)
            {
                if (Array.IndexOf(charFilters, c) > -1)
                    continue;

                sb.Append(c);
            }

            return  sb.ToString();
        }

        protected override IEnumerable<string> TokenizeText(string text)
        {
            var tokens = text.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            return tokens;
        }

        protected override IEnumerable<string> FilterTokens(IEnumerable<string> tokens)
        {
            return tokens.Select(t => t.ToLower());
        }
    }
}