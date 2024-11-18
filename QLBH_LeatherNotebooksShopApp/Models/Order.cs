namespace QLBH_LeatherNotebooksShopApp.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Order
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Order()
        {
            this.OrderDetails = new HashSet<OrderDetail>();
        }
    
        public int ID { get; set; }
        public System.DateTime DateOrder { get; set; }
        public Nullable<int> IDCus { get; set; }
        public string AddressDeliverry { get; set; }
        public string OrderStatus { get; set; }
        public string PaymentMethod { get; set; }
        public string Note { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual Customer Customer { get; set; }
    }
}
