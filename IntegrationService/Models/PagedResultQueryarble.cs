namespace IntegrationService.Models
{
    public class PagedResultQueryarble<T> 
    {
        public IQueryable<T> Results { get; set; }
        public int CurrentPage { get; set; }
        public int PageCount { get; set; }
        public int PageSize { get; set; }
        public int RowCount { get; set; }
        public int StartPage { get; set; }
        public int EndPage { get; set; }
    }
}
