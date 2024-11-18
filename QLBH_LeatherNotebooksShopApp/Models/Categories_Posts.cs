namespace QLBH_LeatherNotebooksShopApp.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Categories_Posts
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Categories_Posts()
        {
            this.BlogPosts = new HashSet<BlogPost>();
        }
    
        public int Id { get; set; }
        [Required(ErrorMessage = "Tên danh mục bài viết là bắt buộc.")]
        [StringLength(50, ErrorMessage = "Tên danh mục không được vượt quá 50 ký tự.")]
        public string Name { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BlogPost> BlogPosts { get; set; }
    }
}
