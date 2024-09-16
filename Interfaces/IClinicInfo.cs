using ClinicManagement.Models;
using ClinicManagement.Tools;

namespace ClinicManagement.Interfaces
{
    public interface IClinicInfo
    {
        
        
        PaginatedList<ClinicInfo> GetItems(string SortProperty,SortOrder sortOrder, string SearchText="", int pageIndex = 1, int pageSize = 5); //read all

        ClinicInfo Create(ClinicInfo clinicInfo);

        ClinicInfo Edit(ClinicInfo clinicInfo);

        ClinicInfo Delete(ClinicInfo clinicInfo);

      

        ClinicInfo GetClinicInfo(int id);


    }
}