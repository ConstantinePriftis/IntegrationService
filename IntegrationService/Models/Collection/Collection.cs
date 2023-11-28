using IntegrationService.Models.EntityBases;

namespace IntegrationService.Models.Collection
{
    public class Collection
    {
        public Collection() 
        {
            
        }
        public string Sku { get; set; }
        public string StoreCode { get; set; }
        public Guid ChannelId { get; set; }
        public string Value { get; set; }
        public DateTime ModifiedOn { get; set; }

		public string GetCollectionStatusByValue()
		{

			return Value switch
			{
				"9" => "IN",
				"X" => "OUT",
				" " => string.Empty
			};
		}
	}
}
