using IntegrationService.IServices.IHttpFactoryService;
using IntegrationService.IServices.IRepository;
using IntegrationService.Models.Errors;
using IntegrationService.Models.Results;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace IntegrationService.ConcreteServices.HttpFactory
{
    public class HttpFactoryService : IHttpClientFactoryService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IErrorRepository _errorRepository;

        public HttpFactoryService(
            IHttpClientFactory httpClientFactory,
            IErrorRepository errorRepository)
        {
            _httpClientFactory = httpClientFactory;
            _errorRepository = errorRepository;
        }

        public async Task<string> ExecuteGetAsync(string url)
        {
            var httpClient = _httpClientFactory.CreateClient();
            try
            {
                Uri uriResult;
                bool isValidUrl = Uri.TryCreate(url, UriKind.Absolute, out uriResult)
                    && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

                if (isValidUrl)
                {
                    using (var response = await httpClient.GetAsync(uriResult))
                    {
                        var result = response;
                        response.EnsureSuccessStatusCode();
                        var stringContent = await result.Content.ReadAsStringAsync();
                        if (!string.IsNullOrEmpty(stringContent))
                            return stringContent;
                        else
                            return string.Empty;
                    }
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
            catch (Exception ex)
            {
                var errorRaised = Error.Create(ex.Message);
                _errorRepository.Add(errorRaised);
                return string.Empty;
            }
        }

        public async Task<HttpResult> ExecutePostAsync(string url, object data)
        {
            var result = new HttpResult();
            try
            {
                string serializedData = JsonConvert.SerializeObject(data);
                if (string.IsNullOrWhiteSpace(serializedData))
                {
                    result.StatusCode = 400;
                    result.ResponseMessage = "Null Data";
                    return result;
                }

                using (var httpClient = _httpClientFactory.CreateClient())
                {
                    Uri uriResult;
                    bool isValidUrl = Uri.TryCreate(url, UriKind.Absolute, out uriResult)
                        && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

                    if (isValidUrl)
                    {
                        var requestContent = new StringContent(serializedData, Encoding.UTF8, "application/json");
                        var response = await httpClient.PostAsync(uriResult, requestContent);
                        response.EnsureSuccessStatusCode();

                        result.ResponseMessage = await response.Content.ReadAsStringAsync();
                        result.StatusCode = (int)response.StatusCode;
                        return result;
                    }
                    else
                    {
                        throw new InvalidOperationException();
                    }
                }
            }
            catch (Exception ex)
            {
                var errorRaised = Error.Create(ex.Message);
                _errorRepository.Add(errorRaised);
                return result;
            }
        }
    }
}
