using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnergyManagement.Model
{
    public class Energy
    {
        public int AccountId { get; set; }

        public string MeterReadingDateTime { get; set; }

        public string MeterReadValue { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
