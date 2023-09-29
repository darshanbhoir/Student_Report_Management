using Student_Report_Management.Models;
using Student_Report_Management.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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


        //Edit Semester
        public ActionResult EditSemester(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var semester = db.tbl_Semister_Master.Find(id);
            if(semester==null)
            {
                return HttpNotFound();
            }
            var viewmodel = new StudentReportViewModel
            {
                Semister_Id = semester.Semister_Id,
                Semister_Name = semester.Semister_Name,
                Year_Id = semester.Year_Id,
            };
            return View(viewmodel);
        }
        [HttpPost]
        public ActionResult EditSemester(StudentReportViewModel viewmodel)
        {
            if(ModelState.IsValid)
            {
                //try
                //{
                    var semester = db.tbl_Semister_Master.Find(viewmodel.Semister_Id);
                    if(semester==null)
                    {
                        return HttpNotFound();
                    }
                    semester.Semister_Name = viewmodel.Semister_Name;
                    semester.Year_Id = viewmodel.Year_Id;

                    db.Entry(semester).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("GetSemesterData");
                //}
                //catch (Exception ex)
                //{
                //    ModelState.AddModelError("", "An error occurred while Updating the Semester.");
                //}
            }
            return View(viewmodel);
        }


        //Delete Semester
        public ActionResult DeleteSemester(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var semester = db.tbl_Semister_Master.Find(id);
            if (semester == null)
            {
                return HttpNotFound();
            }
            var viewmodel = new StudentReportViewModel
            {
                Semister_Id = semester.Semister_Id,
                Semister_Name = semester.Semister_Name,
                Year_Id = semester.Year_Id,
            };
            return View(viewmodel);
        }
        [HttpPost]
        public ActionResult DeleteSemester(StudentReportViewModel viewmodel)
        {
            if(ModelState.IsValid)
            {
                var semester = db.tbl_Semister_Master.Find(viewmodel.Semister_Id);
                if (semester == null)
                {
                    return HttpNotFound();
                }
                db.tbl_Semister_Master.Remove(semester);
                db.SaveChanges();
                return RedirectToAction("GetSemesterData");
            }
            return View(viewmodel);
        }
    }
}