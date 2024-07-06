using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceApp.Domain.Constants
{
    public static class OrderStatusConstants
    {
        public const string Pending = "Pending";
        public const string Paid = "Paid";
        public const string Delivery = "Delivery";
        public const string Cancelled = "Cancelled";
        public const string Completed = "Completed";
        public const string Failed = "Failed";
    }
}
