using ClinicManagement.Models;
using ClinicManagement.Tools;

namespace ClinicManagement.Interfaces
{
    public interface IAddress
    {
        PaginatedList<Address> GetItems(string SortProperty,SortOrder sortOrder, string SearchText="", int pageIndex = 1, int pageSize = 5); //read all
        Address GetAddress(int? id); // read particular item

        Address Create(Address address);

        Address Edit(Address address);

        Address Delete(Address address);


    }
}