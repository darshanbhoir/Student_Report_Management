using Student_Report_Management.Models;
using Student_Report_Management.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Student_Report_Management.Controllers
{
    public class SemesterMasterController : Controller
    {
        private SRM_conn db = new SRM_conn();

        // GET: SemesterMaster
        public ActionResult GetSemesterData(string SemesterName)
        {
            var query= db.tbl_Semister_Master
                            .Where(s => string.IsNullOrEmpty(SemesterName) || s.Semister_Name.Contains(SemesterName))
                            .Select(s=>new StudentReportViewModel
                            {
                                Semister_Id= s.Semister_Id,
                                Semister_Name= s.Semister_Name,
                            }).ToList();

            ViewBag.CurrentFilter = SemesterName;

            var Result = query;
            return View(Result);
        }


        //Add New Semester
        public ActionResult AddSemester()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddSemester(StudentReportViewModel viewmodel)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    var semester = new tbl_Semister_Master
                    {
                        //Semister_Id= viewmodel.Semister_Id,
                        Semister_Name = viewmodel.Semister_Name,
                        Year_Id = viewmodel.Year_Id,
                    };
                    db.tbl_Semister_Master.Add(semester);
                    db.SaveChanges();

                    return RedirectToAction("GetSemesterData");
                }
                catch (Exception ex)
                {
                    string errorMessage = ex.Message;
                    //ViewBag.ErrorMessage = errorMessage;
                    Console.WriteLine(errorMessage);
                    return View("Error");
                }

            }
            return View(viewmodel);
        }
    }
}