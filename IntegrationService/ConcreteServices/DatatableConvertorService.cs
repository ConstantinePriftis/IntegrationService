using System.ComponentModel;
using System.Data;

namespace IntegrationService.ConcreteServices
{
    public class DatatableConvertorService<T> where T : class, new()
    {
        public static DataTable ConvertEntityToDatatable(IEnumerable<T> dataToConvert)
        {
            DataTable dt = new DataTable();
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
            var InstanceOfT = Activator.CreateInstance(typeof(T));
            //TypeConverter conv = null;
            foreach (PropertyDescriptor prop in props)
            {
                var propertyType = prop.GetType();
                dt.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }
            foreach (T item in dataToConvert)
            {
                DataRow row = dt.NewRow();
                foreach (PropertyDescriptor prop in props)
                {
                    //conv = prop.Converter; //property Convertor for property Values --> To be needed Later
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                }
                dt.Rows.Add(row);
            }
            return dt;
        }

    }
}
