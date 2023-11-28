using IntegrationService.Models.Results;

namespace IntegrationService.IServices.IHttpFactoryService
{
    public interface IHttpClientFactoryService
    {
        Task<string> ExecuteGetAsync(string url);
        Task<HttpResult> ExecutePostAsync(string url, object data);
    }
}
