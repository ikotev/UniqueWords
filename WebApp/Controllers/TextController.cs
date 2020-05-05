namespace UniqueWords.WebApp.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Models;

    using System.Net.Mime;
    using System.Threading.Tasks;
    using UniqueWords.Application.Words;

    public class TextController : WebApiControllerBase
    {
        private readonly ITextProcessingService _textProcessingService;

        public TextController(ITextProcessingService textProcessingService)
        {
            _textProcessingService = textProcessingService;
        }

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<ActionResult<TextResultModel>> PostText([FromBody] string text)
        {
            var result = await _textProcessingService.ProcessTextAsync(text);

            return new TextResultModel
            {
                DistinctWords = result.DistinctWords,
                DistinctUniqueWords = result.DistinctUniqueWords,
                WatchlistWords = result.WatchlistWords
            };
        }
    }
}
