using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicManagement.Models
{
    public class MedicinesCategory
    {
        public int MedicinesID{get;set;}
        [ForeignKey("MedicinesID")]
        public virtual Medicines Medicines{get;set;}


         public int CategoryID{get;set;}
        [ForeignKey("CategoryID")]
        public virtual Category Category{get;set;}
    }
}