using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Services.Interfaces;

namespace ODataAPI.Controllers
{
    [Route("odata/category")] 
    public class CategoriesController : ODataController
    {
        private readonly ICatergoryService _service;

        public CategoriesController(ICatergoryService service)
        {
            _service = service;
        }

        [EnableQuery]
        [HttpGet] 
        public async Task<IActionResult> Get()
        {
            var articles = await _service.GetCategories();
            return Ok(articles.AsQueryable());
        }
    }
}
