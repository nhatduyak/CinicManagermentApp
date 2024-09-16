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
using System.IO;
using FastReport;
using Microsoft.AspNetCore.Hosting;

namespace ClinicManagement.Controllers
{
    //[Route("Benhnhan/{action}/{id?}")]
    public class PrescriptionsController : Controller
    {
      //private readonly ClinicManagementDbContext _context;
        private readonly Iprescriptions _prescriptionsRepo;  
        private readonly IPrescriptionDetail _prescriptionsDetailRepo;

        public IWebHostEnvironment _env;
        

        public PrescriptionsController(Iprescriptions prescriptionsRepo,IPrescriptionDetail prescriptionsDetailRepo,IWebHostEnvironment env)
        {
            //_context = context;
            _prescriptionsRepo=prescriptionsRepo;
            _prescriptionsDetailRepo=prescriptionsDetailRepo;
            _env=env;
        }

        // GET: prescriptions
        public IActionResult Index(string sortExpression="", string SearchText = "",int pg=1,int pageSize=10)
        {
            SortModel sortModel=new SortModel();
            sortModel.AddColumn("datecreate");
            sortModel.AddColumn("code");
            sortModel.AddColumn("pname");
            sortModel.AddColumn("dname");
            
            sortModel.ApplySort(sortExpression);
            ViewData["sortModel"] = sortModel;


            ViewBag.SearchText=SearchText;
            PaginatedList<prescriptions> prescriptions=_prescriptionsRepo.GetItems(sortModel.SortedProperty,sortModel.SortedOrder,SearchText,pg,pageSize);
            var pager=new PagerModel(prescriptions.TotalRecords,pg,pageSize);
            pager.SortExpression=sortExpression;
            this.ViewBag.Pager=pager;

            TempData["CurrentPage"]=pg;
            return View(prescriptions);
            
        }

        // GET: prescriptions/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var prescriptions= _prescriptionsRepo.Getprescriptions(id);
         
            if (prescriptions == null)
            {
                return NotFound();
            }
            ViewData["MedicinesID"] = new SelectList(_prescriptionsRepo.GetMedicinesList(), "ID", "Name");
            MedicinesWhenToTake whenToTake=new MedicinesWhenToTake();
            ViewData["When_To_Take"] = new SelectList(whenToTake.GetListMedicinesWhenToTake(), "ID", "Name");
            return View(prescriptions);
        }

        // GET: prescriptions/Create
        public IActionResult Create()
        {
            ViewData["PatientID"] = new SelectList(_prescriptionsRepo.GetPatientList(), "ID", "FullName");
            ViewData["DoctorID"] = new SelectList(_prescriptionsRepo.GetDoctorList(), "ID", "FullName");
            ViewData["MedicinesID"] = new SelectList(_prescriptionsRepo.GetMedicinesList(), "ID", "Name");
            MedicinesWhenToTake whenToTake=new MedicinesWhenToTake();
            ViewData["When_To_Take"] = new SelectList(whenToTake.GetListMedicinesWhenToTake(), "ID", "Name");
            prescriptions prescriptions = new prescriptions();
            prescriptions.PrescriptionsDetail.Add(new PrescriptionDetail(){ID=1});
            //prescriptions.address=new Address();
            return View(prescriptions);
        }

        // POST: prescriptions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public IActionResult Create([Bind("ID,FName,LName,Age,Phone,Email,Address,GenderID,BloodGroupID,AddressID")] prescriptions prescriptions)
        public IActionResult Create(prescriptions prescriptions)
        {
            if (ModelState.IsValid)
            {
                
                    if (prescriptions.DoctorID == -1) prescriptions.DoctorID = null;
                    //if (prescriptions.address?.Street == string.Empty&&prescriptions.address?.City==string.Empty) prescriptions.AddressID = null;
                  
                prescriptions.DateCreate=DateTime.Now;
                prescriptions = _prescriptionsRepo.Create(prescriptions);
                return RedirectToAction(nameof(Index));

            }
               ViewData["PatientID"] = new SelectList(_prescriptionsRepo.GetPatientList(), "ID", "Name");
            ViewData["DoctorID"] = new SelectList(_prescriptionsRepo.GetDoctorList(), "ID", "Name");

            return View(prescriptions);
        }

        // GET: prescriptions/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewData["PatientID"] = new SelectList(_prescriptionsRepo.GetPatientList(), "ID", "FullName");
            ViewData["DoctorID"] = new SelectList(_prescriptionsRepo.GetDoctorList(), "ID", "FullName");
            ViewData["MedicinesID"] = new SelectList(_prescriptionsRepo.GetMedicinesList(), "ID", "Name");
            MedicinesWhenToTake whenToTake=new MedicinesWhenToTake();
            ViewData["When_To_Take"] = new SelectList(whenToTake.GetListMedicinesWhenToTake(), "ID", "Name");


            var prescriptions = _prescriptionsRepo.Getprescriptions(id);
           
            return View(prescriptions);
        }

        // POST: prescriptions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, prescriptions prescriptions)
        {
           
            if (ModelState.IsValid)
            {
                try
                {

                     prescriptions.DateCreate=DateTime.Now;
                    if (prescriptions.DoctorID == -1) prescriptions.DoctorID = null;                   

                 
                     _prescriptionsRepo.Edit(prescriptions);

                }
                catch (DbUpdateConcurrencyException)
                {
                    
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["PatientID"] = new SelectList(_prescriptionsRepo.GetPatientList(), "ID", "Name");
            ViewData["DoctorID"] = new SelectList(_prescriptionsRepo.GetDoctorList(), "ID", "Name");

            return View(prescriptions);
        }

        // GET: prescriptions/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prescriptions = _prescriptionsRepo.Getprescriptions(id);
            if (prescriptions == null)
            {
                return NotFound();
            }
            ViewData["MedicinesID"] = new SelectList(_prescriptionsRepo.GetMedicinesList(), "ID", "Name");
            MedicinesWhenToTake whenToTake=new MedicinesWhenToTake();
            ViewData["When_To_Take"] = new SelectList(whenToTake.GetListMedicinesWhenToTake(), "ID", "Name");
            return View(prescriptions);
        }

        // POST: prescriptions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var prescriptions = _prescriptionsRepo.Getprescriptions(id);
            if(prescriptions==null)
                return NotFound();
        
            prescriptions=_prescriptionsRepo.Delete(prescriptions);
            
            return RedirectToAction(nameof(Index));
        }

        public FileResult Generate(int id)
        {
            FastReport.Utils.Config.WebMode=true;
            Report report=new Report(); 
            string path="wwwroot/Report/Prescriptions.frx";
            report.Load(path);

            prescriptions prescriptions=_prescriptionsRepo.Getprescriptions(id);
            List<PrescriptionDetail> details=prescriptions.PrescriptionsDetail;
            List<ReportPrescriptions> reportPrescriptions=new List<ReportPrescriptions>();
            MedicinesWhenToTake whenToTake=new MedicinesWhenToTake();
            List<MedicinesWhenToTake> listwhentotake= whenToTake.GetListMedicinesWhenToTake();
            foreach(var item in details)
            {
                ReportPrescriptions prreport=new ReportPrescriptions();
                prreport.MedicinesName=item.Medicines.Name;
                prreport.No_of_day=item.No_of_day.ToString("N0");
                prreport.Dosage=item.Dosage;
                prreport.When_To_Take=listwhentotake.Where(p=>p.ID==item.ID).FirstOrDefault().Name;
                string uongtruocan="";
                if(item.is_Before_Meal)
                    {
                        uongtruocan=" trước khi ăn";
                    }
                prreport.uongtruocan=uongtruocan;
                reportPrescriptions.Add(prreport);
            }

            ClinicInfo info=_prescriptionsRepo.GetClinicInfo();
            report.SetParameterValue("clinicname",info?.Name);
            report.SetParameterValue("clinicphone",info?.Phone);
            report.SetParameterValue("clinicaddress",info?.Address);
            report.SetParameterValue("patientname",prescriptions.Patient.FullName);
            report.SetParameterValue("gender",prescriptions.Patient.gender?.Name);
            report.SetParameterValue("age",prescriptions.Patient?.Age);
            report.SetParameterValue("nextvisit",prescriptions?.NextVisit);
            report.SetParameterValue("doctorname",prescriptions.Doctor.FullName);
            report.SetParameterValue("note",prescriptions.Note);
            report.SetParameterValue("advice",prescriptions.advice);
            report.SetParameterValue("code",prescriptions.code);

            report.RegisterData(reportPrescriptions,"PrescriptionsRef");

            if(report.Report.Prepare())
            {
                FastReport.Export.PdfSimple.PDFSimpleExport pDFSimpleExport=new FastReport.Export.PdfSimple.PDFSimpleExport();
                pDFSimpleExport.ShowProgress=false;
                pDFSimpleExport.Subject="Subject report";
                pDFSimpleExport.Title="Title Report clinic";
                MemoryStream ms=new MemoryStream();
                report.Report.Export(pDFSimpleExport,ms);
                report.Dispose();
                pDFSimpleExport.Dispose();
                ms.Position=0;
                return File(ms,"application/pdf","DonThuoc.pdf");
            }
            else
            {
                return null;
            }

        }
       
    }
}
