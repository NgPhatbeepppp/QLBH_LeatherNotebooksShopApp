﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace QLBH_LeatherNotebooksShopApp.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Web;

    public partial class Product
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Product()
        {
            this.OrderDetails = new HashSet<OrderDetail>();
            this.Reviews = new HashSet<Review>();
        }
    
        public int ProductID { get; set; }
        [Required(ErrorMessage = "Tên sản phẩm là bắt buộc.")]
        [StringLength(100, ErrorMessage = "Tên sản phẩm không được vượt quá 100 ký tự.")]
        [Display(Name = "Tên sản phẩm")]
        public string NamePro { get; set; }

        [StringLength(500, ErrorMessage = "Mô tả sản phẩm không được vượt quá 500 ký tự.")]
        [Display(Name = "Mô tả sản phẩm")]
        public string DecriptionPro { get; set; }

        [Required(ErrorMessage = "Danh mục sản phẩm là bắt buộc.")]
        public int CategoryID { get; set; }

        [Required(ErrorMessage = "Giá sản phẩm là bắt buộc.")]
        public decimal Price { get; set; }

        [Display(Name = "Hình ảnh sản phẩm")]
        public string ImagePro { get; set; }

        [Required(ErrorMessage = "Vui lòng cung cấp số lượng sản phẩm.")]
        [Range(0, int.MaxValue, ErrorMessage = "Số lượng phải là một số không âm.")]
        public Nullable<int> Quantity { get; set; }

        [NotMapped]
        public HttpPostedFileBase UploadImage { get; set; }

        public virtual Category Category { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Review> Reviews { get; set; }
    }
}
