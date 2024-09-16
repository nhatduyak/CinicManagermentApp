using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicManagement.Models
{
    public class PrescriptionDetail
    {
      [Key]
        public int ID { get; set; }

        [ForeignKey("PrescriptionsID")]
        public int PrescriptionsID { get; set; }
        public virtual prescriptions Prescriptions{get;private set;}

         [Display(Name ="STT")]
        public int CountNum{get;set;}


         public int MedicinesID { get; set; }
        [ForeignKey("MedicinesID")]
        public virtual Medicines Medicines{get;set;}


          [Display(Name ="Số ngày Uống")]
          [Range(1,30,ErrorMessage ="Vui lòng nhập vào số ngày uống")]
        public int No_of_day{get;set;}


        [Display(Name ="Uống trước bữa ăn?")]

        public bool is_Before_Meal{get;set;}

        [Display(Name ="Uống Khi nào?")]
        public string When_To_Take{get;set;}


      [Display(Name ="Lượng dùng mỗi lần")]
        public string Dosage{get;set;}

        [NotMapped]
        public bool IsDeleted{get;set;}=false;

    }
}