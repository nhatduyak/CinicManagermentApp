using System;
using System.ComponentModel.DataAnnotations;

namespace ClinicManagement.Models
{
    public class Manufacture
    {
          [Key]
        public int ID { get; set; }
        [Display(Name ="Tên nhà sản xuất")]
        [Required(ErrorMessage ="phải nhập {0}")]
        [StringLength(50,ErrorMessage ="{0} phải từ {2} đến {1} ký tự",MinimumLength =1)]
        public string Name { get; set; }
         [Display(Name ="Mô tả")]      
        [StringLength(150,ErrorMessage ="{0} phải từ {2} đến {1} ký tự",MinimumLength =1)]
        public string Description { get; set; }

         [Display(Name ="Địa chỉ")]
        [StringLength(255,ErrorMessage ="{0} phải nhỏ hơn {1} ký tự")]
        public string Address{get;set;}

        [Display(Name ="Ngày tạo")]
        [DataType(DataType.Date)]
        public DateTime DateCreate{get;set;}=DateTime.Now;
    }
}