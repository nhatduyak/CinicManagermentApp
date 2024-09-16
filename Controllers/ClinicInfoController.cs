using System;
using ClinicManagement.Interfaces;
using ClinicManagement.Models;
using ClinicManagement.Tools;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagement.Controllers
{
    public class ClinicInfoController:Controller
    {
         [TempData]
        public string StatusMessage { get; set;}
        private readonly IClinicInfo _clinicInforepo;

        public ClinicInfoController(IClinicInfo clinicInfo)
        {
            _clinicInforepo = clinicInfo;       
        }

        // GET: ClinicInfo
        public IActionResult Index(string sortExpression="", string SearchText = "",int pg=1,int pageSize=5)
        {
            SortModel sortModel = new SortModel();
            sortModel.AddColumn("name");
            sortModel.AddColumn("Address");
            sortModel.ApplySort(sortExpression);
            ViewData["sortModel"] = sortModel;
            
            ViewBag.SearchText = SearchText;

            PaginatedList<ClinicInfo> bloods = _clinicInforepo.GetItems(sortModel.SortedProperty, sortModel.SortedOrder,SearchText,pg,pageSize);            
            

            var pager = new PagerModel(bloods.TotalRecords, pg, pageSize);
            pager.SortExpression = sortExpression;
            this.ViewBag.Pager = pager;


            TempData["CurrentPage"] = pg;


            return View(bloods);        
            
            }

        // GET: ClinicInfo/Details/5
        public IActionResult Details(int id) //read
        {
             ClinicInfo clinicInfo =_clinicInforepo.GetClinicInfo(id);              
            return View(clinicInfo);        
        }

        // GET: ClinicInfo/Create
       
        public IActionResult Create()
        {
            ClinicInfo clinicInfo = new ClinicInfo();
            return View(clinicInfo);
        }

        [HttpPost]  
        public IActionResult Create(ClinicInfo clinicInfo)
        {
            bool bolret = false;
            string errMessage = "";
            try
            {
               

              

                if (errMessage == "")
                {
                    clinicInfo = _clinicInforepo.Create(clinicInfo);
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
                return View(clinicInfo);
            }
            else
            {
                //StatusMessage= "Đơn vị " + clinicInfo.Name + " tạo thành công";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: ClinicInfo/Edit/5
      
        public IActionResult Edit(int id)
        {
            ClinicInfo clinicInfo = _clinicInforepo.GetClinicInfo(id);
            TempData.Keep();
            return View(clinicInfo);
        }  
        
        [HttpPost]
        public IActionResult Edit(ClinicInfo clinicInfo)
        {
            bool bolret = false;
            string errMessage = "";

            try
            {
                
               
                if (errMessage == "")
                {
                    clinicInfo = _clinicInforepo.Edit(clinicInfo);
                    //StatusMessage= clinicInfo.Name + ", đơn vị lưu thành công";
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
                return View(clinicInfo);
            }
            else
            return RedirectToAction(nameof(Index),new {pg=currentPage});
        }

        // GET: ClinicInfo/Delete/5
        public IActionResult Delete(int id)
        {
            ClinicInfo clinicInfo = _clinicInforepo.GetClinicInfo(id);
            TempData.Keep();
            return View(clinicInfo);
        }

        
        [HttpPost]
        public IActionResult Delete(ClinicInfo clinicInfo)
        {
            try
            {
                clinicInfo = _clinicInforepo.Delete(clinicInfo);
            }
            catch(Exception ex)
            {
                string errMessage = ex.Message;
                //StatusMessage = errMessage;
                ModelState.AddModelError("", errMessage);
                return View(clinicInfo);
            }          
            
            int currentPage = 1;
            if (TempData["CurrentPage"] != null)
                currentPage = (int)TempData["CurrentPage"];

            //StatusMessage = "đơn vị " + clinicInfo.Name + " xóa thành công";
            return RedirectToAction(nameof(Index), new { pg = currentPage });


        }
        
    }
}