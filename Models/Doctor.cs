using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicManagement.Models
{
    public class Doctor
    {
        [Key]
        public int ID { get; set; }

        [Display(Name ="Tên")]
        [Required(ErrorMessage ="phải nhập {0}")]
        [StringLength(20,ErrorMessage ="{0} phải từ {2} đến {1} ký tự",MinimumLength =1)]
        public string FName { get; set; }

        [Display(Name ="Họ")]
        [Required(ErrorMessage ="phải nhập {0}")]
        [StringLength(20,ErrorMessage ="{0} phải từ {2} đến {1} ký tự",MinimumLength =1)]
        public string LName { get; set; }

        [Display(Name ="Tuổi")]
        public int? Age { get; set; }

            [Display(Name ="Điện thoại")]
        // [Required(ErrorMessage ="phải nhập {0}")]
        [Phone(ErrorMessage ="Điện thoại không hợp lệ")]
        public string Phone { get; set; }
        [Display(Name ="Email")]
        // [Required(ErrorMessage ="phải nhập {0}")]
        // [StringLength(60,ErrorMessage ="{0} phải từ {2} đến {1} ký tự",MinimumLength =3)]
        [EmailAddress(ErrorMessage ="Địa chỉ Email không hợp lệ")]
        public string Email { get; set; }

        [Display(Name ="Ngày Đăng ký")]
        [DataType(DataType.Date)]
        public DateTime Registed_Date { get; set; }=DateTime.Now;
       
        [Display(Name ="Giới tính")]

        public int? GenderID { get; set; }
        [ForeignKey("GenderID")]
        public virtual Gender gender{set;get;}

        [Display(Name ="Nhóm máu")]
         public int? BloodGroupID { get; set; }
        [ForeignKey("BloodGroupID")]
        public virtual BloodGroup boolGroup{set;get;}

        public int? AddressID { get; set; }
        [ForeignKey("AddressID")]
        public virtual Address address{set;get;}


        public bool Doctor_Status{get;set;}

        public string ImatePath{get;set;}

        [Display(Name ="Tên bác sĩ")]

        [NotMapped]
        public string FullName 
        {
            get { return LName + " " +FName ;}
        }

    }
}