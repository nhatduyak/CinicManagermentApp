using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicManagement.Models
{
    public class Medicines
    {
        [Key]
        public int ID { get; set; }

        [Display(Name ="Code")]
        public string Code { get; set; }=DateTime.Now.ToString("yyyyMMddhhmmss");

        [Display(Name ="Tên Thuốc")]
        [Required(ErrorMessage ="phải nhập {0}")]
        [StringLength(50,ErrorMessage ="{0} phải từ {2} đến {1} ký tự",MinimumLength =3)]
        public string Name { get; set; }

        [Display(Name ="Mô tả")]
        [StringLength(150,ErrorMessage ="{0} phải từ {2} đến {1} ký tự",MinimumLength =3)]
        public string Description { get; set; }

        public virtual List<MedicinesCategory> MedicinesCatogorys{get;set;}

        [Display(Name ="Đơn vị tính")]
        public int? UnitID{get;set;}
        [ForeignKey("UnitID")]
        public virtual Unit Units{get;set;}

         [Display(Name ="Nhà sản xuất")]
        public int? ManufactureId{get;set;}
        [ForeignKey("ManufactureId")]
        public virtual Manufacture Manufacture{get;set;}

        [Display(Name ="Danh mục")]
        public int? CategoryId{get;set;}
        [ForeignKey("CategoryId")]
        public virtual Category Category{get;set;}
  
        [Display(Name ="Đơn giá")]
        [Column(TypeName ="money")]
        // [DisplayFormat(DataFormatString ="{0:0.000}",ApplyFormatInEditMode =true)]
        [DisplayFormat(DataFormatString ="{0:c}",ApplyFormatInEditMode =true)]

        
        public decimal? UnitPrice{get;set;}

        [Display(Name ="Giá bán")]
        [Column(TypeName ="money")]
        // [DisplayFormat(DataFormatString ="{0:0.000}",ApplyFormatInEditMode =true)]
        [DisplayFormat(DataFormatString ="{0:c}",ApplyFormatInEditMode =true)]

        public decimal? SellPrice{get;set;}      
        [Display(Name ="Số lượng")]
        public int? Quantity{get;set;}

        [Display(Name ="Ngày Hết hạn")]
        [DataType(DataType.Date)]
        public DateTime? ExpiryDate{get;set;}=DateTime.Now;


          [Display(Name ="Đơn giá cũ")]
        [DisplayFormat(DataFormatString ="{0:c}",ApplyFormatInEditMode =true)]
        [Column(TypeName ="money")]
        public decimal? OldUnitPrice{get;set;}

      
        [Display(Name ="Giá bán cũ")]
        [Column(TypeName ="money")]
        [DisplayFormat(DataFormatString ="{0:c}",ApplyFormatInEditMode =true)]
        public decimal? OldSellPrice{get;set;}    


         [Display(Name ="Ngày tạo")]
         [DataType(DataType.Date)]
        public DateTime? DateCreate{get;set;}
        
         [Display(Name ="Ngày Cập Nhật")]
        [DataType(DataType.Date)]
        public DateTime? DateModify{get;set;} =null;

         [Display(Name ="Người tạo")]
        public string UserID{get;set;}
        [ForeignKey("UserID")]
        public virtual AppUser UserCreate{get;set;}
       
        [Display(Name ="Người cập nhật")]
        public string UserIDModify{get;set;}
         [ForeignKey("UserIDModify")]
        public virtual AppUser UserModify{get;set;}

    }
}