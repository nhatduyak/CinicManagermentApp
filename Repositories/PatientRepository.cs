using System.Collections.Generic;
using System.Linq;
using ClinicManagement.Interfaces;
using ClinicManagement.Models;
using ClinicManagement.Tools;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagement.Repositories
{
    public class PatientRepository:IPatient
    {
        private readonly ClinicManagementDbContext _context;
        public PatientRepository(ClinicManagementDbContext context)
        {
            _context=context;
        }
        public Patient Create(Patient patient)
        {
                _context.Patients.Add(patient);
                _context.SaveChanges();
                return patient;
        }

        public Patient Delete(Patient patient)
        {
            _context.Attach(patient);
            _context.Entry(patient).State=EntityState.Deleted;
            _context.SaveChanges();
            return patient;
        }

        public Patient Edit(Patient patient)
        {
             _context.Patients.Attach(patient);
            _context.Entry(patient).State = EntityState.Modified;
            _context.SaveChanges();
            return patient;
        }
        private List<Patient> DoSort(List<Patient> Patient, string SortProperty, SortOrder sortOrder)
        {           

            if (SortProperty.ToLower() == "fname")
            {
                if (sortOrder == SortOrder.Ascending)
                    Patient = Patient.OrderBy(n => n.FName).ToList();
                else
                    Patient = Patient.OrderByDescending(n => n.FName).ToList();
            }
            else if(SortProperty.ToLower()=="age")
            {
                if (sortOrder == SortOrder.Ascending)
                    Patient = Patient.OrderBy(d => d.Age).ToList();
                else
                    Patient = Patient.OrderByDescending(d => d.Age).ToList();
            }
            else
            {
                if (sortOrder == SortOrder.Ascending)
                    Patient = Patient.OrderByDescending(d => d.Registed_Date).ToList();
                else
                    Patient = Patient.OrderBy(d => d.Registed_Date).ToList();
            }
          

            return Patient;
        }
        public PaginatedList<Patient> GetItems(string SortProperty, SortOrder sortOrder, string SearchText = "", int pageIndex = 1, int pageSize = 5)
        {
            List<Patient> Patient;

            if (SearchText != "" && SearchText!=null)
            {
                Patient = _context.Patients.Where(n => n.FName.Contains(SearchText) || n.LName.Contains(SearchText)||n.Age.ToString()==SearchText)
                    .ToList();            
            }
            else
                Patient= _context.Patients.ToList();

            Patient = DoSort(Patient,SortProperty,sortOrder);
            
            PaginatedList<Patient> retPatient = new PaginatedList<Patient>(Patient, pageIndex, pageSize);

            return retPatient;
        }             

   
        public Patient GetPatient(int? id)
        {
              Patient patient = _context.Patients.Where(u => u.ID == id).FirstOrDefault();
            return patient;            
        }

        public bool IsPatientNameExists(string name)
        {
            bool result= _context.Patients.Any(c=>c.FName==name);
            return result;        
        }

        public bool IsPatientNameExists(string name, int Id)
        {
        bool result= _context.Patients.Any(c=>c.FName==name && c.ID==Id);
            return result;         }

       

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

        public List<prescriptions> GetPrescriptions(int? id)
        {
            var result= _context.Prescriptions.Where(p=>p.PatientID==id && p.isdeleted==false).OrderByDescending(d=>d.DateCreate).ToList();
            return result;
        }

        public List<Medicines> GetMedicines()
        {
            return _context.Medicines.ToList();
        }
    }
}