using ChannelIntegration.EfoodAPI.Catalogs.CatalogProducts.CatalogProducts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChannelIntegration.EfoodAPI.Catalogs.CatalogProducts.Command
{
    public class UpdateCatalogProductsCommand
    {
        public IEnumerable<CatalogProduct> CatalogProducts { get; set; }
    }
}
