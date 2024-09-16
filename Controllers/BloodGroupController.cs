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
    public class BloodGroupController : Controller
    {
        [TempData]
        public string StatusMessage { get; set;}
        private readonly IBloodGroup _bloodgrouprepo;

        public BloodGroupController(IBloodGroup ibloodgrouprepo)
        {
            _bloodgrouprepo = ibloodgrouprepo;       
        }

        // GET: BloodGroup
        public IActionResult Index(string sortExpression="", string SearchText = "",int pg=1,int pageSize=5)
        {
            SortModel sortModel = new SortModel();
            sortModel.AddColumn("name");
            sortModel.AddColumn("description");
            sortModel.ApplySort(sortExpression);
            ViewData["sortModel"] = sortModel;
            
            ViewBag.SearchText = SearchText;

            PaginatedList<BloodGroup> bloods = _bloodgrouprepo.GetItems(sortModel.SortedProperty, sortModel.SortedOrder,SearchText,pg,pageSize);            
            

            var pager = new PagerModel(bloods.TotalRecords, pg, pageSize);
            pager.SortExpression = sortExpression;
            this.ViewBag.Pager = pager;


            TempData["CurrentPage"] = pg;


            return View(bloods);        
            
            }

        // GET: BloodGroup/Details/5
        public IActionResult Details(int id) //read
        {
             BloodGroup bloodGroup =_bloodgrouprepo.GetBlooGroup(id);              
            return View(bloodGroup);        
        }

        // GET: BloodGroup/Create
       
        public IActionResult Create()
        {
            BloodGroup bloodGroup = new BloodGroup();
            return View(bloodGroup);
        }

        [HttpPost]  
        public IActionResult Create(BloodGroup bloodGroup)
        {
            bool bolret = false;
            string errMessage = "";
            try
            {
               

                if (_bloodgrouprepo.IsBloodGroupNameExists(bloodGroup.Name) == true)
                    errMessage = errMessage + " " + " tên nhóm máu" + bloodGroup.Name +" đã tồn tại";

                if (errMessage == "")
                {
                    bloodGroup = _bloodgrouprepo.Create(bloodGroup);
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
                return View(bloodGroup);
            }
            else
            {
                //StatusMessage= "Đơn vị " + bloodGroup.Name + " tạo thành công";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: BloodGroup/Edit/5
      
        public IActionResult Edit(int id)
        {
            BloodGroup bloodGroup = _bloodgrouprepo.GetBlooGroup(id);
            TempData.Keep();
            return View(bloodGroup);
        }  
        
        [HttpPost]
        public IActionResult Edit(BloodGroup bloodGroup)
        {
            bool bolret = false;
            string errMessage = "";

            try
            {
                
                if (_bloodgrouprepo.IsBloodGroupNameExists(bloodGroup.Name, bloodGroup.ID) == true)
                    errMessage = errMessage + "Tên đơn vị " + bloodGroup.Name + " đã tồn tại";

                if (errMessage == "")
                {
                    bloodGroup = _bloodgrouprepo.Edit(bloodGroup);
                    //StatusMessage= bloodGroup.Name + ", đơn vị lưu thành công";
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
                return View(bloodGroup);
            }
            else
            return RedirectToAction(nameof(Index),new {pg=currentPage});
        }

        // GET: BloodGroup/Delete/5
        public IActionResult Delete(int id)
        {
            BloodGroup bloodGroup = _bloodgrouprepo.GetBlooGroup(id);
            TempData.Keep();
            return View(bloodGroup);
        }

        
        [HttpPost]
        public IActionResult Delete(BloodGroup bloodGroup)
        {
            try
            {
                bloodGroup = _bloodgrouprepo.Delete(bloodGroup);
            }
            catch(Exception ex)
            {
                string errMessage = ex.Message;
                //StatusMessage = errMessage;
                ModelState.AddModelError("", errMessage);
                return View(bloodGroup);
            }          
            
            int currentPage = 1;
            if (TempData["CurrentPage"] != null)
                currentPage = (int)TempData["CurrentPage"];

            //StatusMessage = "đơn vị " + bloodGroup.Name + " xóa thành công";
            return RedirectToAction(nameof(Index), new { pg = currentPage });


        }
    }
}
