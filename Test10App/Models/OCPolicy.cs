using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test10App
{
    class OCPolicy
    {
        public DateTime ValidTo { get; set; }
        public string Provider { get; set; }
        public int Price { get; set; }
        public bool IsActive { get; set; }

        public OCPolicy(DateTime to, string prov, int price, bool ifActive)
        {
            this.ValidTo = to;
            this.Provider = prov;
            this.Price = price;
            this.IsActive = ifActive;
        }
    }
}
