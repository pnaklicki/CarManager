using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test10App
{
    public class Event
    {
        public string Place { get; set; }
        public DateTime Date { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }

        public Event(string place, DateTime date, double price, string desc, string type)
        {
            this.Place = place;
            this.Date = date;
            this.Price = price;
            this.Description = desc;
            this.Type = type;
        }
    }
}
