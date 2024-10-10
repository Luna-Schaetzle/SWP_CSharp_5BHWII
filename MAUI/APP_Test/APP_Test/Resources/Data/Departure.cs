using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APP_Test.Resources.Data
{
    public class Departure
    {
        public string Route { get; set; } = string.Empty; // Zuweisung eines leeren Strings
        public string Time { get; set; } = string.Empty; // Zuweisung eines leeren Strings

        public static Departure FromSmartinfo(Smartinfo smartinfo)
        {
            return new Departure
            {
                Route = smartinfo.Route,
                Time = smartinfo.Time
            };
        }
    }
}
