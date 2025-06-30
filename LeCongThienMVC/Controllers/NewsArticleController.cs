using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LeCongThienMVC.Models;
using Microsoft.AspNetCore.Authorization;
using FUnewsDTO;
using LeCongThienMVC.Utilities;
using Services.Interfaces;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using DataAccessObjects;

namespace LeCongThienMVC.Controllers
{
    [ValidateJwtToken]
    public class NewsArticleController : Controller
    {
        private readonly ITagService _tagService;
        private readonly INewsArticleService _newsArticleService;
        private readonly ICatergoryService _categoryService;
        private readonly ISystemAccountService _systemAccountService;
        private readonly HttpClient _httpClient;
        public NewsArticleController(ITagService tagService, INewsArticleService newsArticleService, ICatergoryService categoryService, 
                ISystemAccountService systemAccountService, IHttpClientFactory factory)
        {
            _tagService = tagService;
            _newsArticleService = newsArticleService;
            _categoryService = categoryService;
            _systemAccountService = systemAccountService;
            _httpClient = factory.CreateClient("ODataAPI");
        }

        public async Task<IActionResult> Index(string searchTerm = "", string sortField = "CreatedDate", string sortDirection = "asc",
            int pageNumber = 1, int pageSize = 4)
        {

            //string sortDirection = "desc"
            int skip = (pageNumber - 1) * pageSize;          

            string filterQuery = string.IsNullOrEmpty(searchTerm)
                ? ""
                : $"$filter=contains(NewsTitle,'{searchTerm}')&";

            //string sortFields = "CategoryId";

            string orderByQuery = $"$orderby={sortField} {sortDirection}&";

            string pagingQuery = $"$skip={skip}&$top={pageSize}&$count=true";
            //add filter and order, paging to query
            string query = $"/odata/newsArticles?{filterQuery}{orderByQuery}{pagingQuery}";

            var response = await _httpClient.GetAsync(query);
            Console.WriteLine("Status Code: " + response.StatusCode);
            if (!response.IsSuccessStatusCode)
                return View("Error");

            var content = await response.Content.ReadAsStringAsync();
            var json = JObject.Parse(content);

            var accounts = json["value"].ToObject<List<NewsArticleDTO>>();
            int totalCount = json["@odata.count"]?.Value<int>() ?? 0;

            var model = new PagedListViewModel<NewsArticleDTO>
            {
                Items = accounts,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount
            };

            return View(model);
        }

        public async Task<IActionResult> Detail(string id)
        {
            var news = await _newsArticleService.GetNewsArticleById(id);
            if (news == null) return NotFound();
            return View(news);
        }

        public async Task<IActionResult> Create()
        {
            var categories = await _categoryService.GetCategories();
            ViewBag.CategoryId = new SelectList(categories, "CategoryId", "CategoryName");

            var users = await _systemAccountService.GetAccounts();
            ViewBag.CreatedById = new SelectList(users, "AccountId", "AccountName");
            ViewBag.UpdatedById = new SelectList(users, "AccountId", "AccountName");

            var tagList = await _tagService.GetTags();
            ViewBag.TagList = new MultiSelectList(tagList, "TagId", "TagName");


            int newId;
            int existingIds = (await _newsArticleService.GetNewsArticles())
                                .Select(n => n.NewsArticleId)
                                .Count();

            
            newId = existingIds + 1;
            

            ViewBag.NewsId = newId.ToString(); // Thêm ID vào ViewBag để sử dụng trong view

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(NewsArticleDTO dto)
        {
            if (!ModelState.IsValid)
            {
                // Repopulate dropdowns nếu ModelState không valid
                var categories = await _categoryService.GetCategories();
                ViewBag.CategoryId = new SelectList(categories, "CategoryId", "CategoryName");

                var users = await _systemAccountService.GetAccounts();
                ViewBag.CreatedById = new SelectList(users, "AccountId", "AccountName");
                ViewBag.UpdatedById = new SelectList(users, "AccountId", "AccountName");

                var tagList = await _tagService.GetTags();
                ViewBag.TagList = new MultiSelectList(tagList, "TagId", "TagName");


                int newId;
                int existingIds = (await _newsArticleService.GetNewsArticles())
                                    .Select(n => n.NewsArticleId)
                                    .Count();


                newId = existingIds + 1;


                ViewBag.NewsId = newId.ToString(); // Thêm ID vào ViewBag để sử dụng trong view
            }

            try
            {
                // Tự động tạo ID nếu chưa có
                if (string.IsNullOrEmpty(dto.NewsArticleId))
                {
                    dto.NewsArticleId = Guid.NewGuid().ToString();
                }

                // Thêm logging để kiểm tra
                Console.WriteLine($"Creating news article with ID: {dto.NewsArticleId}");
                if(dto.ModifiedDate == null)
                {
                    dto.ModifiedDate = DateTime.Now;
                }

                await _newsArticleService.Create(dto);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Log lỗi
                Console.WriteLine($"Error creating news article: {ex.Message}");

                TempData["ErrorMessage"] = "An error occurred while creating the news article";
                return RedirectToAction(nameof(Create));
            }
        }

        public async Task<IActionResult> Edit(string id)
        {
            var article = await _newsArticleService.GetNewsArticleById(id);
            if (article == null) return NotFound();

            var categories = await _categoryService.GetCategories();
            ViewBag.CategoryId = new SelectList(categories, "CategoryId", "CategoryName", article.CategoryId);

            var users = await _systemAccountService.GetAccounts();
            ViewBag.CreatedById = new SelectList(users, "AccountId", "AccountName", article.CreatedById);
            ViewBag.UpdatedById = new SelectList(users, "AccountId", "AccountName", article.UpdatedById);

            return View(article);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, NewsArticleDTO dto)
        {
            if (id != dto.NewsArticleId) return BadRequest();
            if (!ModelState.IsValid) return View(dto);

            dto.ModifiedDate = DateTime.Now;

            await _newsArticleService.Update(dto);
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> Approve(string id, NewsArticleDTO dto)
        {
            if (id != dto.NewsArticleId) return BadRequest();
            if (!ModelState.IsValid) return View(dto);

            dto.ModifiedDate = DateTime.Now;
            dto.NewsStatus = true;
            await _newsArticleService.Update(dto);
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(string id)
        {
            var article = await _newsArticleService.GetNewsArticleById(id);
            if (article == null) return NotFound();
            return View(article);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmDelete(string id)
        {
            try
            {
                await _newsArticleService.Delete(id);

                TempData["SuccessMessage"] = "News article deleted successfully";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting article: {ex.Message}");
                TempData["ErrorMessage"] = "An error occurred while deleting the news article";
                return RedirectToAction(nameof(Delete), new { id });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            try
            {
                var newsArticle = await _newsArticleService.GetNewsArticleById(id);
                if (newsArticle == null)
                {
                    TempData["ErrorMessage"] = "News article not found";
                    return RedirectToAction(nameof(Index));
                }

                ViewBag.CategoryName = newsArticle.Category.CategoryName ?? "N/A";
                ViewBag.CreatedByName = newsArticle.CreatedBy.AccountName ?? "N/A";
                //ViewBag.UpdatedByName = newsArticle.UpdatedBy.AccountName ?? "N/A";

                return View(newsArticle);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving news article details: {ex.Message}");
                TempData["ErrorMessage"] = "An error occurred while retrieving news article details";
                return RedirectToAction(nameof(Index));
            }
        }

        public async Task<IActionResult> MyNews()
        {
            var myArticles = await _newsArticleService.GetNewsArticlesByAuthor();
            return View(myArticles);
        }

        public async Task<IActionResult> AdminManageNews()
        {
            var allArticles = await _newsArticleService.GetNewsArticlesActiveAsync();
            return View(allArticles);
        }
    }
}
