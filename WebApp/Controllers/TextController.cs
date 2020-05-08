using Microsoft.AspNetCore.Mvc;

using System.Net.Mime;
using System.Threading.Tasks;
using UniqueWords.Application.TextProcessing;
using UniqueWords.WebApp.Models;

namespace UniqueWords.WebApp.Controllers
{
    public class TextController : WebApiControllerBase
    {
        private readonly ITextProcessingService _textProcessingService;

        public TextController(ITextProcessingService textProcessingService)
        {
            _textProcessingService = textProcessingService;
        }

        /// <summary>
        /// Uses DB lock to synchronize addition of new words
        /// </summary>
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<ActionResult<TextResultModel>> PostTextWithDbSync([FromBody] TextModel model)
        {
            var result = await _textProcessingService.ProcessTextAsync(model.Text);

            return new TextResultModel
            {
                DistinctWords = result.DistinctWords,
                DistinctUniqueWords = result.DistinctUniqueWords,
                WatchlistWords = result.WatchlistWords
            };
        }

        /// <summary>
        /// Uses back-end service to synchronize addition of new words
        /// </summary>
        [HttpPost("backend-sync")]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<ActionResult<TextResultModel>> PostTextWithBackEndSync([FromBody] TextModel model)
        {
            var result = await _textProcessingService.ProcessTextAsync(model.Text);

            return new TextResultModel
            {
                DistinctWords = result.DistinctWords,
                DistinctUniqueWords = result.DistinctUniqueWords,
                WatchlistWords = result.WatchlistWords
            };
        }
    }
}
