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
    [Route("{Controller}/{action}/{id?}")]
    public class DoctorController : Controller
    {
         public class doctordata{
        public Doctor datadoctor{get;set;}
        public string Street{get;set;}
        public string city{get;set;}
        public string province{get;set;}
        public string Post_Code{get;set;}
       }
        private readonly IDoctor _doctorRepo;
        private readonly IAddress _address;

        public DoctorController(IDoctor doctor,IAddress address)
        {
            //_context = context;
            _doctorRepo=doctor;
            _address=address;
        }
       

        // GET: Doctor
        public IActionResult Index(string sortExpression="", string SearchText = "",int pg=1,int pageSize=5)
        {
            SortModel sortModel=new SortModel();
            sortModel.AddColumn("fname");
            sortModel.AddColumn("lname");
            sortModel.AddColumn("age");
            sortModel.ApplySort(sortExpression);
            ViewData["sortModel"] = sortModel;


            ViewBag.SearchText=SearchText;
            PaginatedList<Doctor> doctors=_doctorRepo.GetItems(sortModel.SortedProperty,sortModel.SortedOrder,SearchText,pg,pageSize);

            var pager=new PagerModel(doctors.TotalRecords,pg,pageSize);
            pager.SortExpression=sortExpression;
            this.ViewBag.Pager=pager;

            TempData["CurrentPage"]=pg;
            return View(doctors);
            
        }

        // GET: Doctor/Details/5
        public IActionResult Details(int id)
        {
            
            var doctor= _doctorRepo.GetDoctor(id);
         
            if (doctor == null)
            {
                return NotFound();
            }

            return View(doctor);
        }

        // GET: Doctor/Create
        public IActionResult Create()
        {
            ViewData["GenderID"] = new SelectList(_doctorRepo.GetGenderList(), "ID", "Name");
            ViewData["BloodGroupID"] = new SelectList(_doctorRepo.GetBloodGroupList(), "ID", "Name");

            var doctorAddress=new doctordata{datadoctor=new Doctor()};

            return View(doctorAddress);
        }

        // POST: Doctor/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create( [Bind("datadoctor,Street,city,province,Post_Code")]doctordata doctor)
        {
            if (ModelState.IsValid)
            {
                // _context.Add(doctor);
                // await _context.SaveChangesAsync();
                // return RedirectToAction(nameof(Index));
                if (doctor.datadoctor.GenderID == -1) doctor.datadoctor.GenderID = null;
                    if (doctor.datadoctor.BloodGroupID == -1) doctor.datadoctor.BloodGroupID = null;
                    //if (patient.address?.Street == string.Empty&&patient.address?.City==string.Empty) patient.AddressID = null;
                    //Console.WriteLine($"ten duong {patientAddress.Street}" +"city "+ $"{patientAddress.city}");
                     if(!string.IsNullOrEmpty(doctor.Street)||!string.IsNullOrEmpty(doctor.city))
                    {
                        Address addr=new Address(){
                            Street=doctor.Street,
                            City=doctor.city,
                            Province=doctor.province,
                            Post_Code=doctor.Post_Code
                        };
                        addr= _address.Create(addr);
                        doctor.datadoctor.AddressID=addr.ID;
                    }
                    else
                    {
                        doctor.datadoctor.address=null;
                        doctor.datadoctor.AddressID=null;
                    }
                // _context.Add(patient);
                // await _context.SaveChangesAsync();
                // return RedirectToAction(nameof(Index));
                doctor.datadoctor.Registed_Date=DateTime.Now;
                doctor.datadoctor = _doctorRepo.Create(doctor.datadoctor);
                return RedirectToAction(nameof(Index));

            }
             ViewData["GenderID"] = new SelectList(_doctorRepo.GetGenderList(), "ID", "Name");
            ViewData["BloodGroupID"] = new SelectList(_doctorRepo.GetBloodGroupList(), "ID", "Name");
            return View(doctor);
        }

        // GET: Doctor/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctor = _doctorRepo.GetDoctor(id);
            if (doctor == null)
            {
                return NotFound();
            }           
            //doctor.address=_doctorRepo.GetAddress(doctor.AddressID);
            ViewData["GenderID"] = new SelectList(_doctorRepo.GetGenderList(), "ID", "Name");
            ViewData["BloodGroupID"] = new SelectList(_doctorRepo.GetBloodGroupList(), "ID", "Name");
            var data=new doctordata(){datadoctor=doctor,
                                                    Street=doctor.address?.Street,
                                                    city=doctor.address?.City,
                                                    province=doctor.address?.Province,
                                                    Post_Code=doctor.address?.Post_Code};
            return View(data);
        }

        // POST: Doctor/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int? id, [Bind("datadoctor,Street,city,province,Post_Code")]doctordata doctor)
        {
            if (id ==null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //patientAddress.Patient=_patientRepo.GetPatient(id);
                    if(doctor.datadoctor==null)
                    {
                        return NotFound();
                    }
                     doctor.datadoctor.Registed_Date=DateTime.Now;
                    if (doctor.datadoctor.GenderID == -1) doctor.datadoctor.GenderID = null;
                    if (doctor.datadoctor.BloodGroupID == -1) doctor.datadoctor.BloodGroupID = null;

                    if(doctor.datadoctor.AddressID==null && (!string.IsNullOrEmpty(doctor.Street)||!string.IsNullOrEmpty(doctor.city)))
                    {
                        Address addr = new Address(){Street=doctor.Street,
                                                        City=doctor.city,
                                                        Province=doctor.province,
                                                        Post_Code=doctor.Post_Code
                                                        };
                        addr= _address.Create(addr);
                        doctor.datadoctor.AddressID=addr.ID;
                    }else if(doctor.datadoctor.AddressID!=null)
                    {
                        Console.WriteLine($"Edit address {doctor.Street}");
                        Address addr= _address.GetAddress(doctor.datadoctor.AddressID);
                        addr.Street=doctor.Street;
                        addr.City=doctor.city;
                        addr.Province=doctor.province;
                        addr.Post_Code=doctor.Post_Code;
                        _address.Edit(addr);
                    }              


                 
                     _doctorRepo.Edit(doctor.datadoctor);

                }
                catch (DbUpdateConcurrencyException)
                {
                    
                }
                return RedirectToAction(nameof(Index));
            }
             ViewData["GenderID"] = new SelectList(_doctorRepo.GetGenderList(), "ID", "Name");
            ViewData["BloodGroupID"] = new SelectList(_doctorRepo.GetBloodGroupList(), "ID", "Name");
            return View(doctor);
        }

        // GET: Doctor/Delete/5
        public IActionResult Delete(int id)
        {       

            var doctor = _doctorRepo.GetDoctor(id);
            if (doctor == null)
            {
                return NotFound();
            }

            return View(doctor);
        }

        // POST: Doctor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var doctor = _doctorRepo.GetDoctor(id);
            if(doctor==null)
                return NotFound();
            Address address=doctor.address;
            doctor=_doctorRepo.Delete(doctor);
            if(address!=null)
                _address.Delete(address);
            return RedirectToAction(nameof(Index));
        }
    }
}
