namespace LeCongThienMVC.Models
{
    public class NewsArticleReportDTO
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<NewsArticle> NewsArticles { get; set; }
    }
}
