using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicManagement.Models
{
    public class PaymentDetail
    {
        [Key]
        public int ID { get; set; }





        public int PaymentHeaderID{get;set;}
        [ForeignKey("PaymentHeaderID")]
        public virtual PaymentHeader paymentHeader{get;set;}

        public int MedicinesID{get;set;}
         [ForeignKey("MedicinesID")]
        public virtual Medicines Medicines{get;set;}

        public int Quantity{set;get;}

         [Display(Name ="Giá bán")]
         [Column(TypeName ="money")]
        [DisplayFormat(DataFormatString ="{0:0.000}",ApplyFormatInEditMode =true)]
        public decimal? SellPrice{get;set;}      

        public bool isdeleted;
    }
}