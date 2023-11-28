using ChannelIntegration.EfoodAPI.Catalogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using static ChannelIntegration.Catalog.Categories.TestMockup;

namespace ChannelIntegration.EfoodAPI
{
    public class Efood
    {
        public IEnumerable<Catalog> Catalogs { get; set; }
    }
}