using System.Collections.Generic;
using ClinicManagement.Models;
using ClinicManagement.Tools;

namespace ClinicManagement.Interfaces
{
    public interface Iprescriptions
    {
        
        PaginatedList<prescriptions> GetItems(string SortProperty,SortOrder sortOrder, string SearchText="", int pageIndex = 1, int pageSize = 5); //read all
        prescriptions Getprescriptions(int? id); // read particular item

        prescriptions Create(prescriptions prescriptions);

        prescriptions Edit(prescriptions prescriptions);

        prescriptions Delete(prescriptions prescriptions);

        List<Patient> GetPatientList();

        List<Doctor> GetDoctorList();

        List<Medicines> GetMedicinesList();


        Patient GetPatient(int? id);
      
        public ClinicInfo GetClinicInfo();

    }
}