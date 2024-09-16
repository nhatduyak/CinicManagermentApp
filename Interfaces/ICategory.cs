using System.Collections.Generic;
using System.Threading.Tasks;
using ClinicManagement.Models;
using ClinicManagement.Tools;

namespace ClinicManagement.Interfaces
{
    public interface ICategory
    {
        PaginatedList<Category> GetItems(string SortProperty,SortOrder sortOrder,string SearchText="",int pageIndex=1,int pageSize=5);

        Category GetCategory(int? id);

        Category Create(Category category);

        Category Edit(Category category);

        Category Delete(Category category);

        public bool IsCategoryNameExits(string name);

        public bool IsCategoryNameExits(string name,int id);
            public List<Category> GetChildCategory(int? parentId);
        List<Category> GetItemsSelectlist();

    }
}