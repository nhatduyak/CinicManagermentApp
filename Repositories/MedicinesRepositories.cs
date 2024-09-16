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
    public class MedicinesRepositories : IMedicines
    {
        public readonly ClinicManagementDbContext _context;
        public MedicinesRepositories(ClinicManagementDbContext context)
        {
            _context=context;
        }
        public Medicines Create(Medicines medicines)
        {
            _context.Medicines.Add(medicines);
            _context.SaveChanges();
            return medicines;
        }

        public Medicines Delete(Medicines medicines)
        {
            _context.Attach(medicines);
            _context.Entry(medicines).State=EntityState.Deleted;
            _context.SaveChanges();
            return medicines;
        }

        public Medicines Edit(Medicines medicines)
        {
            _context.Attach(medicines);
            _context.Entry(medicines).State=EntityState.Modified;
            _context.SaveChanges();
            return medicines;
        }

      

        public PaginatedList<Medicines> GetItems(string SortProperty, SortOrder sortOrder, string SearchText = "", int pageIndex = 1, int pageSize = 5)
        {
            List<Medicines> medicines;

            if (SearchText != "" && SearchText!=null)
            {
                medicines = _context.Medicines                                 
                            .Where(n => (n.Name.Contains(SearchText) || n.Description.Contains(SearchText)))
                            .ToList();            
            }
            else
            {
                    medicines= _context.Medicines                                   
                                   .ToList();
            }
                

            medicines = DoSort(medicines,SortProperty,sortOrder);
            
            PaginatedList<Medicines> retCategories = new PaginatedList<Medicines>(medicines, pageIndex, pageSize);

            return retCategories;
        }
        public List<Medicines> GetItemsSelectlist()
        {
            //List<Category> categories;

            
                // categories = _context.Categories
                //             .Include(p=>p.ParentCategory)
                //             .Include(c=>c.CategoryChildren)
                //             .Where(n => n.ParentCategory==null)
                //             .ToList();            
           var qr = (from c in _context.Medicines select c)
                     .Include(c => c.Units)
                     .Include(c => c.Manufacture);

            var medicines = (qr.ToList())
                             //.Where(c => c.ParentCategory == null)
                             .ToList();   

            return medicines;
        }

        private List<Medicines> DoSort(List<Medicines> medicines,string SortProperty,SortOrder sortOrder)
        {
             if (SortProperty.ToLower() == "name")
            {
                if (sortOrder == SortOrder.Ascending)
                    medicines = medicines.OrderBy(n => n.Name).ToList();
                else
                    medicines = medicines.OrderByDescending(n => n.Name).ToList();
            }
            else if(SortProperty.ToLower()=="code")
            {
                if (sortOrder == SortOrder.Ascending)
                    medicines = medicines.OrderBy(d => d.Code).ToList();
                else
                    medicines = medicines.OrderByDescending(d => d.Code).ToList();
            }
            else if(SortProperty.ToLower()=="Category")
            {
                if (sortOrder == SortOrder.Ascending)
                    medicines = medicines.OrderBy(d => d.Category.Name).ToList();
                else
                    medicines = medicines.OrderByDescending(d => d.Category.Name).ToList();
            }
            else if(SortProperty.ToLower()=="unitprice")
            {
                if (sortOrder == SortOrder.Ascending)
                    medicines = medicines.OrderBy(d => d.UnitPrice).ToList();
                else
                    medicines = medicines.OrderByDescending(d => d.UnitPrice).ToList();
            }
            else if(SortProperty.ToLower()=="sellprice")
            {
                if (sortOrder == SortOrder.Ascending)
                    medicines = medicines.OrderBy(d => d.SellPrice).ToList();
                else
                    medicines = medicines.OrderByDescending(d => d.SellPrice).ToList();
            }
             else if(SortProperty.ToLower()=="quantity")
            {
                if (sortOrder == SortOrder.Ascending)
                    medicines = medicines.OrderBy(d => d.Quantity).ToList();
                else
                    medicines = medicines.OrderByDescending(d => d.Quantity).ToList();
            }
             else if(SortProperty.ToLower()=="expirydate")
            {
                if (sortOrder == SortOrder.Ascending)
                    medicines = medicines.OrderBy(d => d.ExpiryDate).ToList();
                else
                    medicines = medicines.OrderByDescending(d => d.ExpiryDate).ToList();
            }
            else
            {
                if (sortOrder == SortOrder.Ascending)
                    medicines = medicines.OrderBy(d => d.DateCreate).ToList();
                else
                    medicines = medicines.OrderByDescending(d => d.DateCreate).ToList();
            }

            return medicines;
        }

        public bool IsMedicinesIdExits(int Id)
        {
            bool result= _context.Medicines.Any(c=>c.ID==Id);
            return result;
        }

        public bool IsMedicinesNameExits(string name, int id)
        {
            bool result= _context.Medicines.Any(c=>c.Name==name && c.ID==id);
            return result;       
        }
        public async Task<Medicines> GetMedicines(int? id)
        {
            // Category category=_context.Categories.Where(c=>c.ID==id).FirstOrDefault();
            //  var kq = _context.Medicines
            //                 .Include(p=>p.Units)
            //                 .Include(c=>c.Manufacture);
            //   Category  category=kq.ToList()            
            //                 .Where(c=>c.ID==id)
            //                 .FirstOrDefault();        
            Medicines medicines=await _context.Medicines.Where(c=>c.ID==id).FirstOrDefaultAsync();    
            return medicines;
        }

        public List<Unit> GetUnitsSelected()
        {
            var units=_context.Units.ToList();
            units.Insert(0,new Unit{ID=-1,Name="Không rõ đơn vị tính"});
            return units;
        }

        public List<Manufacture> GetManufactureSelected()
        {
            var Manufacture=_context.Manufactures.ToList();
            Manufacture.Insert(0,new Manufacture{ID=-1,Name="Không rõ nhà sản xuất"});
            return Manufacture;
        }

        public List<Category> GetCategorySelected()
        {
             var qr = (from c in _context.Categories select c)
                     .Include(c => c.ParentCategory)
                     .Include(c => c.CategoryChildren);

            var categories = qr.ToList()
                             .Where(c => c.ParentCategory == null)
                             .ToList();   
            categories.Insert(0, new Category(){
                ID = -1,
                Name = "Không có danh mục cha"
            });
            var items = new List<Category>();
            CreateSelectItems(categories, items, 0);
            return items;
        }

         private void CreateSelectItems(List<Category> source, List<Category> des, int level)
        {
            string prefix = string.Concat(Enumerable.Repeat("----", level));
            foreach (var category in source)
            {
                // category.Title = prefix + " " + category.Title;
                des.Add(new Category() {
                    ID = category.ID,
                    Name = prefix + " " + category.Name
                });
                if (category.CategoryChildren?.Count > 0)
                {
                    CreateSelectItems(category.CategoryChildren.ToList(), des, level +1);
                }
            }
        }



        // public List<Category> GetChildCategory(int? parentId)
        // {

        //          var qr = (from c in _context.Categories select c)
        //              .Include(c => c.ParentCategory)
        //              .Include(c => c.CategoryChildren);

        //     var categories = (qr.ToList())
        //                      .Where(c => c.ParentCategoryID == parentId)
        //                      .ToList();   

        //         return categories;
        // }
    }
}