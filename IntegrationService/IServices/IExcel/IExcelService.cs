using OfficeOpenXml;

namespace IntegrationService.IServices.Excel
{
    public interface IExcelService
    {
        byte[] ProcessExcelData<T>(ExcelPackage package, Dictionary<string, IQueryable<T>> Data, string[] sheets, List<string> headers, Func<List<T>, object[]> formatterFunc = null);
    }
}
