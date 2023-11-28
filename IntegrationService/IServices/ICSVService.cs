using System.Text;

namespace IntegrationService.Services
{
    public interface ICSVService
    {
        public IEnumerable<T> ReadCSV<T>(Stream file, Encoding encoding);
    }
}
