using Student_Report_Management.Models;
using Student_Report_Management.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Student_Report_Management.Controllers
{
    public class SemSubMapController : Controller
    {
        private SRM_conn db = new SRM_conn();

        // GET: SemSubMap
        public ActionResult GetSemSubData(string SubjectName)
        {
            var query = db.tbl_Semister_Subject_Map
                            .Where(s => string.IsNullOrEmpty(SubjectName) || s.tbl_Subject_Master.Subject_Name.Contains(SubjectName))
                            .Select(s => new StudentReportViewModel
                            {
                                Sem_Subject_Id = s.Sem_Subject_Id,
                                Semister_Id = s.Semister_Id,
                                Semister_Name= s.tbl_Semister_Master.Semister_Name,
                                Subject_Id = s.Subject_Id,
                                Subject_Name= s.tbl_Subject_Master.Subject_Name,
                                Max_Score = (int)s.Max_Score,
                            }).ToList();

            ViewBag.CurrentFilter = SubjectName;
            var Result = query;
            return View(Result);
        }


        //Add Details 
        public ActionResult AddDetail()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddDetail(StudentReportViewModel viewmodel)
        {
            if(ModelState.IsValid)
            {
                var detail = new tbl_Semister_Subject_Map
                {
                    Semister_Id = viewmodel.Semister_Id,
                    Subject_Id = viewmodel.Subject_Id,
                    Max_Score = viewmodel.Max_Score,
                };
                db.tbl_Semister_Subject_Map.Add(detail);
                db.SaveChanges();

                return RedirectToAction("GetSemSubData");
            }
            return View(viewmodel);
        }
    }
}