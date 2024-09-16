using System;
using System.ComponentModel.DataAnnotations;

namespace ClinicManagement.Models
{
    public class Unit
    {
        [Key]
        public int ID { get; set; }
        [Display(Name ="Tên Đơn vị")]
        [Required(ErrorMessage ="phải nhập {0}")]
        [StringLength(20,ErrorMessage ="{0} phải từ {2} đến {1} ký tự",MinimumLength =1)]
        public string Name { get; set; }
         [Display(Name ="Mô tả")]      
        [StringLength(150,ErrorMessage ="{0} phải từ {2} đến {1} ký tự",MinimumLength =1)]
        public string Description { get; set; }

         [Display(Name ="Ngày tạo")]
        [DataType(DataType.Date)]
        public DateTime DateCreate{get;set;}
    }
}