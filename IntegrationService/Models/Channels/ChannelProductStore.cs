using IntegrationService.Models.EntityBases;
using IntegrationService.Models.Fields;
using IntegrationService.Models.Product;
using MessagePack;

namespace IntegrationService.Models.Channels
{
    public class ChannelProductStore 
    {
        public ChannelProductStore()
        {

        }
        //public ChannelProductStore(Guid productStoresId,  string value)
        //{
        //    ProductStoresId = productStoresId;
        //    Value = value;
        //    CreatedOn = DateTime.UtcNow;
        //    ModifiedOn = DateTime.UtcNow;
        //}
       // public Guid Id { get; set; } = Guid.NewGuid();
        public Guid ProductStoresId { get; set; }
        public string Efood { get; set; }
        public string EfoodExpress { get; set; }
        public string Digital { get; set; }
        public DateTime ModifiedOn { get; set; }

        //public static ChannelProductStore Create(Guid productStoresId, Guid channelId, string value)
        //{
        //    return new(productStoresId, channelId, value);
        //}

        //public void UpdateValue(string value)
        //{
        //    Value = value;
        //    ModifiedOn = DateTime.UtcNow;
        //}
    }
}
