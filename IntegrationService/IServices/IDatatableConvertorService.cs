using System.Data;

namespace IntegrationService.Services
{
    public interface IDatatableConvertorService<T>
    {
        public  DataTable ConvertEntityToDatatable(IEnumerable<T> dataToConvert);
    }
}
