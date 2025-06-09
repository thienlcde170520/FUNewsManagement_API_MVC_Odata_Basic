using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Services.Interfaces;

namespace ODataAPI.Controllers
{
    [Route("odata/newsArticles")] 
    public class NewsArticlesController : ODataController
    {
        private readonly INewsArticleService _service;

        public NewsArticlesController(INewsArticleService service)
        {
            _service = service; 
        }

        [EnableQuery]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var articles = await _service.GetNewsArticles();
            return Ok(articles.AsQueryable());
        }
    }
}
