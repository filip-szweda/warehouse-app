using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace entity_framework.Models
{
    public class EClient: Client
    {
        public string IPAddress { get; set; }

        public override string ToString()
        {
            return $"{Name} - [Address]: {Address}, [IPAddress]: {IPAddress}";
        }
    }
}
