namespace UniqueWords.WebApp.Controllers
{
    using Application.Interfaces;

    using Microsoft.AspNetCore.Mvc;

    using Models;

    using System.Net.Mime;
    using System.Threading.Tasks;

    public class TextController : WebApiControllerBase
    {
        private readonly IUniqueWordsService _uniqueWordsService;

        public TextController(IUniqueWordsService uniqueWordsService)
        {
            _uniqueWordsService = uniqueWordsService;
        }

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<ActionResult<TextResultModel>> PostText([FromBody] string text)
        {
            var result = await _uniqueWordsService.ProcessTextAsync(text);

            return new TextResultModel
            {
                DistinctUniqueWords = result.DistinctUniqueWords,
                WatchlistWords = result.WatchlistWords
            };
        }
    }
}
