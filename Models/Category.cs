using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicManagement.Models
{
    public class Category
    {
        [Key]
        public int ID { get; set; }

         [Display(Name ="Tên Danh Mục")]
        [Required(ErrorMessage ="phải nhập {0}")]
        [StringLength(100,ErrorMessage ="{0} phải từ {2} đến {1} ký tự",MinimumLength =2)]
        public string Name { get; set; }

        [Display(Name ="Mô tả")]
        public string Descriptions { get; set; }

        // Các Category con
      public virtual ICollection<Category> CategoryChildren { get; set; }

        [Display(Name = "Danh mục cha")]
        public int? ParentCategoryID { get; set; }

        [ForeignKey("ParentCategoryID")]
        public virtual Category ParentCategory{get;set;}


    }
}