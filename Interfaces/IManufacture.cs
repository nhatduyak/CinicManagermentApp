using ClinicManagement.Models;
using ClinicManagement.Tools;

namespace ClinicManagement.Interfaces
{
    public interface IManufacture
    {
        PaginatedList<Manufacture> GetItems(string SortProperty,SortOrder sortOrder, string SearchText="", int pageIndex = 1, int pageSize = 5); //read all
        Manufacture GetManufacture(int? id); // read particular item

        Manufacture Create(Manufacture manufacture);

        Manufacture Edit(Manufacture manufacture);

        Manufacture Delete(Manufacture manufacture);

        public bool IsManufactureNameExists(string name);
        public bool IsManufactureNameExists(string name, int Id);


    }
}