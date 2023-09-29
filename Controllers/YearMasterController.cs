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
    public class YearMasterController : Controller
    {
        private SRM_conn db = new SRM_conn();

        // GET: YearMaster
        public ActionResult GetYearData(string YearName)
        {
            var query= db.tbl_Year_Master
                            .Where(s => string.IsNullOrEmpty(YearName) || s.Year_Name.Contains(YearName))
                            .Select(s=>new StudentReportViewModel
                            {
                                Year_Id= s.Year_Id,
                                Year_Name= s.Year_Name,
                            }).ToList();
            ViewBag.CurrentFilter = YearName;
            var Result = query;
            return View("GetYearData", Result);
        }


        //Add New year
        public ActionResult AddYear()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddYear(StudentReportViewModel viewmodel)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    var semester = new tbl_Year_Master
                    {
                        Year_Name = viewmodel.Year_Name,
                    };
                    db.tbl_Year_Master.Add(semester);
                    db.SaveChanges();

                    return RedirectToAction("GetYearData");
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



        //Edit Year
        public ActionResult EditYear(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var year = db.tbl_Year_Master.Find(id);
            if(year==null)
            {
                HttpNotFound();
            }
            var viewmodel = new StudentReportViewModel
            {
                Year_Id = year.Year_Id,
                Year_Name = year.Year_Name,
            };
            return View(viewmodel);
        }
        [HttpPost]
        public ActionResult EditYear(StudentReportViewModel viewmodel)
        {
            if(ModelState.IsValid)
            {
                var year = db.tbl_Year_Master.Find(viewmodel.Year_Id);
                if(year==null)
                {
                    return HttpNotFound();
                }
                year.Year_Name = viewmodel.Year_Name;
                db.Entry(year).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("GetYearData");
            }
            return View(viewmodel);
        }
        


        //Delete Year
        public ActionResult DeleteYear(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var year = db.tbl_Year_Master.Find(id);
            if (year == null)
            {
                HttpNotFound();
            }
            var viewmodel = new StudentReportViewModel
            {
                Year_Id = year.Year_Id,
                Year_Name = year.Year_Name,
            };
            return View(viewmodel);
        }
        [HttpPost]
        public ActionResult DeleteYear(StudentReportViewModel viewmodel)
        {
            if(ModelState.IsValid)
            {
                var year = db.tbl_Year_Master.Find(viewmodel.Year_Id);
                if(year==null)
                {
                    return HttpNotFound();
                }
                db.tbl_Year_Master.Remove(year);
                db.SaveChanges();
                return RedirectToAction("GetYearData");
            }
            return View(viewmodel);

        }
    }
}