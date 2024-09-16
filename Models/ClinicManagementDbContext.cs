using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagement.Models
{
    public class ClinicManagementDbContext : IdentityDbContext<AppUser>
    {
        public ClinicManagementDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            base.OnConfiguring(builder);
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {

            base.OnModelCreating(builder);
            // Bỏ tiền tố AspNet của các bảng: mặc định các bảng trong IdentityDbContext có
            // tên với tiền tố AspNet như: AspNetUserRoles, AspNetUser ...
            // Đoạn mã sau chạy khi khởi tạo DbContext, tạo database sẽ loại bỏ tiền tố đó
            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                var tableName = entityType.GetTableName();
                if (tableName.StartsWith("AspNet"))
                {
                    entityType.SetTableName(tableName.Substring(6));
                }

                builder.Entity<MedicinesCategory>(entity=>
                {
                    entity.HasKey(c=>new {c.CategoryID,c.MedicinesID});
                });

                 builder.Entity<MedicinesHistory>(entity=>
                {
                    entity.HasKey(c=>new {c.MedicinesID,c.PaymentHeaderID});
                });

                builder.Entity<Medicines>(enrity=>{
                    enrity.HasIndex(c=>c.Code)
                                    .IsUnique();
                });
                //  builder.Entity<PrescriptionDetail>(entity=>
                // {
                //     entity.HasKey(c=>new {c.ID,c.PrescriptionsID});
                // });
             
        }
        }

        public DbSet<Gender> Genders{get;set;}
        public DbSet<BloodGroup> BloodGroups{get;set;}

        public DbSet<Address> addresses{get;set;}

        public DbSet<ClinicInfo> ClinicInfos{get;set;}

        public DbSet<Patient> Patients{get;set;}

        public DbSet<Unit> Units{get;set;}

        public DbSet<Category> Categories{get;set;}

        public DbSet<Manufacture> Manufactures{get;set;}

        public DbSet<MedicinesCategory> MedicinesCategories{get;set;}

        public DbSet<Medicines> Medicines {get;set;}

        public DbSet<MedicinesHistory> MedicinesHistories{get;set;}

        public DbSet<PaymentHeader> PaymentHeaders{get;set;}
        
        public DbSet<PaymentDetail> paymentDetails{get;set;}

        public DbSet<PrescriptionDetail> prescriptionsDetails{get;set;}

        public DbSet<prescriptions> Prescriptions{get;set;}

        public DbSet<Doctor> Doctors{get;set;}

    }
}