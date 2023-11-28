using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChannelIntegration.EfoodAPI.Catalogs.Commands
{
    public class EfoodDeleteCatalogCommandModel
    {
        public string[] categories { get; set; }
        public string[] items { get; set; }
        public string[] tiers { get; set; }
        public string[] options { get; set; }
    }
}
