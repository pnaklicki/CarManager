using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test10App
{
    class Refuel
    {
        
        public DateTime Date { get; set; }
        public double Volume { get; set; }
        public double Price { get; set; }
        public double PricePerLitre { get; set; }
        public int CarDistance { get; set; }
        public bool ifFullRefuel { get; set; }
        public bool ifEmptyTank { get; set; }

        public Refuel(double vol, double pricePerLitre, int distance, bool ifFull, bool ifEmpty, DateTime date)
        {
            this.Volume = vol;
            this.PricePerLitre = pricePerLitre;
            this.CarDistance = distance;
            this.Price = this.PricePerLitre * this.Volume;
            this.Price = Math.Round(this.Price, 2);
            this.Date = date;
            this.ifFullRefuel = ifFull;
            this.ifEmptyTank = ifEmpty;
        }
    }
}
