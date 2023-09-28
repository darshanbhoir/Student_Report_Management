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
    public class SubjectMasterController : Controller
    {
        private SRM_conn db = new SRM_conn();

        // GET: SubjectMaster
        public ActionResult GetSubjectData(string SubjectName)
        {
            var query = db.tbl_Subject_Master
                            .Where(s=>string.IsNullOrEmpty(SubjectName)|| s.Subject_Name.Contains(SubjectName))
                            .Select(s=>new StudentReportViewModel
                            {
                                Subject_Id= s.Subject_Id,
                                Subject_Name= s.Subject_Name,
                            }).ToList();
            ViewBag.CurrentFilter = SubjectName;
            var Result = query;
            return View("GetSubjectData", Result);
        }


        //Add Subject
        public ActionResult AddSubject()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddSubject(StudentReportViewModel model)
        {
            if(ModelState.IsValid)
            {
                var Subject = new tbl_Subject_Master
                {
                    //Subject_Id = model.Subject_Id,
                    Subject_Name = model.Subject_Name,
                };
                db.tbl_Subject_Master.Add(Subject);

                db.SaveChanges();

                return RedirectToAction("GetSubjectData");
            }
            return View(model);
        }


        //Edit Subject
        public ActionResult EditSubject(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var subject = db.tbl_Subject_Master.Find(id);
            if(subject== null)
            {
                return HttpNotFound();
            }
            var viewmodel = new StudentReportViewModel
            {
                Subject_Id= subject.Subject_Id,
                Subject_Name = subject.Subject_Name,
            };
            return View(viewmodel);
        }
        [HttpPost]
        public ActionResult EditSubject(StudentReportViewModel viewmodel)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    var subject = db.tbl_Subject_Master.Find(viewmodel.Subject_Id);
                    if(subject==null)
                    {
                        return HttpNotFound();
                    }
                    
                    subject.Subject_Name= viewmodel.Subject_Name;

                    db.Entry(subject).State= System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("GetSubjectData");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred while updating the Subject.");
                }
            }
            return View(viewmodel);
        }



        //Delete Subject
        public ActionResult DeleteSubject(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var subject = db.tbl_Subject_Master.Find(id);
            if (subject == null)
            {
                return HttpNotFound();
            }
            var viewmodel = new StudentReportViewModel
            {
                Subject_Id = subject.Subject_Id,
                Subject_Name = subject.Subject_Name,
            };
            return View(viewmodel);
        }
        [HttpPost]
        public ActionResult DeleteSubject(StudentReportViewModel viewmodel)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    var subject = db.tbl_Subject_Master.Find(viewmodel.Subject_Id);
                    if (subject == null)
                    {
                        return HttpNotFound();
                    }
                    db.tbl_Subject_Master.Remove(subject);
                    db.SaveChanges();

                    return RedirectToAction("GetSubjectData");
                }
                catch(Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred while Deleting the Subject.");
                }
            }
            return View(viewmodel);
        }
    }
}