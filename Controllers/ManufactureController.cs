using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ClinicManagement.Models;
using ClinicManagement.Repositories;
using ClinicManagement.Interfaces;
using ClinicManagement.Tools;

namespace ClinicManagement.Controllers
{
    public class ManufactureController : Controller
    {
        //private readonly ClinicManagementDbContext _context;
        private readonly IManufacture _manufactureRepo;

        public ManufactureController(IManufacture manufactureRepo)
        {
            //_context = context;
            _manufactureRepo=manufactureRepo;
        }

        // GET: Manufacture
        public IActionResult Index(string sortExpression="", string SearchText = "",int pg=1,int pageSize=5)
        {
            SortModel sortModel=new SortModel();
            sortModel.AddColumn("name");
            sortModel.AddColumn("description");
            sortModel.AddColumn("DateCreate");
            sortModel.ApplySort(sortExpression);
            ViewData["sortModel"] = sortModel;


            ViewBag.SearchText=SearchText;
            PaginatedList<Manufacture> manufactures=_manufactureRepo.GetItems(sortModel.SortedProperty,sortModel.SortedOrder,SearchText,pg,pageSize);

            var pager=new PagerModel(manufactures.TotalRecords,pg,pageSize);
            pager.SortExpression=sortExpression;
            this.ViewBag.Pager=pager;

            TempData["CurrentPage"]=pg;
            return View(manufactures);
            
        }

        // GET: Manufacture/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var manufacture= _manufactureRepo.GetManufacture(id);
         
            if (manufacture == null)
            {
                return NotFound();
            }

            return View(manufacture);
        }

        // GET: Manufacture/Create
        public IActionResult Create()
        {
            Manufacture manufacture = new Manufacture();
            return View(manufacture);
        }

        // POST: Manufacture/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("ID,Name,Description,Address,DateCreate")] Manufacture manufacture)
        {
            if (ModelState.IsValid)
            {
                // _context.Add(manufacture);
                // await _context.SaveChangesAsync();
                // return RedirectToAction(nameof(Index));
                manufacture = _manufactureRepo.Create(manufacture);
                return RedirectToAction(nameof(Index));

            }
            return View(manufacture);
        }

        // GET: Manufacture/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var manufacture = _manufactureRepo.GetManufacture(id);
            if (manufacture == null)
            {
                return NotFound();
            }            
            return View(manufacture);
        }

        // POST: Manufacture/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("ID,Name,Description,Address,DateCreate")] Manufacture manufacture)
        {
            if (id != manufacture.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                     manufacture = _manufactureRepo.Edit(manufacture);

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_manufactureRepo.IsManufactureNameExists(manufacture.Name,manufacture.ID))
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
            return View(manufacture);
        }

        // GET: Manufacture/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var manufacture = _manufactureRepo.GetManufacture(id);
            if (manufacture == null)
            {
                return NotFound();
            }

            return View(manufacture);
        }

        // POST: Manufacture/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var manufacture = _manufactureRepo.GetManufacture(id);
            if(manufacture==null)
                return NotFound();

            manufacture=_manufactureRepo.Delete(manufacture);
            return RedirectToAction(nameof(Index));
        }

        
    }
}
