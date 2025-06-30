
using FUnewsDTO;
using Microsoft.AspNetCore.OData;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using ODataAPI.ODataEdmModelBuilders;
using Services.Interfaces;
using Services.Service;

namespace ODataAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            static IEdmModel GetEdmModel()
            {
                var builder = new ODataConventionModelBuilder(); 
                //newsArticles
                var newsArticles = builder.EntitySet<NewsArticleDTO>("newsArticles");
                newsArticles.EntityType.HasKey(x => x.NewsArticleId);
                //systemAccount
                var systemAccount = builder.EntitySet<SystemAccountDTO>("systemAccount");
                systemAccount.EntityType.HasKey(x => x.AccountId);
                //category
                var category = builder.EntitySet<CategoryDTO>("category");
                category.EntityType.HasKey(x => x.CategoryId);

                return builder.GetEdmModel();
            }

            builder.Services.AddControllers().AddOData(opt => 
            {
                opt.EnableQueryFeatures()
                    .AddRouteComponents("odata", GetEdmModel())
                    .Select()
                    .Filter()
                    .Expand()
                    .OrderBy()
                    .Count()
                    .Expand()
                    .SetMaxTop(100);
            });

            var apiBaseAddress = "https://localhost:7050/api/";

            // Add services to the container.
            builder.Services.AddHttpClient<INewsArticleService, NewsArticleService>(client =>
            {
                client.BaseAddress = new Uri(apiBaseAddress);
            });
            builder.Services.AddHttpClient<ISystemAccountService, SystemAccountService>(client => 
            {
                client.BaseAddress = new Uri(apiBaseAddress);
            });

            builder.Services.AddHttpClient<ICatergoryService, CatergoryService>(client =>
            {
                client.BaseAddress = new Uri(apiBaseAddress);
            });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
