using System.Collections.Generic;
using System.Linq;
using ClinicManagement.Interfaces;
using ClinicManagement.Models;
using ClinicManagement.Tools;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagement.Repositories
{
    public class BoolGroupRepository:IBloodGroup
    {
        private readonly ClinicManagementDbContext _context;
        public BoolGroupRepository(ClinicManagementDbContext context)
        {
            _context=context;
        }
        public BloodGroup Create(BloodGroup bloodGroup)
        {
                _context.BloodGroups.Add(bloodGroup);
                _context.SaveChanges();
                return bloodGroup;
        }

        public BloodGroup Delete(BloodGroup bloodGroup)
        {
            _context.Attach(bloodGroup);
            _context.Entry(bloodGroup).State=EntityState.Deleted;
            _context.SaveChanges();
            return bloodGroup;
        }

        public BloodGroup Edit(BloodGroup bloodGroup)
        {
             _context.BloodGroups.Attach(bloodGroup);
            _context.Entry(bloodGroup).State = EntityState.Modified;
            _context.SaveChanges();
            return bloodGroup;
        }
        private List<BloodGroup> DoSort(List<BloodGroup> boolGroups, string SortProperty, SortOrder sortOrder)
        {           

           
                if (sortOrder == SortOrder.Ascending)
                    boolGroups = boolGroups.OrderBy(d => d.Name).ToList();
                else
                    boolGroups = boolGroups.OrderByDescending(d => d.Name).ToList();
            

            return boolGroups;
        }
        public PaginatedList<BloodGroup> GetItems(string SortProperty, SortOrder sortOrder, string SearchText = "", int pageIndex = 1, int pageSize = 5)
        {
            List<BloodGroup> boolGroups;

            if (SearchText != "" && SearchText!=null)
            {
                boolGroups = _context.BloodGroups.Where(n => n.Name.Contains(SearchText))
                    .ToList();            
            }
            else
                boolGroups= _context.BloodGroups.ToList();

            boolGroups = DoSort(boolGroups,SortProperty,sortOrder);
            
            PaginatedList<BloodGroup> retBoolGroup = new PaginatedList<BloodGroup>(boolGroups, pageIndex, pageSize);

            return retBoolGroup;
        }           



        public BloodGroup GetBlooGroup(int id)
        {
            BloodGroup bloodGroup = _context.BloodGroups.Where(u => u.ID == id).FirstOrDefault();
            return bloodGroup;         }

        public bool IsBloodGroupNameExists(string name)
        {
            bool result= _context.BloodGroups.Any(c=>c.Name==name);
            return result;             
        }

        public bool IsBloodGroupNameExists(string name, int Id)
        {
             bool result= _context.BloodGroups.Any(c=>c.Name==name && c.ID==Id);
            return result; 
        }
    }
}