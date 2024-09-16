using System.Collections.Generic;
using System.Linq;
using ClinicManagement.Interfaces;
using ClinicManagement.Models;
using ClinicManagement.Tools;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagement.Repositories
{
    public class ClinicInfoRemository:IClinicInfo
    {
        private readonly ClinicManagementDbContext _context;
        public ClinicInfoRemository(ClinicManagementDbContext context)
        {
            _context=context;
        }
        public ClinicInfo Create(ClinicInfo clinicInfo)
        {
                _context.ClinicInfos.Add(clinicInfo);
                _context.SaveChanges();
                return clinicInfo;
        }

        public ClinicInfo Delete(ClinicInfo clinicInfo)
        {
            _context.Attach(clinicInfo);
            _context.Entry(clinicInfo).State=EntityState.Deleted;
            _context.SaveChanges();
            return clinicInfo;
        }

        public ClinicInfo Edit(ClinicInfo clinicInfo)
        {
             _context.ClinicInfos.Attach(clinicInfo);
            _context.Entry(clinicInfo).State = EntityState.Modified;
            _context.SaveChanges();
            return clinicInfo;
        }
        private List<ClinicInfo> DoSort(List<ClinicInfo> clinicinfo, string SortProperty, SortOrder sortOrder)
        {           

           
                if (sortOrder == SortOrder.Ascending)
                    clinicinfo = clinicinfo.OrderBy(d => d.Name).ToList();
                else
                    clinicinfo = clinicinfo.OrderByDescending(d => d.Name).ToList();
           

            return clinicinfo;
        }
        public PaginatedList<ClinicInfo> GetItems(string SortProperty, SortOrder sortOrder, string SearchText = "", int pageIndex = 1, int pageSize = 5)
        {
            List<ClinicInfo> clinicinfo;

            if (SearchText != "" && SearchText!=null)
            {
                clinicinfo = _context.ClinicInfos.Where(n => n.Name.Contains(SearchText) )
                    .ToList();            
            }
            else
                clinicinfo= _context.ClinicInfos.ToList();

            clinicinfo = DoSort(clinicinfo,SortProperty,sortOrder);
            
            PaginatedList<ClinicInfo> retManufacture = new PaginatedList<ClinicInfo>(clinicinfo, pageIndex, pageSize);

            return retManufacture;
        }
     
        public ClinicInfo GetClinicInfo(int id)
        {
            ClinicInfo clinicInfo = _context.ClinicInfos.Where(u => u.ID == id).FirstOrDefault();
            return clinicInfo;            
        }
    }
}