using System.Collections.Generic;
using System.Linq;
using ClinicManagement.Interfaces;
using ClinicManagement.Models;
using ClinicManagement.Tools;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagement.Repositories
{
    public class DoctorRepository:IDoctor
    {
        private readonly ClinicManagementDbContext _context;
        public DoctorRepository(ClinicManagementDbContext context)
        {
            _context=context;
        }
        public Doctor Create(Doctor doctor)
        {
                _context.Doctors.Add(doctor);
                _context.SaveChanges();
                return doctor;
        }

        public Doctor Delete(Doctor doctor)
        {
            _context.Attach(doctor);
            _context.Entry(doctor).State=EntityState.Deleted;
            _context.SaveChanges();
            return doctor;
        }

        public Doctor Edit(Doctor doctor)
        {
             _context.Doctors.Attach(doctor);
            _context.Entry(doctor).State = EntityState.Modified;
            _context.SaveChanges();
            return doctor;
        }
        private List<Doctor> DoSort(List<Doctor> Doctor, string SortProperty, SortOrder sortOrder)
        {           

            if (SortProperty.ToLower() == "fname")
            {
                if (sortOrder == SortOrder.Ascending)
                    Doctor = Doctor.OrderBy(n => n.FName).ToList();
                else
                    Doctor = Doctor.OrderByDescending(n => n.FName).ToList();
            }
            else if(SortProperty.ToLower()=="age")
            {
                if (sortOrder == SortOrder.Ascending)
                    Doctor = Doctor.OrderBy(d => d.Age).ToList();
                else
                    Doctor = Doctor.OrderByDescending(d => d.Age).ToList();
            }
            else
            {
                if (sortOrder == SortOrder.Ascending)
                    Doctor = Doctor.OrderBy(d => d.LName).ToList();
                else
                    Doctor = Doctor.OrderByDescending(d => d.LName).ToList();
            }
          

            return Doctor;
        }
        public PaginatedList<Doctor> GetItems(string SortProperty, SortOrder sortOrder, string SearchText = "", int pageIndex = 1, int pageSize = 5)
        {
            List<Doctor> Doctor;

            if (SearchText != "" && SearchText!=null)
            {
                Doctor = _context.Doctors.Where(n => n.FName.Contains(SearchText) || n.LName.Contains(SearchText)||n.Age.ToString()==SearchText)
                    .ToList();            
            }
            else
                Doctor= _context.Doctors.ToList();

            Doctor = DoSort(Doctor,SortProperty,sortOrder);
            
            PaginatedList<Doctor> retDoctor = new PaginatedList<Doctor>(Doctor, pageIndex, pageSize);

            return retDoctor;
        }             

   
        public Doctor GetDoctor(int? id)
        {
              Doctor doctor = _context.Doctors.Where(u => u.ID == id).FirstOrDefault();
                return doctor;            
        }

        public bool IsDoctorNameExists(string name)
        {
            bool result= _context.Doctors.Any(c=>c.FName==name);
            return result;        
        }

        public bool IsDoctorNameExists(string name, int Id)
        {
        bool result= _context.Doctors.Any(c=>c.FName==name && c.ID==Id);
            return result;        
        }
        

        public List<Gender> GetGenderList()
        {
            var gender = _context.Genders.ToList();
            gender.Insert(0,new Gender{ID=-1,Name="Không rõ giớ tính"});
            return gender;              
        }

        public List<BloodGroup> GetBloodGroupList()
        {
            var blood = _context.BloodGroups.ToList();
            blood.Insert(0,new BloodGroup{ID=-1,Name="Không rõ nhóm máu"});
            return blood;       
        }
         public Address GetAddress(int? id)
        {
            var addr=_context.addresses.Where(c=>c.ID==id).FirstOrDefault();
            return addr;
        }

    
    }
    
}