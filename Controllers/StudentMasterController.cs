using Student_Report_Management.Models;
using Student_Report_Management.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Student_Report_Management.Controllers
{
    public class StudentMasterController : Controller
    {
        private SRM_conn db = new SRM_conn();


        // GET: StudentMaster
        public ActionResult GetStudentData(string StudentName)
        {
            var query = db.tbl_Student_Master
                                .Where(s=>string.IsNullOrEmpty(StudentName)|| s.Student_Name.Contains(StudentName))
                                .Select(s=> new StudentReportViewModel
                                        {
                                            Student_Id= s.Student_Id,
                                            Student_Name= s.Student_Name,
                                            Student_DOB= (DateTime)s.Student_DOB,
                                            Student_Email=s.Student_Email,
                                            Student_Mobile=s.Student_Mobile,
                                        }).ToList();

            //if(!string.IsNullOrEmpty(StudentName))
            //{
            //    query= query.Where(s=>s.Student_Name.Contains(StudentName));
            //}
            ViewBag.CurrentFilter = StudentName;

            var Result = query;
            return View("GetStudentData", Result);
        }

        //Create Student
        public ActionResult CreateStudent()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateStudent(StudentReportViewModel model)
        {
            if (ModelState.IsValid)
            {
                var student = new tbl_Student_Master
                {
                    Student_Name = model.Student_Name,
                    Student_Email = model.Student_Email,
                    Student_Mobile = model.Student_Mobile,
                    Student_DOB = model.Student_DOB,
                };
                db.tbl_Student_Master.Add(student);
                db.SaveChanges();

                return RedirectToAction("GetStudentData");
            }
            return View(model);
        }


        //Edit Student
        public ActionResult EditStudent(int id)
        {
            var student= db.tbl_Student_Master.Find(id);
            if(student==null)
            {
                return HttpNotFound();
            }
            var model = new StudentReportViewModel
            {
                Student_Id = student.Student_Id,
                Student_Name = student.Student_Name,
                Student_Email = student.Student_Email,
                Student_Mobile = student.Student_Mobile,
                Student_DOB = (DateTime)student.Student_DOB,
            };
            return View(model);
        }
        [HttpPost]
        public ActionResult EditStudent(StudentReportViewModel model)
        {
            if(ModelState.IsValid)
            {
                var student= db.tbl_Student_Master.Find(model.Student_Id);
                if(student == null)
                {
                    return HttpNotFound();
                }
                student.Student_Name = model.Student_Name;
                student.Student_Email = model.Student_Email;
                student.Student_Mobile = model.Student_Mobile;
                student.Student_DOB= model.Student_DOB;

                db.SaveChanges();
                return RedirectToAction("GetStudentData");
            }
            return View(model);
        }
    }
}