using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace Test10App
{
    class TechnicalReview
    {
        public TechnicalReview(DateTime date, string place, bool ifActive = false)
        {
            this.Date = date;
            this.Place = place;
            this.ValidTo = this.Date.AddYears(1);
            this.IsActive = ifActive;
        }

        public DateTime Date { get; set; }
        public DateTime ValidTo { get; }
        public string Place { get; set; }
        public bool IsActive { get; set; }
    }
}
