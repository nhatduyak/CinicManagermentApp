using System.Collections.Generic;
using System.Linq;
using ClinicManagement.Interfaces;
using ClinicManagement.Models;
using ClinicManagement.Tools;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagement.Repositories
{
    public class GenderRepository:IGender
    {
        private readonly ClinicManagementDbContext _context;
        public GenderRepository(ClinicManagementDbContext context)
        {
            _context=context;
        }
        public Gender Create(Gender gender)
        {
                _context.Genders.Add(gender);
                _context.SaveChanges();
                return gender;
        }

        public Gender Delete(Gender gender)
        {
            _context.Attach(gender);
            _context.Entry(gender).State=EntityState.Deleted;
            _context.SaveChanges();
            return gender;
        }

        public Gender Edit(Gender gender)
        {
             _context.Genders.Attach(gender);
            _context.Entry(gender).State = EntityState.Modified;
            _context.SaveChanges();
            return gender;
        }
        private List<Gender> DoSort(List<Gender> genders, string SortProperty, SortOrder sortOrder)
        {           

           
                if (sortOrder == SortOrder.Ascending)
                    genders = genders.OrderBy(d => d.Name).ToList();
                else
                    genders = genders.OrderByDescending(d => d.Name).ToList();
            

            return genders;
        }
        public PaginatedList<Gender> GetItems(string SortProperty, SortOrder sortOrder, string SearchText = "", int pageIndex = 1, int pageSize = 5)
        {
            List<Gender> genders;

            if (SearchText != "" && SearchText!=null)
            {
                genders = _context.Genders.Where(n => n.Name.Contains(SearchText))
                    .ToList();            
            }
            else
                genders= _context.Genders.ToList();

            genders = DoSort(genders,SortProperty,sortOrder);
            
            PaginatedList<Gender> retGender = new PaginatedList<Gender>(genders, pageIndex, pageSize);

            return retGender;
        }

             

        public bool IsGenderExists(string name)
        {
            bool result= _context.Genders.Any(c=>c.Name==name);
            return result;        
        }

        public bool IsGenderExists(string name, int Id)
        {
            bool result= _context.Genders.Any(c=>c.Name==name && c.ID==Id);
            return result; 
        }
        public Gender GetGender(int id)
        {
            Gender gender=_context.Genders.Where(g=>g.ID==id).FirstOrDefault();
            return gender;
        }

      
    }
}