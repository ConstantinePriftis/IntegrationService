using IntegrationService.Models.EntityBases;

namespace IntegrationService.Models.Imports
{
    public class Import : SimpleBaseEntity
    {
        //public Import(string storeCode, string sku, string productDescription, int type, int step, string status, DateTime importDate, DateTime createdOn, DateTime modifiedOn):base()
        //{
        //    WarehouseID = storeCode;
        //    Sku = sku;
        //    ProductDescription = productDescription;
        //    Type = type;
        //    Step = step;
        //    Status = status;
        //    ImportDate = importDate;
        //    CreatedOn = createdOn;
        //    ModifiedOn = modifiedOn;
        //}
        public string StoreCode { get; set; }
        public string Sku { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string Step { get; set; }
        public string Status { get; set; }
        public string Efood { get; set; }
        public string Digital { get; set; }
        public DateTime ImportDate { get; set; }

        //public static Import Create(string storeCode, string sku,  string productDescription, int type, int step, string status, DateTime importDate)
        //{
        //    return new Import(
        //        storeCode,
        //        sku,
        //        productDescription,
        //        type,
        //        step,
        //        status,
        //        importDate,
        //        DateTime.UtcNow,
        //        DateTime.UtcNow);
        //}

    }
}
