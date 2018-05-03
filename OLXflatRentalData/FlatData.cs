using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLXflatRentalData
{
    public class FlatData
    {
        public string Title { get; set; }
        public int NominalPrice { get; set; }
        public int AdditionalPrice { get; set; }
        public int RealPrice { get; }
        public string OfferFrom { get; set; }
        public int Area { get; set; }
        public int RoomsCount { get; set; }
    }
}
