using System.Collections.Generic;
using System.Linq;
using ClinicManagement.Interfaces;
using ClinicManagement.Models;
using ClinicManagement.Tools;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagement.Repositories
{
    public class AddressRepository:IAddress
    {
        private readonly ClinicManagementDbContext _context;
        public AddressRepository(ClinicManagementDbContext context)
        {
            _context=context;
        }
        public Address Create(Address address)
        {
                _context.addresses.Add(address);
                _context.SaveChanges();
                return address;
        }

        public Address Delete(Address address)
        {
            _context.Attach(address);
            _context.Entry(address).State=EntityState.Deleted;
            _context.SaveChanges();
            return address;
        }

        public Address Edit(Address address)
        {
             _context.addresses.Attach(address);
            _context.Entry(address).State = EntityState.Modified;
            _context.SaveChanges();
            return address;
        }
        private List<Address> DoSort(List<Address> address, string SortProperty, SortOrder sortOrder)
        {           

                if(SortProperty.ToLower()=="street")
                {
                     if (sortOrder == SortOrder.Ascending)
                    address = address.OrderBy(d => d.Street).ToList();
                    else
                        address = address.OrderByDescending(d => d.Street).ToList();
                }
                else
                {
                      if (sortOrder == SortOrder.Ascending)
                    address = address.OrderBy(d => d.City).ToList();
                else
                    address = address.OrderByDescending(d => d.City).ToList();
                }
           
              
            

            return address;
        }
        public PaginatedList<Address> GetItems(string SortProperty, SortOrder sortOrder, string SearchText = "", int pageIndex = 1, int pageSize = 5)
        {
            List<Address> address;

            if (SearchText != "" && SearchText!=null)
            {
                address = _context.addresses.Where(n => n.Street.Contains(SearchText)||n.City.Contains(SearchText))
                    .ToList();            
            }
            else
                address= _context.addresses.ToList();

            address = DoSort(address,SortProperty,sortOrder);
            
            PaginatedList<Address> retBoolGroup = new PaginatedList<Address>(address, pageIndex, pageSize);

            return retBoolGroup;
        }
        public Address GetAddress(int? id)
        {
            Address address = _context.addresses.Where(u => u.ID == id).FirstOrDefault();
            return address;        
         }

        
        
    }
}