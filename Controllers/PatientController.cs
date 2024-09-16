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
    //[Route("Benhnhan/{action}/{id?}")]
    public class PatientController : Controller
    {
      //private readonly ClinicManagementDbContext _context;
        private readonly IPatient _patientRepo;
        private readonly IAddress _address;

       public class PatientAddress{
        public Patient Patient{get;set;}
        public string Street{get;set;}
        public string city{get;set;}
        public string province{get;set;}
        public string Post_Code{get;set;}
       }


        

        public PatientController(IPatient patientrepo,IAddress address)
        {
            //_context = context;
            _patientRepo=patientrepo;
            _address=address;
        }

        // GET: Patient
        public IActionResult Index(string sortExpression="", string SearchText = "",int pg=1,int pageSize=10)
        {
            SortModel sortModel=new SortModel();
            sortModel.AddColumn("Registed_Date");
            sortModel.AddColumn("fname");
            sortModel.AddColumn("lname");
            sortModel.AddColumn("age");
            sortModel.ApplySort(sortExpression);
            ViewData["sortModel"] = sortModel;


            ViewBag.SearchText=SearchText;
            PaginatedList<Patient> patients=_patientRepo.GetItems(sortModel.SortedProperty,sortModel.SortedOrder,SearchText,pg,pageSize);
            var pager=new PagerModel(patients.TotalRecords,pg,pageSize);
            pager.SortExpression=sortExpression;
            this.ViewBag.Pager=pager;

            TempData["CurrentPage"]=pg;
            return View(patients);
            
        }

        // GET: Patient/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var patient= _patientRepo.GetPatient(id);
            List<prescriptions> listprescr=_patientRepo.GetPrescriptions(id);
            ViewData["prescriptionsList"]=listprescr;
            Console.WriteLine($"Don thuoc la {listprescr.Count}");
            ViewData["MedicinesID"]=new SelectList(_patientRepo.GetMedicines(), "ID", "Name");
            MedicinesWhenToTake whenToTake=new MedicinesWhenToTake();
            ViewData["When_To_Take"] = new SelectList(whenToTake.GetListMedicinesWhenToTake(), "ID", "Name");
         
            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);
        }

        // GET: Patient/Create
        public IActionResult Create()
        {
            ViewData["GenderID"] = new SelectList(_patientRepo.GetGenderList(), "ID", "Name");
            ViewData["BloodGroupID"] = new SelectList(_patientRepo.GetBloodGroupList(), "ID", "Name");


            Patient patient = new Patient();
            //patient.address=new Address();
            PatientAddress patientdata=new PatientAddress{Patient=patient};
            return View(patientdata);
        }

        // POST: Patient/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public IActionResult Create([Bind("ID,FName,LName,Age,Phone,Email,Address,GenderID,BloodGroupID,AddressID")] Patient patient)
        public IActionResult Create(PatientAddress patientAddress)
        {
            if (ModelState.IsValid)
            {
                if (patientAddress.Patient.GenderID == -1) patientAddress.Patient.GenderID = null;
                    if (patientAddress.Patient.BloodGroupID == -1) patientAddress.Patient.BloodGroupID = null;
                    //if (patient.address?.Street == string.Empty&&patient.address?.City==string.Empty) patient.AddressID = null;
                    Console.WriteLine($"ten duong {patientAddress.Street}" +"city "+ $"{patientAddress.city}");
                     if(!string.IsNullOrEmpty(patientAddress.Street)||!string.IsNullOrEmpty(patientAddress.city))
                    {
                        Address addr=new Address(){
                            Street=patientAddress.Street,
                            City=patientAddress.city,
                            Province=patientAddress.province,
                            Post_Code=patientAddress.Post_Code
                        };
                        addr= _address.Create(addr);
                        patientAddress.Patient.AddressID=addr.ID;
                    }
                    else
                    {
                        patientAddress.Patient.address=null;
                        patientAddress.Patient.AddressID=null;
                    }
                // _context.Add(patient);
                // await _context.SaveChangesAsync();
                // return RedirectToAction(nameof(Index));
                patientAddress.Patient.Registed_Date=DateTime.Now;
                patientAddress.Patient = _patientRepo.Create(patientAddress.Patient);
                return RedirectToAction(nameof(Index));

            }
              ViewData["GenderID"] = new SelectList(_patientRepo.GetGenderList(), "ID", "Name");
            ViewData["BloodGroupID"] = new SelectList(_patientRepo.GetBloodGroupList(), "ID", "Name");
            return View(patientAddress.Patient);
        }

        // GET: Patient/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewData["GenderID"] = new SelectList(_patientRepo.GetGenderList(), "ID", "Name");
            ViewData["BloodGroupID"] = new SelectList(_patientRepo.GetBloodGroupList(), "ID", "Name");

            var patient = _patientRepo.GetPatient(id);
            patient.address=_patientRepo.GetAddress(patient.AddressID);
            if (patient == null)
            {
                return NotFound();
            }            
            var patientdata=new PatientAddress(){Patient=patient,
                                                    Street=patient.address?.Street,
                                                    city=patient.address?.City,
                                                    province=patient.address?.Province,
                                                    Post_Code=patient.address?.Post_Code};
            return View(patientdata);
        }

        // POST: Patient/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Patient,Street,city,province,Post_Code")]PatientAddress patientAddress)
        {
           
            if (ModelState.IsValid)
            {
                try
                {
                    //patientAddress.Patient=_patientRepo.GetPatient(id);
                    if(patientAddress.Patient==null)
                    {
                        return NotFound();
                    }

                     patientAddress.Patient.Registed_Date=DateTime.Now;
                    if (patientAddress.Patient.GenderID == -1) patientAddress.Patient.GenderID = null;
                    if (patientAddress.Patient.BloodGroupID == -1) patientAddress.Patient.BloodGroupID = null;
                    Console.WriteLine($"gia tri  id {patientAddress.Patient.AddressID} street {patientAddress.Street} string {!string.IsNullOrEmpty(patientAddress.Street)} city {patientAddress.city} {!string.IsNullOrEmpty(patientAddress.city)}");
                    if(patientAddress.Patient.AddressID==null && (!string.IsNullOrEmpty(patientAddress.Street)||!string.IsNullOrEmpty(patientAddress.city)))
                    {
                        Address addr = new Address(){Street=patientAddress.Street,
                                                        City=patientAddress.city,
                                                        Province=patientAddress.province,
                                                        Post_Code=patientAddress.Post_Code
                                                        };
                        addr= _address.Create(addr);
                        patientAddress.Patient.AddressID=addr.ID;
                    }else if(patientAddress.Patient.AddressID!=null)
                    {
                        Console.WriteLine($"Edit address {patientAddress.Street}");
                        Address addr= _address.GetAddress(patientAddress.Patient.AddressID);
                        addr.Street=patientAddress.Street;
                        addr.City=patientAddress.city;
                        addr.Province=patientAddress.province;
                        addr.Post_Code=patientAddress.Post_Code;
                        _address.Edit(addr);
                    }              


                 
                     _patientRepo.Edit(patientAddress.Patient);

                }
                catch (DbUpdateConcurrencyException)
                {
                    
                }
                return RedirectToAction(nameof(Index));
            }

              ViewData["GenderID"] = new SelectList(_patientRepo.GetGenderList(), "ID", "Name");
            ViewData["BloodGroupID"] = new SelectList(_patientRepo.GetBloodGroupList(), "ID", "Name");
            return View(patientAddress);
        }

        // GET: Patient/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = _patientRepo.GetPatient(id);
            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);
        }

        // POST: Patient/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var patient = _patientRepo.GetPatient(id);
            if(patient==null)
                return NotFound();
        
            Address address=patient.address;
            patient=_patientRepo.Delete(patient);
            if(address!=null)
                _address.Delete(address);
            return RedirectToAction(nameof(Index));
        }
    }
}
