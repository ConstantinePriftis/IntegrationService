using IntegrationService.Models.Stores;

namespace IntegrationService.Models.Collection
{
    public class CollectionValueEnumeration : Enumeration<CollectionValueEnumeration, string>
    {
        public static readonly string IN = "9";
        public static readonly string OUT = "X";
        public static readonly string EMPTY = " ";
    }
}
