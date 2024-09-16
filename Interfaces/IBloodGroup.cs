using ClinicManagement.Models;
using ClinicManagement.Tools;

namespace ClinicManagement.Interfaces
{
    public interface IBloodGroup
    {
         PaginatedList<BloodGroup> GetItems(string SortProperty,SortOrder sortOrder, string SearchText="", int pageIndex = 1, int pageSize = 5); //read all
        BloodGroup GetBlooGroup(int id); // read particular item

        BloodGroup Create(BloodGroup boolGroup);

        BloodGroup Edit(BloodGroup boolGroup);

        BloodGroup Delete(BloodGroup boolGroup);

        public bool IsBloodGroupNameExists(string name);
        public bool IsBloodGroupNameExists(string name, int Id);
    }
}