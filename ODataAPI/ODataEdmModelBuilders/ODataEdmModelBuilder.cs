using FUnewsDTO;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;

namespace ODataAPI.ODataEdmModelBuilders
{
    public static class ODataEdmModelBuilderAdd 
    {
        public static IEdmModel GetEdmModel()
        {
            var builder = new ODataConventionModelBuilder();
            var newsArticles = builder.EntitySet<NewsArticleDTO>("NewsArticles");
            newsArticles.EntityType.HasKey(x => x.NewsArticleId);
            return builder.GetEdmModel();
        }
    }
}
