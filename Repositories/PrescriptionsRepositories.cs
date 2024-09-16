using System;
using System.Collections.Generic;
using System.Linq;
using ClinicManagement.Interfaces;
using ClinicManagement.Models;
using ClinicManagement.Tools;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagement.Repositories
{
    public class PrescriptionsRepositories : Iprescriptions
    {
        private readonly ClinicManagementDbContext _context;
        public PrescriptionsRepositories(ClinicManagementDbContext context)
        {
            _context=context;
        }
       public prescriptions Create(prescriptions prescriptions)
        {

                //_context.Attach(prescriptions);
                _context.Prescriptions.Add(prescriptions);
                Console.WriteLine($"prescriptions {prescriptions.PrescriptionsDetail.Count} value {prescriptions.PrescriptionsDetail?[0].MedicinesID}");
               // _context.prescriptionsDetails.AddRange(prescriptions.PrescriptionsDetail);
                _context.SaveChanges();
                return prescriptions;
        }

        public prescriptions Delete(prescriptions prescriptions)
        {
            _context.Attach(prescriptions);
            _context.Entry(prescriptions).State=EntityState.Deleted;
            //prescriptions.isdeleted=true;
            _context.SaveChanges();
            return prescriptions;
        }

        public prescriptions Edit(prescriptions prescriptions)
        {
            List<PrescriptionDetail> oldprescrDetail=_context.prescriptionsDetails.Where(p=>p.PrescriptionsID==prescriptions.ID).ToList();
            //Console.WriteLine($"so luong oldprdetail {oldprescrDetail.Count}");
            _context.prescriptionsDetails.RemoveRange(oldprescrDetail);
            _context.SaveChanges();

           // Console.WriteLine($"so luong prdetail {prescriptions.PrescriptionsDetail.Count}");
          //  Console.WriteLine($"so luong prdetail true  {prescriptions.PrescriptionsDetail.Where(p=>p.IsDeleted==true).ToList().Count}");
            prescriptions.PrescriptionsDetail.RemoveAll(n=>n.IsDeleted==true);
             _context.Attach(prescriptions);
            _context.Entry(prescriptions).State = EntityState.Modified;
            _context.prescriptionsDetails.AddRange(prescriptions.PrescriptionsDetail);

            _context.SaveChanges();
            return prescriptions;
        }
        private List<prescriptions> DoSort(List<prescriptions> prescriptions, string SortProperty, SortOrder sortOrder)
        {           

            if (SortProperty.ToLower() == "pname")
            {
                if (sortOrder == SortOrder.Ascending)
                    prescriptions = prescriptions.OrderBy(n => n.Patient.FName).ToList();
                else
                    prescriptions = prescriptions.OrderByDescending(n => n.Patient.FName).ToList();
            }
            else if(SortProperty.ToLower()=="dname")
            {
                if (sortOrder == SortOrder.Ascending)
                    prescriptions = prescriptions.OrderBy(d => d.Doctor.FName).ToList();
                else
                    prescriptions = prescriptions.OrderByDescending(d => d.Doctor.FName).ToList();
            }
            else if(SortProperty.ToLower()=="code")
            {
                if (sortOrder == SortOrder.Ascending)
                    prescriptions = prescriptions.OrderBy(d => d.code).ToList();
                else
                    prescriptions = prescriptions.OrderByDescending(d => d.code).ToList();
            }
            else
            {
                if (sortOrder == SortOrder.Ascending)
                    prescriptions = prescriptions.OrderByDescending(d => d.DateCreate).ToList();
                else
                    prescriptions = prescriptions.OrderBy(d => d.DateCreate).ToList();
            }
          

            return prescriptions;
        }
        public PaginatedList<prescriptions> GetItems(string SortProperty, SortOrder sortOrder, string SearchText = "", int pageIndex = 1, int pageSize = 5)
        {
            List<prescriptions> prescriptions;

            if (SearchText != "" && SearchText!=null)
            {
                prescriptions = _context.Prescriptions.Where(n => n.isdeleted==false &&(n.Patient.FName.Contains(SearchText) || n.Patient.LName.Contains(SearchText)||n.Doctor.FName.Contains(SearchText) || n.Doctor.LName.Contains(SearchText)))
                    .ToList();            
            }
            else
                prescriptions= _context.Prescriptions.Where(p=>p.isdeleted==false).ToList();

            prescriptions = DoSort(prescriptions,SortProperty,sortOrder);
            
            PaginatedList<prescriptions> retprescriptions = new PaginatedList<prescriptions>(prescriptions, pageIndex, pageSize);

            return retprescriptions;
        }             

   
        public prescriptions Getprescriptions(int? id)
        {
              prescriptions prescriptions = _context.Prescriptions.Where(u => u.ID == id)
                                                    .FirstOrDefault();
            return prescriptions;            
        }

       
       

        public List<Patient> GetPatientList()
        {
            var patient = _context.Patients.ToList();
            //patient.Insert(0,new Patient{ID=-1,Name="Không rõ giớ tính"});
            return patient;              
        }

        public List<Doctor> GetDoctorList()
        {
            var doctors = _context.Doctors.ToList();
            doctors.Insert(0,new Doctor{ID=-1,FName="Không rõ bác sĩ"});
            return doctors;       
        }

        public Patient GetPatient(int? id)
        {
            var addr=_context.Patients.Where(c=>c.ID==id).FirstOrDefault();
            return addr;
        }

        public List<Medicines> GetMedicinesList()
        {
            var Medicines = _context.Medicines.ToList();
            return Medicines;
        }
        public ClinicInfo GetClinicInfo()
        {
            ClinicInfo info=_context.ClinicInfos.FirstOrDefault();
            return info;
        }
    }
}