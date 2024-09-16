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
    public class UnitController : Controller
    {
        //private readonly ClinicManagementDbContext _context;

        [TempData]
        public string StatusMessage { get; set;}
        private readonly IUnit _unitRepo;

        public UnitController(IUnit unitrepo)
        {
            _unitRepo = unitrepo;       
        }

        // GET: Unit
        public IActionResult Index(string sortExpression="", string SearchText = "",int pg=1,int pageSize=5)
        {
            SortModel sortModel = new SortModel();
            sortModel.AddColumn("name");
            sortModel.AddColumn("description");
            sortModel.ApplySort(sortExpression);
            ViewData["sortModel"] = sortModel;
            
            ViewBag.SearchText = SearchText;

            PaginatedList<Unit> units = _unitRepo.GetItems(sortModel.SortedProperty, sortModel.SortedOrder,SearchText,pg,pageSize);            
            

            var pager = new PagerModel(units.TotalRecords, pg, pageSize);
            pager.SortExpression = sortExpression;
            this.ViewBag.Pager = pager;


            TempData["CurrentPage"] = pg;


            return View(units);        }

        // GET: Unit/Details/5
        public IActionResult Details(int id) //read
        {
             Unit unit =_unitRepo.GetUnit(id);              
            return View(unit);        
        }

        // GET: Unit/Create
       
        public IActionResult Create()
        {
            Unit unit = new Unit();
            return View(unit);
        }

        [HttpPost]  
        public IActionResult Create(Unit unit)
        {
            bool bolret = false;
            string errMessage = "";
            try
            {
               

                if (_unitRepo.IsUnitNameExists(unit.Name) == true)
                    errMessage = errMessage + " " + " tên đơn vị" + unit.Name +" đã tồn tại";

                if (errMessage == "")
                {
                    unit = _unitRepo.Create(unit);
                    bolret = true;
                }                
            }
            catch(Exception ex) 
            {
                errMessage = errMessage + " " + ex.Message;
            }
            if (bolret == false)
            {
                //StatusMessage = errMessage;
                ModelState.AddModelError("", errMessage);
                return View(unit);
            }
            else
            {
                //StatusMessage= "Đơn vị " + unit.Name + " tạo thành công";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Unit/Edit/5
      
        public IActionResult Edit(int id)
        {
            Unit unit = _unitRepo.GetUnit(id);
            TempData.Keep();
            return View(unit);
        }  
        
        [HttpPost]
        public IActionResult Edit(Unit unit)
        {
            bool bolret = false;
            string errMessage = "";

            try
            {
                
                if (_unitRepo.IsUnitNameExists(unit.Name, unit.ID) == true)
                    errMessage = errMessage + "Tên đơn vị " + unit.Name + " đã tồn tại";

                if (errMessage == "")
                {
                    unit = _unitRepo.Edit(unit);
                    //StatusMessage= unit.Name + ", đơn vị lưu thành công";
                    bolret = true;
                }
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
                return View(unit);
            }
            else
            return RedirectToAction(nameof(Index),new {pg=currentPage});
        }

        // GET: Unit/Delete/5
        public IActionResult Delete(int id)
        {
            Unit unit = _unitRepo.GetUnit(id);
            TempData.Keep();
            return View(unit);
        }

        
        [HttpPost]
        public IActionResult Delete(Unit unit)
        {
            try
            {
                unit = _unitRepo.Delete(unit);
            }
            catch(Exception ex)
            {
                string errMessage = ex.Message;
                //StatusMessage = errMessage;
                ModelState.AddModelError("", errMessage);
                return View(unit);
            }          
            
            int currentPage = 1;
            if (TempData["CurrentPage"] != null)
                currentPage = (int)TempData["CurrentPage"];

            //StatusMessage = "đơn vị " + unit.Name + " xóa thành công";
            return RedirectToAction(nameof(Index), new { pg = currentPage });


        }
    }
}
