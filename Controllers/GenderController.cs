using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ClinicManagement.Models;
using ClinicManagement.Interfaces;
using ClinicManagement.Tools;

namespace ClinicManagement.Controllers
{
    public class GenderController : Controller
    {
       [TempData]
        public string StatusMessage{get;set;}
        private readonly IGender _genderrepo;
        public GenderController(IGender gender)
        {
            _genderrepo = gender;
        }

        public IActionResult Index(string sortExpression="", string SearchText = "",int pg=1,int pageSize=5)
        {
            SortModel sortModel = new SortModel();
            sortModel.AddColumn("name");
            sortModel.AddColumn("description");

            sortModel.ApplySort(sortExpression);
            ViewData["sortModel"] = sortModel;
            
            ViewBag.SearchText = SearchText;

            PaginatedList<Gender> genders = _genderrepo.GetItems(sortModel.SortedProperty, sortModel.SortedOrder,SearchText,pg,pageSize);            
            

            var pager = new PagerModel(genders.TotalRecords, pg, pageSize);
            pager.SortExpression = sortExpression;
            this.ViewBag.Pager = pager;


            TempData["CurrentPage"] = pg;


            return View(genders);        }

        // GET: Gender/Details/5
        public IActionResult Details(int id) //read
        {
             Gender genders =_genderrepo.GetGender(id);              
            return View(genders);        
        }

        // GET: Gender/Create
       
        public IActionResult Create()
        {
            Gender genders = new Gender();
            return View(genders);
        }

        [HttpPost]  
        public IActionResult Create(Gender genders)
        {
            bool bolret = false;
            string errMessage = "";
            try
            {
               

                    genders = _genderrepo.Create(genders);
                    bolret = true;
                           
            }
            catch(Exception ex) 
            {
                errMessage = errMessage + " " + ex.Message;
            }
            if (bolret == false)
            {
                ModelState.AddModelError("", errMessage);
                return View(genders);
            }
            else
            {
                //StatusMessage= "Địa chỉ " + genders.Street + " tạo thành công";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Gender/Edit/5
      
        public IActionResult Edit(int id)
        {
            Gender genders = _genderrepo.GetGender(id);
            TempData.Keep();
            return View(genders);
        }  
        
        [HttpPost]
        public IActionResult Edit(Gender genders)
        {
            bool bolret = false;
            string errMessage = "";

            try
            {
                
              

                
                    genders = _genderrepo.Edit(genders);
                    //StatusMessage= genders.Street + ", đơn vị lưu thành công";
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
                return View(genders);
            }
            else
            return RedirectToAction(nameof(Index),new {pg=currentPage});
        }

        // GET: Gender/Delete/5
        public IActionResult Delete(int id)
        {
            Gender genders = _genderrepo.GetGender(id);
            TempData.Keep();
            return View(genders);
        }

        
        [HttpPost]
        public IActionResult Delete(Gender genders)
        {
            try
            {
                genders = _genderrepo.Delete(genders);
            }
            catch(Exception ex)
            {
                string errMessage = ex.Message;
                ModelState.AddModelError("", errMessage);
                return View(genders);
            }          
            
            int currentPage = 1;
            if (TempData["CurrentPage"] != null)
                currentPage = (int)TempData["CurrentPage"];

            ////StatusMessage = "đơn vị " + genders.Street + " xóa thành công";
            return RedirectToAction(nameof(Index), new { pg = currentPage });


        }
        // GET: Gender
    }
}
