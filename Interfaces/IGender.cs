using ClinicManagement.Models;
using ClinicManagement.Tools;

namespace ClinicManagement.Interfaces
{
    public interface IGender
    {
        
        PaginatedList<Gender> GetItems(string SortProperty,SortOrder sortOrder, string SearchText="", int pageIndex = 1, int pageSize = 5); //read all

        Gender Create(Gender gender);

        Gender Edit(Gender gender);

        Gender Delete(Gender gender);

        public bool IsGenderExists(string name);
        public bool IsGenderExists(string name, int Id);

        Gender GetGender(int id);

    }
}