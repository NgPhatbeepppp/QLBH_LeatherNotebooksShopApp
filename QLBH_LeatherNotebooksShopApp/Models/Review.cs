namespace QLBH_LeatherNotebooksShopApp.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Review
    {
        public int Id { get; set; }
        public Nullable<int> ProductId { get; set; }
        public Nullable<int> CustomerId { get; set; }
        public Nullable<int> Rating { get; set; }
        public string Comment { get; set; }
        public System.DateTime CreatedDate { get; set; }
    
        public virtual Product Product { get; set; }
        public virtual Customer Customer { get; set; }
    }
}
