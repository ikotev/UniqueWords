namespace UniqueWords.WebApp.Controllers
{
    using Models;
    using Microsoft.AspNetCore.Mvc;    

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
        public async Task<ActionResult<TextResultModel>> PostText([FromBody] TextModel model)
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
