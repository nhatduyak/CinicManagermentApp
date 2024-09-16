using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace ClinicManagement.Models
{
    public class AppUser : IdentityUser
    {
          public int? DoctorID { get; set; }
          [ForeignKey("DoctorID")]
          public virtual Doctor Doctor{get;set;}

        //   // [Required]       
        //   [DataType(DataType.Date)]
        //   public DateTime? BirthDate { get; set; }
    }
}