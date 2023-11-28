using IntegrationService.Models;
using IntegrationService.ViewModels;

namespace IntegrationService.ConcreteServices.Helper
{
    public static class Pagination
    {
        public static Task<PagedResultQueryarble<T>> ApplyPagedListAsync<T>(
       this IQueryable<T> query, int page, int pageSize)
        {
            var result = new PagedResultQueryarble<T>();
            result.PageSize = pageSize;
            result.RowCount = query.Count();
            var pageCount = (double)result.RowCount / pageSize;
            result.PageCount = (int)Math.Ceiling(pageCount);
            result.CurrentPage = result.PageCount > 1 && result.PageCount >= page ? page :1;
            var skip = (result.CurrentPage - 1) * pageSize;
            result.Results = query.Skip(skip).Take(pageSize);
            result.StartPage = result.CurrentPage - 5;
            result.EndPage = result.CurrentPage + 4;
            if (result.StartPage <= 0)
            {
                result.EndPage -= (result.StartPage - 1);
                result.StartPage = 1;
            }

            if (result.EndPage > result.PageCount)
            {
                result.EndPage = result.PageCount;
                if (result.EndPage > 10)
                {
                    result.StartPage = result.EndPage - 9;
                }
            }

            return Task.FromResult(result);
        }
    }
}
