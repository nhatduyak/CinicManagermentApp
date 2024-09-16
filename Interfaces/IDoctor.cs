using System.Collections.Generic;
using ClinicManagement.Models;
using ClinicManagement.Tools;

namespace ClinicManagement.Interfaces
{
    public interface IDoctor
    {
        PaginatedList<Doctor> GetItems(string SortProperty,SortOrder sortOrder, string SearchText="", int pageIndex = 1, int pageSize = 5); //read all
        Doctor GetDoctor(int? id); // read particular item

        Doctor Create(Doctor doctor);

        Doctor Edit(Doctor doctor);

        Doctor Delete(Doctor doctor);

        public List<Gender> GetGenderList();

        public List<BloodGroup> GetBloodGroupList();
         public Address GetAddress(int? id);
       
     

    }
}