using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicManagement.Models
{
    public class prescriptions
    {
        [Key]
        public int ID{get;set;}

         [Display(Name ="Mã đơn thuốc")]
        public string code{get;set;}="DT"+DateTime.Now.ToString("yyyyMMddHHmmss");

        [Display(Name ="Bệnh nhân")]

        public int PatientID{get;set;}
        [ForeignKey("PatientID")]
        public virtual Patient Patient{get;set;}


        [Display(Name ="Bác sĩ")]
          public int? DoctorID{get;set;}
        [ForeignKey("DoctorID")]
        public virtual Doctor Doctor{get;set;}


        [Display(Name ="Ngày tạo")]
        [DataType(DataType.Date)]
        public DateTime? DateCreate{get;set;}  =DateTime.Now;

        [Display(Name ="Ngày tái khám")]
        [DataType(DataType.Date)]
        public DateTime? NextVisit{get;set;} 

        [Display(Name ="Chuẩn đoán")]
        [StringLength(255,ErrorMessage ="{0} phải từ 0 đến {1} ký tự")]
        public string advice { get; set; }


         [Display(Name ="Biểu hiện lâm sàng")]
        [StringLength(200,ErrorMessage ="{0} phải từ 0 đến {1} ký tự")]
        public string Note { get; set; }


      public virtual List<PrescriptionDetail> PrescriptionsDetail{get;set;}=new List<PrescriptionDetail>();
      public bool isdeleted{get;set;}
        
    }
}