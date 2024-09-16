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
using Microsoft.AspNetCore.Identity;

namespace ClinicManagement.Controllers
{
    public class MedicinesController : Controller
    {
        //private readonly ClinicManagementDbContext _context;
        private readonly IMedicines _medicinesrepo;
        private UserManager<AppUser> _userManager;
         [TempData]
        public string StatusMessage { get; set;}
        public MedicinesController(IMedicines medicinesrepo,UserManager<AppUser> userManager)
        {
            //_context = context;
            _medicinesrepo=medicinesrepo;
            _userManager=userManager;
        }

        // GET: Medicines
        public IActionResult Index(string sortExpression="", string SearchText = "",int pg=1,int pageSize=10)
        {
             SortModel sortModel=new SortModel();
            sortModel.AddColumn("code");
            sortModel.AddColumn("name");
            sortModel.AddColumn("Category");
            sortModel.AddColumn("unitprice");
            sortModel.AddColumn("SellPrice");
            sortModel.AddColumn("Quantity");
            sortModel.AddColumn("ExpiryDate");
            sortModel.AddColumn("DateCreate");

            sortModel.ApplySort(sortExpression);
            ViewData["sortModel"] = sortModel;
               ViewBag.SearchText=SearchText;
            PaginatedList<Medicines> medicines=_medicinesrepo.GetItems(sortModel.SortedProperty,sortModel.SortedOrder,SearchText,pg,pageSize);

            var pager=new PagerModel(medicines.TotalRecords,pg,pageSize);
            pager.SortExpression=sortExpression;
            this.ViewBag.Pager=pager;

            TempData["CurrentPage"]=pg;
            return View(medicines);

        }

        //GET: Medicines/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // var medicines = await _context.Medicines
            //     .Include(m => m.Units)
            //     .FirstOrDefaultAsync(m => m.ID == id);
            var medicines=await _medicinesrepo.GetMedicines(id);
            if (medicines == null)
            {
                return NotFound();
            }

            return View(medicines);
        }

        // GET: Medicines/Create
        public IActionResult Create()
        {
            
            ViewData["UnitID"] = new SelectList(_medicinesrepo.GetUnitsSelected(), "ID", "Name");
            ViewData["ManufactureID"]=new SelectList(_medicinesrepo.GetManufactureSelected(),"ID","Name");
            ViewData["CategoryId"]=new SelectList(_medicinesrepo.GetCategorySelected(),"ID","Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Medicines medicines)
        {
            string errMessage = "";
            try
            {

                // if (_category.IsCategoryNameExits(category.Name) == true)
                //     errMessage = errMessage + " " + " Tên danh mục " + category.Name +" đã tồn tại";

                if (ModelState.IsValid)
                {
                    if (medicines.UnitID == -1) medicines.UnitID = null;
                    if (medicines.CategoryId == -1) medicines.CategoryId = null;
                    if (medicines.ManufactureId == -1) medicines.ManufactureId = null;
                    var user=await _userManager.GetUserAsync(User);
                    if(user!!=null)
                    {
                        medicines.UserID=user.Id;
                    }
                    medicines.DateCreate=DateTime.Now;
                    medicines = _medicinesrepo.Create(medicines);
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                errMessage = errMessage + " " + ex.Message;
            }

            //TempData["ErrorMessage"] = errMessage;\
            //StatusMessage=errMessage;
            ViewData["UnitID"] = new SelectList(_medicinesrepo.GetUnitsSelected(), "ID", "Name");
            ViewData["ManufactureID"]=new SelectList(_medicinesrepo.GetManufactureSelected(),"ID","Name");
            ViewData["CategoryId"]=new SelectList(_medicinesrepo.GetCategorySelected(),"ID","Name");
            return View(medicines);
        }

        // POST: Medicines/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // [HttpPost]
        // [ValidateAntiForgeryToken]
        // public async Task<IActionResult> Create([Bind("ID,Code,Name,Description,UnitID,UnitPrice,SellPrice,Quantity,ExpiryDate,OldUnitPrice,OldSellPrice,DateCreate,DateModify,UserID,UserIDModify")] Medicines medicines)
        // {
        //     if (ModelState.IsValid)
        //     {
        //         _context.Add(medicines);
        //         await _context.SaveChangesAsync();
        //         return RedirectToAction(nameof(Index));
        //     }
        //     ViewData["UnitID"] = new SelectList(_context.Units, "ID", "Name", medicines.UnitID);
        //     return View(medicines);
        // }

        // GET: Medicines/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicines = await _medicinesrepo.GetMedicines(id);
            if (medicines == null)
            {
                return NotFound();
            }
            //ViewData["UnitID"] = new SelectList(_context.Units, "ID", "Name", medicines.UnitID);
            ViewData["UnitID"] = new SelectList(_medicinesrepo.GetUnitsSelected(), "ID", "Name",medicines.UnitID);
            ViewData["ManufactureID"]=new SelectList(_medicinesrepo.GetManufactureSelected(),"ID","Name",medicines.ManufactureId);
            ViewData["CategoryId"]=new SelectList(_medicinesrepo.GetCategorySelected(),"ID","Name",medicines.CategoryId);
            return View(medicines);
        }

        // POST: Medicines/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Code,Name,Description,UnitID,ManufactureId,CategoryId,UnitPrice,SellPrice,Quantity,ExpiryDate,OldUnitPrice,OldSellPrice,DateCreate,DateModify,UserID,UserIDModify")] Medicines medicines)
        {
            if (id != medicines.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var user=await _userManager.GetUserAsync(User);
                    if(user!!=null)
                    {
                        medicines.UserIDModify=user.Id;
                    }
                    medicines.DateModify=DateTime.Now;
                    if (medicines.UnitID == -1) medicines.UnitID = null;
                    if (medicines.CategoryId == -1) medicines.CategoryId = null;
                    if (medicines.ManufactureId == -1) medicines.ManufactureId = null;
                    _medicinesrepo.Edit(medicines);
                    
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_medicinesrepo.IsMedicinesIdExits(medicines.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["UnitID"] = new SelectList(_medicinesrepo.GetUnitsSelected(), "ID", "Name",medicines.UnitID);
            ViewData["ManufactureID"]=new SelectList(_medicinesrepo.GetManufactureSelected(),"ID","Name",medicines.ManufactureId);
            ViewData["CategoryId"]=new SelectList(_medicinesrepo.GetCategorySelected(),"ID","Name",medicines.CategoryId);            
            return View(medicines);
        }

        // GET: Medicines/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicines =await _medicinesrepo.GetMedicines(id);
            if (medicines == null)
            {
                return NotFound();
            }

            return View(medicines);
        }

        // POST: Medicines/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var medicines = await _medicinesrepo.GetMedicines(id);
            if(medicines==null)
            {
                return NotFound();
            }
            _medicinesrepo.Delete(medicines);
            return RedirectToAction(nameof(Index));
        }

        // private bool MedicinesExists(int id)
        // {
        //     return _context.Medicines.Any(e => e.ID == id);
        // }
    }
}
