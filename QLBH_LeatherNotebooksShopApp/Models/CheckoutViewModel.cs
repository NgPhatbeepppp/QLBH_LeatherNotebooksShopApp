using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLBH_LeatherNotebooksShopApp.Models
{
    public class CheckoutViewModel
    {
        public string DeliveryAddress { get; set; }
        public string PaymentMethod { get; set; }
        public string Note { get; set; }
    }
}