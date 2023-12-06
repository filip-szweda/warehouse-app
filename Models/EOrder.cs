using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace entity_framework.Models
{
    internal class EOrder : Order
    {
        public string IPAddress { get; set; }
    }
}
