using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClinicManagement.Interfaces;
using ClinicManagement.Models;
using ClinicManagement.Tools;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagement.Repositories
{
    public class CategoryRepository : ICategory
    {
        public readonly ClinicManagementDbContext _context;
        public CategoryRepository(ClinicManagementDbContext context)
        {
            _context=context;
        }
        public Category Create(Category category)
        {
            _context.Categories.Add(category);
            _context.SaveChanges();
            return category;
        }

        public Category Delete(Category category)
        {
            _context.Attach(category);
            _context.Entry(category).State=EntityState.Deleted;
            _context.SaveChanges();
            return category;
        }

        public Category Edit(Category category)
        {
            //_context.Attach(category);
            var cate=_context.Categories.Where(p=>p.ID==category.ID).FirstOrDefault();
            _context.Entry(cate).State=EntityState.Detached;
            _context.Entry(category).State=EntityState.Modified;
            _context.SaveChanges();
            return category;
        }

        public Category GetCategory(int? id)
        {
            // Category category=_context.Categories.Where(c=>c.ID==id).FirstOrDefault();
             var kq = _context.Categories
                            .Include(p=>p.ParentCategory)
                            .Include(c=>c.CategoryChildren);
              Category  category=kq.ToList()            
                            .Where(c=>c.ID==id)
                            .FirstOrDefault();            
            return category;
        }

        public PaginatedList<Category> GetItems(string SortProperty, SortOrder sortOrder, string SearchText = "", int pageIndex = 1, int pageSize = 5)
        {
            List<Category> categories;

            if (SearchText != "" && SearchText!=null)
            {
                var kq = _context.Categories
                            .Include(p=>p.ParentCategory)
                            .Include(c=>c.CategoryChildren);
                categories=kq.ToList()            
                            .Where(n => n.ParentCategory==null&&(n.Name.Contains(SearchText) || n.Descriptions.Contains(SearchText)))
                            .ToList();            
            }
            else
            {
                    var kq= _context.Categories
                                    .Include(p=>p.ParentCategory)
                                    .Include(c=>c.CategoryChildren);
                     categories=kq.ToList()
                                  .Where(n => n.ParentCategory==null)
                                   .ToList();
            }
                

            categories = DoSort(categories,SortProperty,sortOrder);
            
            PaginatedList<Category> retCategories = new PaginatedList<Category>(categories, pageIndex, pageSize);

            return retCategories;
        }
        public List<Category> GetItemsSelectlist()
        {
            //List<Category> categories;

            
                // categories = _context.Categories
                //             .Include(p=>p.ParentCategory)
                //             .Include(c=>c.CategoryChildren)
                //             .Where(n => n.ParentCategory==null)
                //             .ToList();            
           var qr = (from c in _context.Categories select c)
                     .Include(c => c.ParentCategory)
                     .Include(c => c.CategoryChildren);

            var categories = (qr.ToList())
                             .Where(c => c.ParentCategory == null)
                             .ToList();   

            return categories;
        }

        private List<Category> DoSort(List<Category> categories,string SortProperty,SortOrder sortOrder)
        {
             if (SortProperty.ToLower() == "name")
            {
                if (sortOrder == SortOrder.Ascending)
                    categories = categories.OrderBy(n => n.Name).ToList();
                else
                    categories = categories.OrderByDescending(n => n.Name).ToList();
            }
            else if(SortProperty.ToLower()=="description")
            {
                if (sortOrder == SortOrder.Ascending)
                    categories = categories.OrderBy(d => d.Descriptions).ToList();
                else
                    categories = categories.OrderByDescending(d => d.Descriptions).ToList();
            }
            else
            {
                if (sortOrder == SortOrder.Ascending)
                    categories = categories.OrderBy(d => d.ParentCategory).ToList();
                else
                    categories = categories.OrderByDescending(d => d.ParentCategory).ToList();
            }

            return categories;
        }

        public bool IsCategoryNameExits(string name)
        {
            bool result= _context.Categories.Any(c=>c.Name==name);
            return result;
        }

        public bool IsCategoryNameExits(string name, int id)
        {
            bool result= _context.Categories.Any(c=>c.Name==name && c.ID==id);
            return result;       
        }

        public List<Category> GetChildCategory(int? parentId)
        {
            
                 var qr = (from c in _context.Categories select c)
                     .Include(c => c.ParentCategory)
                     .Include(c => c.CategoryChildren);

            var categories = (qr.ToList())
                             .Where(c => c.ParentCategoryID == parentId)
                             .ToList();   
                     
                return categories;
        }
    }
}