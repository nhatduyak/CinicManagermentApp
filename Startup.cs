using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ClinicManagement.Data;
using ClinicManagement.Interfaces;
using ClinicManagement.Menu;
using ClinicManagement.Models;
using ClinicManagement.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;


namespace ClinicManagement
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddDbContext<ClinicManagementDbContext>(options=>{
                options.UseLazyLoadingProxies()
                        .UseSqlServer(Configuration.GetConnectionString("AppDbContext"));
                        // .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });

              // Dang ky Identity
            services.AddIdentity<AppUser, IdentityRole>()
                    .AddEntityFrameworkStores<ClinicManagementDbContext>()
                    .AddDefaultTokenProviders();

            // services.AddDefaultIdentity<AppUser>()
            //         .AddEntityFrameworkStores<MyBlogContext>()
            //         .AddDefaultTokenProviders();
                    

            // Truy cập IdentityOptions
            services.Configure<IdentityOptions> (options => {
                // Thiết lập về Password
                options.Password.RequireDigit = false; // Không bắt phải có số
                options.Password.RequireLowercase = false; // Không bắt phải có chữ thường
                options.Password.RequireNonAlphanumeric = false; // Không bắt ký tự đặc biệt
                options.Password.RequireUppercase = false; // Không bắt buộc chữ in
                options.Password.RequiredLength = 3; // Số ký tự tối thiểu của password
                options.Password.RequiredUniqueChars = 1; // Số ký tự riêng biệt

                // Cấu hình Lockout - khóa user
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes (5); // Khóa 5 phút
                options.Lockout.MaxFailedAccessAttempts = 3; // Thất bại 3 lầ thì khóa
                options.Lockout.AllowedForNewUsers = true;

                // Cấu hình về User.
                options.User.AllowedUserNameCharacters = // các ký tự đặt tên user
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;  // Email là duy nhất
            

                // Cấu hình đăng nhập.
                options.SignIn.RequireConfirmedEmail = true;            // Cấu hình xác thực địa chỉ email (email phải tồn tại)
                options.SignIn.RequireConfirmedPhoneNumber = false;     // Xác thực số điện thoại
                options.SignIn.RequireConfirmedAccount = true; 
                
            });      

            services.ConfigureApplicationCookie(options => {
                options.LoginPath = "/login/";
                options.LogoutPath = "/logout/";
                options.AccessDeniedPath = "/khongduoctruycap.html";
            });  

              services.AddAuthorization(options => {
                    options.AddPolicy("ViewManageMenu", builder => {
                        builder.RequireAuthenticatedUser();
                        builder.RequireRole(RoleName.Administrator);
                    });
                });
                           // services.AddTransient<CartService>();
services.AddOptions();
            var mailsetting = Configuration.GetSection("MailSettings");
            services.Configure<MailSettings>(mailsetting);
            services.AddSingleton<IEmailSender, SendMailService>();

                            services.AddTransient<IActionContextAccessor, ActionContextAccessor>();
                services.AddTransient<AdminSidebarService>();
                services.AddScoped<IUnit, UnitRepository>();   
                services.AddScoped<ICategory, CategoryRepository>();      
                services.AddScoped<IManufacture, ManufactureRepository>(); 
                services.AddScoped<IMedicines, MedicinesRepositories>();                    
                services.AddScoped<IAddress, AddressRepository>();     
                services.AddScoped<IBloodGroup, BoolGroupRepository>();     
                services.AddScoped<IGender, GenderRepository>();     
                services.AddScoped<IPatient, PatientRepository>();     
                services.AddScoped<IDoctor, DoctorRepository>();     
                services.AddScoped<IClinicInfo, ClinicInfoRemository>(); 
                services.AddTransient<Iprescriptions, PrescriptionsRepositories>(); 
                services.AddTransient<IPrescriptionDetail, PrescriptionDetailRepository>();     

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            // // /contents/1.jpg => Uploads/1.jpg
            // app.UseStaticFiles(new StaticFileOptions() {
            //     FileProvider = new PhysicalFileProvider(
            //         Path.Combine(Directory.GetCurrentDirectory(), "Uploads")
            //     ),
            //     RequestPath = "/contents"
            // });

            // app.UseSession();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
