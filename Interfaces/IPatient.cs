using System.Collections.Generic;
using ClinicManagement.Models;
using ClinicManagement.Tools;

namespace ClinicManagement.Interfaces
{
    public interface IPatient
    {
        PaginatedList<Patient> GetItems(string SortProperty,SortOrder sortOrder, string SearchText="", int pageIndex = 1, int pageSize = 5); //read all
        Patient GetPatient(int? id); // read particular item

        Patient Create(Patient patient);

        Patient Edit(Patient patient);

        Patient Delete(Patient patient);

        List<prescriptions> GetPrescriptions(int? id);
        List<Medicines> GetMedicines();

        List<Gender> GetGenderList();

        List<BloodGroup> GetBloodGroupList();

        Address GetAddress(int? id);

        public bool IsPatientNameExists(string name);
        public bool IsPatientNameExists(string name, int Id);

    }
}