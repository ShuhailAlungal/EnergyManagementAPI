using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnergyManagementMvc.Models
{
    public class Energy
    {
        public int accountId { get; set; }

        public string meterReadingDateTime { get; set; }

        public string meterReadValue { get; set; }

        public string firstName { get; set; }

        public string lastName { get; set; }
    }
}
