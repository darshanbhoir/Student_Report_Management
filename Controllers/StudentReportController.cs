using Student_Report_Management.Models;
using Student_Report_Management.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Student_Report_Management.Controllers
{
    public class StudentReportController : Controller
    {
        private SRM_conn db = new SRM_conn();


        // GET: StudentReport
        public ActionResult GetStudentReport(string StudentName)
        {
            var query = db.tbl_Student_Report
                            .Where(s=>string.IsNullOrEmpty(StudentName)||s.tbl_Student_Master.Student_Name.Contains(StudentName))
                            .Select(s => new StudentReportViewModel
                            {
                                Report_Id = s.Report_Id,
                                Student_Id = s.Student_Id,
                                Student_Name = s.tbl_Student_Master.Student_Name,
                                Subject_Name = s.tbl_Semister_Subject_Map.tbl_Subject_Master.Subject_Name,
                                User_Score = (int)s.User_Score,
                                Updated_On = (DateTime)s.Updated_On,
                            }).ToList();

            ViewBag.CurrentFilter = StudentName;
            var Result = query;
            return View(Result);
        }


        //Add New Report
        public ActionResult AddReport()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddReport(StudentReportViewModel viewmodel)
        {
            if(ModelState.IsValid)
            {
                var report = new tbl_Student_Report
                {
                    Student_Id = viewmodel.Student_Id,
                    Sem_Subject_Id = viewmodel.Sem_Subject_Id,
                    User_Score = viewmodel.User_Score,
                    Updated_On = viewmodel.Updated_On,
                };
                db.tbl_Student_Report.Add(report);
                db.SaveChanges();

                return RedirectToAction("GetStudentReport");
            }
            return View(viewmodel);
        }
    }
}