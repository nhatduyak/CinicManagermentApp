using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ClinicManagement.Models;
using ClinicManagement.Repositories;
using ClinicManagement.Tools;
using ClinicManagement.Interfaces;

namespace ClinicManagement.Controllers
{
    public class AddressController : Controller
    {
        [TempData]
        public string StatusMessage{get;set;}
        private readonly IAddress _addressRepository;
        public AddressController(IAddress addressRepository)
        {
            _addressRepository = addressRepository;
        }

        public IActionResult Index(string sortExpression="", string SearchText = "",int pg=1,int pageSize=5)
        {
            SortModel sortModel = new SortModel();
            sortModel.AddColumn("street");
            sortModel.AddColumn("city");

            sortModel.ApplySort(sortExpression);
            ViewData["sortModel"] = sortModel;
            
            ViewBag.SearchText = SearchText;

            PaginatedList<Address> address = _addressRepository.GetItems(sortModel.SortedProperty, sortModel.SortedOrder,SearchText,pg,pageSize);            
            

            var pager = new PagerModel(address.TotalRecords, pg, pageSize);
            pager.SortExpression = sortExpression;
            this.ViewBag.Pager = pager;


            TempData["CurrentPage"] = pg;


            return View(address);        }

        // GET: Address/Details/5
        public IActionResult Details(int id) //read
        {
             Address address =_addressRepository.GetAddress(id);              
            return View(address);        
        }

        // GET: Address/Create
       
        public IActionResult Create()
        {
            Address address = new Address();
            return View(address);
        }

        [HttpPost]  
        public IActionResult Create(Address address)
        {
            bool bolret = false;
            string errMessage = "";
            try
            {
               

                    address = _addressRepository.Create(address);
                    bolret = true;
                           
            }
            catch(Exception ex) 
            {
                errMessage = errMessage + " " + ex.Message;
            }
            if (bolret == false)
            {
                ModelState.AddModelError("", errMessage);
                return View(address);
            }
            else
            {
                //StatusMessage= "Địa chỉ " + address.Street + " tạo thành công";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Address/Edit/5
      
        public IActionResult Edit(int id)
        {
            Address address = _addressRepository.GetAddress(id);
            TempData.Keep();
            return View(address);
        }  
        
        [HttpPost]
        public IActionResult Edit(Address address)
        {
            bool bolret = false;
            string errMessage = "";

            try
            {
                
              

                
                    address = _addressRepository.Edit(address);
                    //StatusMessage= address.Street + ", đơn vị lưu thành công";
                    bolret = true;
               
            }
            catch(Exception ex)
            {
                errMessage = errMessage + " " + ex.Message;                
            }

          

            int currentPage = 1;
            if (TempData["CurrentPage"] != null)
                currentPage = (int)TempData["CurrentPage"];

          
            if(bolret==false)
            {
                //StatusMessage = errMessage;
                ModelState.AddModelError("", errMessage);
                return View(address);
            }
            else
            return RedirectToAction(nameof(Index),new {pg=currentPage});
        }

        // GET: Address/Delete/5
        public IActionResult Delete(int id)
        {
            Address address = _addressRepository.GetAddress(id);
            TempData.Keep();
            return View(address);
        }

        
        [HttpPost]
        public IActionResult Delete(Address address)
        {
            try
            {
                address = _addressRepository.Delete(address);
            }
            catch(Exception ex)
            {
                string errMessage = ex.Message;
                ModelState.AddModelError("", errMessage);
                return View(address);
            }          
            
            int currentPage = 1;
            if (TempData["CurrentPage"] != null)
                currentPage = (int)TempData["CurrentPage"];

            ////StatusMessage = "đơn vị " + address.Street + " xóa thành công";
            return RedirectToAction(nameof(Index), new { pg = currentPage });


        }
        // GET: Address
     
      
    }
}
