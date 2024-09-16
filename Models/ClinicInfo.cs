using System.ComponentModel.DataAnnotations;

namespace ClinicManagement.Models
{
    public class ClinicInfo
    {
        [Key]
        public int ID { get; set; }

        [Display(Name ="Tên Phòng khám")]
        // [Required(ErrorMessage ="phải nhập {0}")]
        [StringLength(60,ErrorMessage ="{0} phải từ {2} đến {1} ký tự",MinimumLength =3)]
        public string Name { get; set; }

        [Display(Name ="Địa chỉ")]
        // [Required(ErrorMessage ="phải nhập {0}")]
        [StringLength(60,ErrorMessage ="{0} phải từ {2} đến {1} ký tự",MinimumLength =3)]
        public string Address { get; set; }

        [Display(Name ="Email")]
        // [Required(ErrorMessage ="phải nhập {0}")]
        // [StringLength(60,ErrorMessage ="{0} phải từ {2} đến {1} ký tự",MinimumLength =3)]
        [EmailAddress(ErrorMessage ="Địa chỉ Email không hợp lệ")]
        public string Email { get; set; }


        [Display(Name ="Điện thoại")]
        // [Required(ErrorMessage ="phải nhập {0}")]
        [Phone(ErrorMessage ="Điện thoại không hợp lệ")]
        public string Phone { get; set; }

    }


}