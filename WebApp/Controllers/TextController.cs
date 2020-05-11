using Microsoft.AspNetCore.Mvc;

using System.Net.Mime;
using System.Threading.Tasks;
using UniqueWords.Application.TextProcessing;
using UniqueWords.WebApp.Models;

namespace UniqueWords.WebApp.Controllers
{
    [ApiVersion(WebApiDefaults.LatestVersion)]
    public class TextController : WebApiControllerBase
    {
        private readonly ITextProcessingServiceFactory _textProcessingServiceFactory;        

        public TextController(ITextProcessingServiceFactory textProcessingServiceFactory)
        {
            _textProcessingServiceFactory = textProcessingServiceFactory;
        }

        /// <summary>
        /// Uses DB lock to synchronize unique words adding
        /// </summary>
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<ActionResult<TextResultModel>> PostTextWithDbSync(TextModel model)
        {
            var textProcessingService = _textProcessingServiceFactory.Create<UniqueWordsAddingDbSync>();
            var result = await textProcessingService.ProcessTextAsync(model.Text);

            return new TextResultModel
            {
                DistinctWords = result.DistinctWords,
                DistinctUniqueWords = result.DistinctUniqueWords,
                WatchlistWords = result.WatchlistWords
            };
        }

        /// <summary>
        /// Uses back-end service to synchronize unique words adding
        /// </summary>
        [HttpPost("backend-sync")]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<ActionResult<TextResultModel>> PostTextWithBackEndSync(TextModel model)
        {
            var textProcessingService = _textProcessingServiceFactory.Create<UniqueWordsAddingBackendSync>();
            var result = await textProcessingService.ProcessTextAsync(model.Text);

            return new TextResultModel
            {
                DistinctWords = result.DistinctWords,
                DistinctUniqueWords = result.DistinctUniqueWords,
                WatchlistWords = result.WatchlistWords
            };
        }
    }
}
