using System.Diagnostics.CodeAnalysis;
using System.Dynamic;
using System.Reflection;

namespace IntegrationService.ConcreteServices.Helper
{
    public class Comparer : IEqualityComparer<object>
    {
        public new bool Equals(object x, object y)
        {
            var properties = x.GetType().GetProperties();
            foreach (var property in properties)
            {
                var xValue = property.GetValue(x)?.ToString();
                var yValue = property.GetValue(y)?.ToString();
                if (xValue != yValue)
                    return false;
            }
            return true;
        }

        public int GetHashCode(object obj)
        {
            // Calculate a unique hash code based on the property values
            var properties = obj.GetType().GetProperties();
            int hashCode = 17;
            foreach (var property in properties)
            {
                var value = property.GetValue(obj);
                if (value != null)
                    hashCode = hashCode * 23 + value.GetHashCode();
            }
            return hashCode;
        }
    }
}
