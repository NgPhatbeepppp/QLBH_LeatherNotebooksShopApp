namespace QLBH_LeatherNotebooksShopApp.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class BlogPost
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public BlogPost()
        {
            this.Comments = new HashSet<Comment>();
        }
    
        public int Id { get; set; }
        [Required(ErrorMessage = "Tiêu đề bài viết là bắt buộc.")]
        [StringLength(200, ErrorMessage = "Tiêu đề không được vượt quá 200 ký tự.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Nội dung bài viết là bắt buộc.")]
        public string Content { get; set; }

        [Required(ErrorMessage = "Tên tác giả là bắt buộc.")]
        [StringLength(100, ErrorMessage = "Tên tác giả không được vượt quá 100 ký tự.")]
        public string Author { get; set; }
        public Nullable<System.DateTime> PublishDate { get; set; }
        public Nullable<int> CategoryId { get; set; }
        public string ImageUrl { get; set; }
        public string Status { get; set; }
    
        public virtual Categories_Posts Categories_Posts { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Comment> Comments { get; set; }
    }
}
