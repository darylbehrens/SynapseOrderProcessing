using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynapseOrders.Models
{
    public class OrderProcessingResult
    {
        public int TotalOrdersProcessed { get; set; }
        public int SuccessfulOrders { get; set; }
        public int FailedOrders { get; set; }
        public bool IsSuccessful => FailedOrders == 0;
    }
}
