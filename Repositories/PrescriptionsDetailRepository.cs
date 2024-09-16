using System.Collections.Generic;
using System.Linq;
using ClinicManagement.Interfaces;
using ClinicManagement.Models;
using ClinicManagement.Tools;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagement.Repositories
{
    public class PrescriptionDetailRepository : IPrescriptionDetail
    {
         private readonly ClinicManagementDbContext _context;
        public PrescriptionDetailRepository(ClinicManagementDbContext context)
        {
            _context=context;
        }
        public PrescriptionDetail Create(PrescriptionDetail PrescriptionDetail)
        {
                _context.prescriptionsDetails.Add(PrescriptionDetail);
                _context.SaveChanges();
                return PrescriptionDetail;
        }

        public PrescriptionDetail Delete(PrescriptionDetail PrescriptionDetail)
        {
            _context.Attach(PrescriptionDetail);
            _context.Entry(PrescriptionDetail).State=EntityState.Deleted;
            _context.SaveChanges();
            return PrescriptionDetail;
        }

        public PrescriptionDetail Edit(PrescriptionDetail PrescriptionDetail)
        {
             _context.prescriptionsDetails.Attach(PrescriptionDetail);
            _context.Entry(PrescriptionDetail).State = EntityState.Modified;
            _context.SaveChanges();
            return PrescriptionDetail;
        }
        private List<PrescriptionDetail> DoSort(List<PrescriptionDetail> PrescriptionDetail, string SortProperty, SortOrder sortOrder)
        {           

            if (SortProperty.ToLower() == "name")
            {
                if (sortOrder == SortOrder.Ascending)
                    PrescriptionDetail = PrescriptionDetail.OrderBy(n => n.Medicines.Name).ToList();
                else
                    PrescriptionDetail = PrescriptionDetail.OrderByDescending(n =>n.Medicines.Name).ToList();
            }
            else if(SortProperty.ToLower()=="datecreate")
            {
                if (sortOrder == SortOrder.Ascending)
                    PrescriptionDetail = PrescriptionDetail.OrderBy(d => d.Prescriptions.DateCreate).ToList();
                else
                    PrescriptionDetail = PrescriptionDetail.OrderByDescending(d => d.Prescriptions.DateCreate).ToList();
            }
            else
            {
                if (sortOrder == SortOrder.Ascending)
                    PrescriptionDetail = PrescriptionDetail.OrderBy(d => d.Prescriptions.Patient.FName).ToList();
                else
                    PrescriptionDetail = PrescriptionDetail.OrderByDescending(d => d.Prescriptions.Patient.FName).ToList();
            }
          

            return PrescriptionDetail;
        }
        public PaginatedList<PrescriptionDetail> GetItems(string SortProperty, SortOrder sortOrder, string SearchText = "", int pageIndex = 1, int pageSize = 10)
        {
            List<PrescriptionDetail> PrescriptionDetail;

            if (SearchText != "" && SearchText!=null)
            {
                PrescriptionDetail = _context.prescriptionsDetails.Where(n => n.Medicines.Name.Contains(SearchText) || n.Prescriptions.Patient.FName.Contains(SearchText)|| n.Prescriptions.Patient.LName.Contains(SearchText))
                    .ToList();            
            }
            else
                PrescriptionDetail= _context.prescriptionsDetails.ToList();

            PrescriptionDetail = DoSort(PrescriptionDetail,SortProperty,sortOrder);
            
            PaginatedList<PrescriptionDetail> retPrescriptionDetail = new PaginatedList<PrescriptionDetail>(PrescriptionDetail, pageIndex, pageSize);

            return retPrescriptionDetail;
        }             

   
        public PrescriptionDetail GetPrescriptionDetail(int? id)
        {
              PrescriptionDetail PrescriptionDetail = _context.prescriptionsDetails.Where(u => u.ID == id).FirstOrDefault();
            return PrescriptionDetail;            
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

        public Patient GetPatient(int? id)
        {
             Patient patient = _context.Patients.Where(u => u.ID == id).FirstOrDefault();
            return patient;       
        }

        public bool IsPatientNameExists(string name)
        {
            throw new System.NotImplementedException();
        }

        public bool IsPatientNameExists(string name, int Id)
        {
            throw new System.NotImplementedException();
        }

       
    }
}