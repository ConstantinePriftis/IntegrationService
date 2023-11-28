using CsvHelper;
using CsvHelper.Configuration;
using IntegrationService.Services;
using System;
using System.Globalization;
using System.Text;

namespace IntegrationService.ConcreteServices
{
    public class CSVService : ICSVService
    {
        public IEnumerable<T> ReadCSV<T>(Stream file,Encoding encoding)
        {
            var exceptions = new List<HeaderValidationException>();
            var config = new CsvConfiguration(CultureInfo.InvariantCulture) { Delimiter = ";", Encoding = encoding };
            config.TrimOptions = TrimOptions.Trim;
            config.BadDataFound = null;
            //Encoding.GetEncoding(1253)
            var reader = new StreamReader(file, encoding);
            List<T> records = null;
            using (var csv = new CsvReader(reader, config))
            {
                if (exceptions.Count > 0)
                {
                    throw new AggregateException(exceptions);
                }
                records = csv.GetRecords<T>().ToList();
            }
            return records;

        }
    }
}
