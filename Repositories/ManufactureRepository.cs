using System.Collections.Generic;
using System.Linq;
using ClinicManagement.Interfaces;
using ClinicManagement.Models;
using ClinicManagement.Tools;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagement.Repositories
{
    public class ManufactureRepository : IManufacture
    {
        private readonly ClinicManagementDbContext _context;
        public ManufactureRepository(ClinicManagementDbContext context)
        {
            _context=context;
        }
        public Manufacture Create(Manufacture manufacture)
        {
                _context.Manufactures.Add(manufacture);
                _context.SaveChanges();
                return manufacture;
        }

        public Manufacture Delete(Manufacture manufacture)
        {
            _context.Attach(manufacture);
            _context.Entry(manufacture).State=EntityState.Deleted;
            _context.SaveChanges();
            return manufacture;
        }

        public Manufacture Edit(Manufacture manufacture)
        {
             _context.Manufactures.Attach(manufacture);
            _context.Entry(manufacture).State = EntityState.Modified;
            _context.SaveChanges();
            return manufacture;
        }
        private List<Manufacture> DoSort(List<Manufacture> manufactures, string SortProperty, SortOrder sortOrder)
        {           

            if (SortProperty.ToLower() == "name")
            {
                if (sortOrder == SortOrder.Ascending)
                    manufactures = manufactures.OrderBy(n => n.Name).ToList();
                else
                    manufactures = manufactures.OrderByDescending(n => n.Name).ToList();
            }
            else if(SortProperty.ToLower()=="Description")
            {
                if (sortOrder == SortOrder.Ascending)
                    manufactures = manufactures.OrderBy(d => d.Description).ToList();
                else
                    manufactures = manufactures.OrderByDescending(d => d.Description).ToList();
            }
            else
            {
                if (sortOrder == SortOrder.Ascending)
                    manufactures = manufactures.OrderBy(d => d.DateCreate).ToList();
                else
                    manufactures = manufactures.OrderByDescending(d => d.DateCreate).ToList();
            }

            return manufactures;
        }
        public PaginatedList<Manufacture> GetItems(string SortProperty, SortOrder sortOrder, string SearchText = "", int pageIndex = 1, int pageSize = 5)
        {
            List<Manufacture> manufactures;

            if (SearchText != "" && SearchText!=null)
            {
                manufactures = _context.Manufactures.Where(n => n.Name.Contains(SearchText) || n.Description.Contains(SearchText))
                    .ToList();            
            }
            else
                manufactures= _context.Manufactures.ToList();

            manufactures = DoSort(manufactures,SortProperty,sortOrder);
            
            PaginatedList<Manufacture> retManufacture = new PaginatedList<Manufacture>(manufactures, pageIndex, pageSize);

            return retManufacture;
        }

             

        public bool IsManufactureNameExists(string name)
        {
            bool result= _context.Manufactures.Any(c=>c.Name==name);
            return result;        
        }

        public bool IsManufactureNameExists(string name, int Id)
        {
            bool result= _context.Manufactures.Any(c=>c.Name==name && c.ID==Id);
            return result; 
        } 

        public Manufacture GetManufacture(int? id)
        {
            Manufacture manufacture = _context.Manufactures.Where(u => u.ID == id).FirstOrDefault();
            return manufacture;            
        }
    }
}