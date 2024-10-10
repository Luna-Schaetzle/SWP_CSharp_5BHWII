using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APP_Test.Resources.Data
{
    public class ApiResponse
    {
        public List<SmartinfoContainer>? Smartinfo { get; set; } // Nullable machen
    }

    public class SmartinfoContainer
    {
        public Smartinfo? smartinfo { get; set; } // Nullable machen
    }

    public class Smartinfo
    {
        public string Route { get; set; } = string.Empty; // Initialisieren mit leerem String
        public string Time { get; set; } = string.Empty; // Initialisieren mit leerem String
    }
}
