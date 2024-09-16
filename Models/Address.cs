using System.ComponentModel.DataAnnotations;

namespace ClinicManagement.Models
{
    public class Address
    {
        [Key]
        public int ID { get; set; }

        [Display(Name ="Tên đường")]
        // [Required(ErrorMessage ="phải nhập {0}")]
        [StringLength(60,ErrorMessage ="{0} phải từ {2} đến {1} ký tự",MinimumLength =3)]
        public string Street { get; set; }

        [Display(Name ="Thành phố")]
        // [Required(ErrorMessage ="phải nhập {0}")]
        [StringLength(60,ErrorMessage ="{0} phải từ {2} đến {1} ký tự",MinimumLength =3)]
        public string City { get; set; }

        [Display(Name ="Tỉnh")]
        // [Required(ErrorMessage ="phải nhập {0}")]
         [StringLength(20,ErrorMessage ="{0} phải từ {2} đến {1} ký tự",MinimumLength =3)]
        //[EmailAddress(ErrorMessage ="Địa chỉ Email không hợp lệ")]
        public string Province { get; set; }


        [Display(Name ="Quốc gia")]
        // [Required(ErrorMessage ="phải nhập {0}")]
        [StringLength(20,ErrorMessage ="{0} phải từ {2} đến {1} ký tự",MinimumLength =3)]

        public string Country { get; set; }

        [Display(Name ="Mã bưu điện")]

        public string Post_Code{get;set;}

    }


}