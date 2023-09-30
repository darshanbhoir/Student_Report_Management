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
    }
}