using System.Collections.Generic;
using ClinicManagement.Models;
using ClinicManagement.Tools;

namespace ClinicManagement.Interfaces
{
    public interface IPrescriptionDetail
    {
        
        PaginatedList<PrescriptionDetail> GetItems(string SortProperty,SortOrder sortOrder, string SearchText="", int pageIndex = 1, int pageSize = 5); //read all
        Patient GetPatient(int? id); // read particular item

        PrescriptionDetail Create(PrescriptionDetail patient);

        PrescriptionDetail Edit(PrescriptionDetail patient);

        PrescriptionDetail Delete(PrescriptionDetail patient);

        List<Gender> GetGenderList();

        List<BloodGroup> GetBloodGroupList();


        public bool IsPatientNameExists(string name);
        public bool IsPatientNameExists(string name, int Id);

    }
}