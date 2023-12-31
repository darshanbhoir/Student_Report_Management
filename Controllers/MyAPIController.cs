﻿using Student_Report_Management.Models;
using Student_Report_Management.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Student_Report_Management.Controllers
{
    public class MyAPIController : Controller
    {
        private SRM_conn db = new SRM_conn();

        // GET: Student Data for Particular Year
        public ActionResult Index(int? StudentId, string YearName)
        {
            var query = from StudentReport in db.tbl_Student_Report
                        join StudentMaster in db.tbl_Student_Master on StudentReport.Student_Id equals StudentMaster.Student_Id
                        join SemisterSubjectMap in db.tbl_Semister_Subject_Map on StudentReport.Sem_Subject_Id equals SemisterSubjectMap.Sem_Subject_Id
                        join SemesterMaster in db.tbl_Semister_Master on SemisterSubjectMap.Semister_Id equals SemesterMaster.Semister_Id
                        join YearMaster in db.tbl_Year_Master on SemesterMaster.Year_Id equals YearMaster.Year_Id
                        join SubjectMaster in db.tbl_Subject_Master on SemisterSubjectMap.Subject_Id equals SubjectMaster.Subject_Id
                        where StudentReport.Student_Id == StudentId && YearMaster.Year_Name == YearName
                        select new StudentReportViewModel
                        {
                            Student_Name = StudentMaster.Student_Name,
                            Sem_Subject_Id = SemisterSubjectMap.Sem_Subject_Id,
                            Semister_Name = SemesterMaster.Semister_Name,
                            Max_Score= (int)SemisterSubjectMap.Max_Score,
                            Subject_Name=SubjectMaster.Subject_Name,
                            User_Score= (int)StudentReport.User_Score
                        };

            var Result = query.ToList();
            //var json = new JavaScriptSerializer().Serialize(Result);
            return View("Index", Result);
        }


        //Semester wise Particular Student Data
        public ActionResult SemesterwiseStudent(string StudentName)
        {
            var query = from StudentReport in db.tbl_Student_Report
                        join StudentMaster in db.tbl_Student_Master on StudentReport.Student_Id equals StudentMaster.Student_Id
                        join SemesterSubjectMap in db.tbl_Semister_Subject_Map on StudentReport.Sem_Subject_Id equals SemesterSubjectMap.Sem_Subject_Id
                        join SemesterMaster in db.tbl_Semister_Master on SemesterSubjectMap.Semister_Id equals SemesterMaster.Semister_Id
                        where StudentMaster.Student_Name == StudentName
                        orderby SemesterMaster.Semister_Name
                        select new StudentReportViewModel
                        {
                            Report_Id=StudentReport.Report_Id,
                            Student_Id=StudentReport.Student_Id,
                            User_Score= (int)StudentReport.User_Score,
                            Student_Name=StudentMaster.Student_Name,
                            Student_Mobile=StudentMaster.Student_Mobile,
                            Semister_Id=SemesterSubjectMap.Semister_Id,
                            Semister_Name=SemesterMaster.Semister_Name
                        };

            var Result = query.ToList();
            //var json = new JavaScriptSerializer().Serialize(Result);
            return View("SemesterwiseStudent",Result);
        }



        //Student Score Yearwise
        public ActionResult StudentScoreYearwise(int? StudentId)
        {
            var query = from StudentReport in db.tbl_Student_Report
                        join StudentMaster in db.tbl_Student_Master on StudentReport.Student_Id equals StudentMaster.Student_Id
                        join SemisterSubjectMap in db.tbl_Semister_Subject_Map on StudentReport.Sem_Subject_Id equals SemisterSubjectMap.Sem_Subject_Id
                        join SemesterMaster in db.tbl_Semister_Master on SemisterSubjectMap.Semister_Id equals SemesterMaster.Semister_Id
                        join YearMaster in db.tbl_Year_Master on SemesterMaster.Year_Id equals YearMaster.Year_Id
                        where StudentReport.Student_Id == StudentId
                        select new StudentReportViewModel
                        {
                            Student_Id=StudentReport.Student_Id,
                            Student_Name=StudentMaster.Student_Name,
                            Report_Id=StudentReport.Report_Id,
                            Semister_Name=SemesterMaster.Semister_Name,
                            Year_Name=YearMaster.Year_Name,
                            User_Score= (int)StudentReport.User_Score
                        };

            var Result = query.ToList();
            //var json = new JavaScriptSerializer().Serialize(Result);
            return View("StudentScoreYearwise", Result);
        }



        //Overall Score for year desc
        public ActionResult OverallScoreforYear(string YearName)
        {
            var query = from SR in db.tbl_Student_Report
                        join SSM in db.tbl_Semister_Subject_Map on SR.Sem_Subject_Id equals SSM.Sem_Subject_Id
                        join SM in db.tbl_Semister_Master on SSM.Semister_Id equals SM.Semister_Id
                        join YM in db.tbl_Year_Master on SM.Year_Id equals YM.Year_Id
                        join Student in db.tbl_Student_Master on SR.Student_Id equals Student.Student_Id
                        where YM.Year_Name == YearName
                        orderby Student.Student_Name, 
                                SR.User_Score descending
                        select new StudentReportViewModel
                        {
                            Student_Name=Student.Student_Name,
                            User_Score=(int)SR.User_Score,
                            Max_Score=(int)SSM.Max_Score,
                            Year_Name=YM.Year_Name,
                            Semister_Name=SM.Semister_Name
                        };
            var Result = query.ToList();
            //var json = new JavaScriptSerializer().Serialize(Result);
            return View("OverallScoreforYear", Result);
        }

        //Top subject Semister wise of a particular user for a particular year.
        public ActionResult StudentMaxScoreYear(string StudentName, string YearName)
        {
            var query = from SR in db.tbl_Student_Report
                        join Student in db.tbl_Student_Master on SR.Student_Id equals Student.Student_Id
                        join SSM in db.tbl_Semister_Subject_Map on SR.Sem_Subject_Id equals SSM.Sem_Subject_Id
                        join SM in db.tbl_Semister_Master on SSM.Semister_Id equals SM.Semister_Id
                        join YM in db.tbl_Year_Master on SM.Year_Id equals YM.Year_Id
                        where Student.Student_Name == StudentName && YM.Year_Name == YearName
                        group SR by Student.Student_Name into StudentGroup
                        select new StudentReportViewModel
                        {
                            Student_Name = StudentGroup.Key,
                            Max_Score = (int)StudentGroup.Max(x => x.User_Score)
                        };
            var Result = query.ToList();
            //var json = new JavaScriptSerializer().Serialize(Result);
            return View("StudentMaxScoreYear", Result);
        }



        // Students with subject details semister wise
        public ActionResult StudentTopSubjectfromSem(string StudentName, string YearName)
        {
            var query = (from SR in db.tbl_Student_Report
                         join Student in db.tbl_Student_Master on SR.Student_Id equals Student.Student_Id
                         join SSM in db.tbl_Semister_Subject_Map on SR.Sem_Subject_Id equals SSM.Sem_Subject_Id
                         join SM in db.tbl_Semister_Master on SSM.Semister_Id equals SM.Semister_Id
                         join YM in db.tbl_Year_Master on SM.Year_Id equals YM.Year_Id
                         join Sub in db.tbl_Subject_Master on SSM.Subject_Id equals Sub.Subject_Id
                         where Student.Student_Name == StudentName && YM.Year_Name == YearName
                         select new
                         {
                             Student.Student_Name,
                             YM.Year_Name,
                             SM.Semister_Name,
                             Sub.Subject_Name,
                             SR.User_Score,

                         })
                        .ToList()
                        .GroupBy(g => g.Semister_Name)
                        .SelectMany(s => s.OrderByDescending(x => x.User_Score)
                                           .Select((x, index) =>
                                           new StudentReportViewModel
                                           {
                                               Student_Name=x.Student_Name,
                                               Year_Name=x.Year_Name,
                                               Semister_Name=x.Semister_Name,
                                               Subject_Name=x.Subject_Name,
                                               User_Score=(int)x.User_Score,
                                               Sem = index + 1
                                           }))
                        .Where(x => x.Sem == 1);

            var Result = query.ToList();
            //var json = new JavaScriptSerializer().Serialize(Result);
            return View(Result);
        }


        //Top Scorer from all Students with subject details
        public ActionResult TopScorer()
        {
            var query = (from SR in db.tbl_Student_Report
                         join Student in db.tbl_Student_Master on SR.Student_Id equals Student.Student_Id
                         join SSM in db.tbl_Semister_Subject_Map on SR.Sem_Subject_Id equals SSM.Sem_Subject_Id
                         join Subject in db.tbl_Subject_Master on SSM.Subject_Id equals Subject.Subject_Id
                         join Semester in db.tbl_Semister_Master on SSM.Semister_Id equals Semester.Semister_Id
                         join Year in db.tbl_Year_Master on Semester.Year_Id equals Year.Year_Id
                         orderby SR.User_Score descending
                         select new StudentReportViewModel
                         {
                             Student_Id=SR.Student_Id,
                             Student_Name=Student.Student_Name,
                             Subject_Name=Subject.Subject_Name,
                             User_Score= (int)SR.User_Score,
                             Semister_Name=Semester.Semister_Name,
                             Year_Name=Year.Year_Name,
                         }).Take(1);
            var Result = query.ToList();
            //var json = new JavaScriptSerializer().Serialize(Result);
            return View("TopScorer", Result);
        }



        //Top Scorer Year wise
        public ActionResult TopScorerYearwise()
        {
            var query = (from SR in db.tbl_Student_Report
                         join Student in db.tbl_Student_Master on SR.Student_Id equals Student.Student_Id
                         join SSM in db.tbl_Semister_Subject_Map on SR.Sem_Subject_Id equals SSM.Sem_Subject_Id
                         join Subject in db.tbl_Subject_Master on SSM.Subject_Id equals Subject.Subject_Id
                         join Semester in db.tbl_Semister_Master on SSM.Semister_Id equals Semester.Semister_Id
                         join Year in db.tbl_Year_Master on Semester.Year_Id equals Year.Year_Id
                         select new 
                         {
                             SR.Student_Id,
                             Student.Student_Name,
                             Subject.Subject_Name,
                             SR.User_Score,
                             Semester.Semister_Name,
                             Year.Year_Name,
                         })
                        .ToList()
                        .GroupBy(g => g.Year_Name)
                        .SelectMany(s => s.OrderByDescending(g => g.User_Score)
                                         .Select((g, index) =>
                                         new StudentReportViewModel
                                         {
                                             Student_Id=g.Student_Id,
                                             Student_Name=g.Student_Name,
                                             Subject_Name=g.Subject_Name,
                                             User_Score=(int)g.User_Score,
                                             Semister_Name=g.Semister_Name,
                                             Year_Name=g.Year_Name,
                                             Years = index + 1
                                         }))
                        .Where(x => x.Years == 1);

            var Result = query.ToList();
            //var json = new JavaScriptSerializer().Serialize(Result);
            return View("TopScorerYearwise", Result);


        }



        //Semister wise subject data with max score
        public ActionResult SemwiseSubjectData(string SubjectName)
        {
            var query = from SSM in db.tbl_Semister_Subject_Map
                        join Semester in db.tbl_Semister_Master on SSM.Semister_Id equals Semester.Semister_Id
                        join Subject in db.tbl_Subject_Master on SSM.Subject_Id equals Subject.Subject_Id
                        select new StudentReportViewModel
                        {
                            Sem_Subject_Id=SSM.Sem_Subject_Id,
                            Subject_Name=Subject.Subject_Name,
                            Max_Score= (int)SSM.Max_Score,
                            Semister_Name=Semester.Semister_Name,
                        };

            if(!string.IsNullOrEmpty(SubjectName))
            {
                query = query.Where(s => s.Subject_Name.Contains(SubjectName));
            }
            ViewBag.CurrentFilter = SubjectName;

            var Result = query.ToList();
            //var json = new JavaScriptSerializer().Serialize(Result);
            return View("SemwiseSubjectData", Result);

        }


        //Particular Students top subject from each Semister
        public ActionResult StudentTopSubEachSem(string StudentName)
        {
            var query = (from SR in db.tbl_Student_Report
                         join Student in db.tbl_Student_Master on SR.Student_Id equals Student.Student_Id
                         join SSM in db.tbl_Semister_Subject_Map on SR.Sem_Subject_Id equals SSM.Sem_Subject_Id
                         join SM in db.tbl_Semister_Master on SSM.Semister_Id equals SM.Semister_Id
                         join YM in db.tbl_Year_Master on SM.Year_Id equals YM.Year_Id
                         where Student.Student_Name== StudentName
                         select new
                         {
                             SR.Student_Id,
                             Student.Student_Name,
                             SM.Semister_Name,
                             SR.User_Score,
                             YM.Year_Name,
                         })
                       .ToList()
                       .GroupBy(g => g.Semister_Name)
                       .SelectMany(s => s.OrderByDescending(g => g.User_Score)
                                        .Select((g, index) =>
                                        new StudentReportViewModel
                                        {
                                            Student_Id=g.Student_Id,
                                            Student_Name=g.Student_Name,
                                            Semister_Name=g.Semister_Name,
                                            User_Score=(int)g.User_Score,
                                            Year_Name=g.Year_Name,
                                            Sem = index + 1
                                        }))
                       .Where(x => x.Sem == 1);

            var Result = query.ToList();
            //var json = new JavaScriptSerializer().Serialize(Result);
            return View("StudentTopSubEachSem", Result);
        }


        //Left Outer Join
        //public string LeftOuterjoin()
        //{
        //    var query = from SR in db.tbl_Student_Report
        //                join SSM in db.tbl_Semister_Subject_Map on SR.Sem_Subject_Id equals SSM.Sem_Subject_Id
        //                        into SSMGroup
        //                from SSM in SSMGroup.DefaultIfEmpty()
        //                select SR;

        //    var Result = query.ToList();
        //    var json = new JavaScriptSerializer().Serialize(Result);
        //    return json;

        //}


        //Lowest Scorer Year Wise
        public ActionResult LowestScorer()
        {
            var query = (from SR in db.tbl_Student_Report
                         join Student in db.tbl_Student_Master on SR.Student_Id equals Student.Student_Id
                         join SSM in db.tbl_Semister_Subject_Map on SR.Sem_Subject_Id equals SSM.Sem_Subject_Id
                         join SM in db.tbl_Subject_Master on SSM.Subject_Id equals SM.Subject_Id
                         join Sem in db.tbl_Semister_Master on SSM.Semister_Id equals Sem.Semister_Id
                         join YM in db.tbl_Year_Master on Sem.Year_Id equals YM.Year_Id
                         select new
                         {
                             SR.Student_Id,
                             Student.Student_Name,
                             SM.Subject_Name,
                             SR.User_Score,
                             Sem.Semister_Name,
                             YM.Year_Name
                         })
                        .ToList()
                        .GroupBy(y => y.Year_Name)
                        .SelectMany(s => s.OrderBy(u => u.User_Score)
                                            .Select((u, Index) =>
                                            new StudentReportViewModel
                                            {
                                                Student_Id=u.Student_Id,
                                                Student_Name=u.Student_Name,
                                                Subject_Name=u.Subject_Name,
                                                User_Score=(int)u.User_Score,
                                                Semister_Name = u.Semister_Name,
                                                Year_Name = u.Year_Name,
                                                Years = Index + 1
                                            }))
                        .Where(y => y.Years == 1);

            var Result = query.ToList();
            //var json = new JavaScriptSerializer().Serialize(Result);
            return View("LowestScorer", Result);
        }


        //Particular Students Lowest Scores modified
        public ActionResult StudentsLowestScore(string StudentName)
        {
            var query = (from SR in db.tbl_Student_Report
                         join Student in db.tbl_Student_Master on SR.Student_Id equals Student.Student_Id
                         join SSM in db.tbl_Semister_Subject_Map on SR.Sem_Subject_Id equals SSM.Sem_Subject_Id
                         join SM in db.tbl_Subject_Master on SSM.Subject_Id equals SM.Subject_Id
                         join Sem in db.tbl_Semister_Master on SSM.Semister_Id equals Sem.Semister_Id
                         join YM in db.tbl_Year_Master on Sem.Year_Id equals YM.Year_Id
                         where Student.Student_Name == StudentName
                         select new
                         {
                             SR.Student_Id,
                             Student.Student_Name,
                             SM.Subject_Name,
                             SR.User_Score,
                             Sem.Semister_Name,
                             YM.Year_Name
                         })
                        .ToList()
                        .GroupBy(y => y.Year_Name)
                        .SelectMany(s => s.OrderBy(u => u.User_Score)
                                            .Select((u, Index) =>
                                            new StudentReportViewModel
                                            {
                                                Student_Id=u.Student_Id,
                                                Student_Name=u.Student_Name,
                                                Subject_Name = u.Subject_Name,
                                                User_Score=(int)u.User_Score,
                                                Semister_Name=u.Semister_Name,
                                                Year_Name=u.Year_Name,
                                                Years = Index + 1
                                            }))
                        .Where(y => y.Years == 1);

            var Result = query.ToList();
            //var json = new JavaScriptSerializer().Serialize(Result);
            return View("StudentsLowestScore", Result);
        }


        //Students passed in particular Subject
        public ActionResult PassedStudent(string SubjectName)
        {
            var query = from SR in db.tbl_Student_Report
                        join Student in db.tbl_Student_Master on SR.Student_Id equals Student.Student_Id
                        join SSM in db.tbl_Semister_Subject_Map on SR.Sem_Subject_Id equals SSM.Sem_Subject_Id
                        join Subject in db.tbl_Subject_Master on SSM.Subject_Id equals Subject.Subject_Id
                        join Sem in db.tbl_Semister_Master on SSM.Semister_Id equals Sem.Semister_Id
                        join Year in db.tbl_Year_Master on Sem.Year_Id equals Year.Year_Id
                        where Subject.Subject_Name == SubjectName && SR.User_Score > 24
                        select new StudentReportViewModel
                        {
                            Student_Id=SR.Student_Id,
                            Student_Name=Student.Student_Name,
                            Subject_Name=Subject.Subject_Name,
                            User_Score=(int)SR.User_Score,
                            Semister_Name=Sem.Semister_Name,
                            Year_Name=Year.Year_Name
                        };

            var Result = query.ToList();
            //var json = new JavaScriptSerializer().Serialize(Result);
            return View("PassedStudent", Result);
        }


        //Count of Students Passed in particular Subject
        public ActionResult PassedStudentCount(string SubjectName)
        {
            var query = from SR in db.tbl_Student_Report
                        join SSM in db.tbl_Semister_Subject_Map on SR.Sem_Subject_Id equals SSM.Sem_Subject_Id
                        join Subject in db.tbl_Subject_Master on SSM.Subject_Id equals Subject.Subject_Id
                        where Subject.Subject_Name == SubjectName && SR.User_Score >= 24
                        group SR by Subject.Subject_Name into Sub
                        select new StudentReportViewModel
                        {
                            Subject_Name = Sub.Key,
                            Count = Sub.Count()
                        };

            var Result = query.ToList();
            //var json = new JavaScriptSerializer().Serialize(Result);
            return View("PassedStudentCount", Result);

        }


        //Avg of Student marks Semester wise
        public ActionResult AvgMarksSemesterwise(string StudentName)
        {
            var query = from SR in db.tbl_Student_Report
                        join Student in db.tbl_Student_Master on SR.Student_Id equals Student.Student_Id
                        join SSM in db.tbl_Semister_Subject_Map on SR.Sem_Subject_Id equals SSM.Sem_Subject_Id
                        join Sem in db.tbl_Semister_Master on SSM.Semister_Id equals Sem.Semister_Id
                        where Student.Student_Name == StudentName
                        group SR by new
                        {
                            Student.Student_Name,
                            Sem.Semister_Name
                        } into X
                        select new StudentReportViewModel
                        {
                            Student_Name = X.Key.Student_Name,
                            Semister_Name = X.Key.Semister_Name,
                            Average = (double)X.Average(u => u.User_Score)
                        };

            var Result = query.ToList();
            //var json = new JavaScriptSerializer().Serialize(Result);
            return View("AvgMarksSemesterwise", Result);

        }


        //Sum and Avg of Particular Student for Semester
        public ActionResult SumandAvgofStudent(string StudentName)
        {
            var query = from SR in db.tbl_Student_Report
                        join Student in db.tbl_Student_Master on SR.Student_Id equals Student.Student_Id
                        join SSM in db.tbl_Semister_Subject_Map on SR.Sem_Subject_Id equals SSM.Sem_Subject_Id
                        join Sem in db.tbl_Semister_Master on SSM.Semister_Id equals Sem.Semister_Id
                        where Student.Student_Name == StudentName
                        group SR by new
                        {
                            Student.Student_Name,
                            Sem.Semister_Name
                        } into X
                        select new StudentReportViewModel
                        {
                            Student_Name = X.Key.Student_Name,
                            Semister_Name = X.Key.Semister_Name,
                            Count = X.Count(),
                            Total= (int)X.Sum(u=>u.User_Score),
                            Average = (double)X.Average(u => u.User_Score)
                        };

            var Result = query.ToList();
            //var json = new JavaScriptSerializer().Serialize(Result);
            return View("SumandAvgofStudent", Result);
        }


        //Student-Subject with avg more than 60
        public string StudentAvgmorethan(string StudentName)
        {
            var query = from SR in db.tbl_Student_Report
                        join Student in db.tbl_Student_Master on SR.Student_Id equals Student.Student_Id
                        join SSM in db.tbl_Semister_Subject_Map on SR.Sem_Subject_Id equals SSM.Sem_Subject_Id
                        join Sem in db.tbl_Semister_Master on SSM.Semister_Id equals Sem.Semister_Id
                        where Student.Student_Name == StudentName
                        group SR by new
                        {
                            Student.Student_Name,
                            Sem.Semister_Name
                        } into X
                        where X.Average(u => u.User_Score)>60
                        select new
                        {
                            StudentName = X.Key.Student_Name,
                            SemisterName = X.Key.Semister_Name,
                            SubjectCount = X.Count(),
                            Total = X.Sum(u => u.User_Score),
                            Average = X.Average(u => u.User_Score)
                        };

            var Result = query.ToList();
            var json = new JavaScriptSerializer().Serialize(Result);
            return json;
        }

        //List of Students with more than 60 avg
        public ActionResult StudentList()  
        {
            var query = from SR in db.tbl_Student_Report
                        join Student in db.tbl_Student_Master on SR.Student_Id equals Student.Student_Id
                        join SSM in db.tbl_Semister_Subject_Map on SR.Sem_Subject_Id equals SSM.Sem_Subject_Id
                        join Sem in db.tbl_Semister_Master on SSM.Semister_Id equals Sem.Semister_Id
                        group SR by new 
                        {
                            Student.Student_Name,
                            Sem.Semister_Name
                        } into X
                        where X.Average(u => u.User_Score) > 60
                        select new StudentReportViewModel
                        {
                            Student_Name = X.Key.Student_Name,
                            Semister_Name = X.Key.Semister_Name,
                            Count = X.Count(),
                            Total = (int)X.Sum(u => u.User_Score),
                            Average = (double)X.Average(u => u.User_Score)
                        };

            var Result = query.ToList();
            //var json = new JavaScriptSerializer().Serialize(Result);
            
            return View("StudentList", Result);
        }


        //Percentage of Particular Student Semester wise
        public ActionResult PercentageofStudent(string StudentName)
        {
            var query = from SR in db.tbl_Student_Report
                        join Student in db.tbl_Student_Master on SR.Student_Id equals Student.Student_Id
                        join SSM in db.tbl_Semister_Subject_Map on SR.Sem_Subject_Id equals SSM.Sem_Subject_Id
                        join Sem in db.tbl_Semister_Master on SSM.Semister_Id equals Sem.Semister_Id
                        where Student.Student_Name == StudentName
                        group new { SR, SSM } by new
                        {
                            Student.Student_Name,
                            Sem.Semister_Name
                        } into X                        
                        select new StudentReportViewModel
                        {
                            Student_Name = X.Key.Student_Name,
                            Semister_Name = X.Key.Semister_Name,
                            Obtained = (int)X.Sum(u => u.SR.User_Score),
                            Total = (int)X.Sum(v=>v.SSM.Max_Score),
                            Percentage = (double)((X.Sum(u => u.SR.User_Score))*100.0/ X.Sum(v => v.SSM.Max_Score))
                        };

            var Result = query.ToList();
            //var json = new JavaScriptSerializer().Serialize(Result);
            return View("PercentageofStudent", Result);
        }



        //Count of Subjects in each Semester
        public ActionResult SubjectCount()
        {
            var query = from SSM in db.tbl_Semister_Subject_Map
                        join Sem in db.tbl_Semister_Master on SSM.Semister_Id equals Sem.Semister_Id
                        join Subject in db.tbl_Subject_Master on SSM.Subject_Id equals Subject.Subject_Id
                        group SSM by Sem.Semister_Name into Sem
                        select new StudentReportViewModel
                        {
                            Semister_Name = Sem.Key,
                            Count = Sem.Count()
                        };

            var Result = query.ToList();
            //var json = new JavaScriptSerializer().Serialize(Result);
            return View("SubjectCount", Result);

        }


        //Students Score using OR operators 
        public string StudentScore()
        {
            var query = from SR in db.tbl_Student_Report
                        join Student in db.tbl_Student_Master on SR.Student_Id equals Student.Student_Id
                        where SR.User_Score < 32 || SR.User_Score > 60
                        select new
                        {
                            Student.Student_Name,
                            SR.User_Score
                        };
            var Result = query.ToList();
            var json = new JavaScriptSerializer().Serialize(Result);
            return json;

        }



        //Determine Age
        //public string StudentsAge()
        //{
        //    var query = from Student in db.tbl_Student_Master
        //                select new
        //                {
        //                    Student.Student_Name,
        //                    Student.Student_DOB,
        //                    Age = DateTime.Now.Year - Student.Student_DOB
        //                };
        //    var Result = query.ToList();
        //    var json = new JavaScriptSerializer().Serialize(Result);
        //    return json;

        //}


        //Overall Score for year desc by User Score & thenby YearName
        public ActionResult OverallScoreforYearThenBy(string YearName)
        {
            var query = from SR in db.tbl_Student_Report
                        join SSM in db.tbl_Semister_Subject_Map on SR.Sem_Subject_Id equals SSM.Sem_Subject_Id
                        join SM in db.tbl_Semister_Master on SSM.Semister_Id equals SM.Semister_Id
                        join YM in db.tbl_Year_Master on SM.Year_Id equals YM.Year_Id
                        join Student in db.tbl_Student_Master on SR.Student_Id equals Student.Student_Id
                        where YM.Year_Name == YearName                         
                        select new StudentReportViewModel
                        {
                            Student_Name=Student.Student_Name,
                            User_Score=(int)SR.User_Score,
                            Max_Score=(int)SSM.Max_Score,
                            Year_Name=YM.Year_Name,
                            Semister_Name=SM.Semister_Name
                        };
            var Result = query.OrderBy(u=>u.User_Score).ThenBy(s=>s.Semister_Name);
            //var json = new JavaScriptSerializer().Serialize(Result);
            return View("OverallScoreforYearThenBy", Result);
        }


        //Percentage of All Students yearwise
        public ActionResult StudentPercentage()
        {
            var query = from SR in db.tbl_Student_Report
                        join Student in db.tbl_Student_Master on SR.Student_Id equals Student.Student_Id
                        join SSM in db.tbl_Semister_Subject_Map on SR.Sem_Subject_Id equals SSM.Sem_Subject_Id
                        join Sem in db.tbl_Semister_Master on SSM.Semister_Id equals Sem.Semister_Id
                        join Year in db.tbl_Year_Master on Sem.Year_Id equals Year.Year_Id
                        group new { SR, SSM } by new
                        {
                            Student.Student_Name,
                            Year.Year_Name
                        } into X
                        select new StudentReportViewModel
                        {
                            Student_Name = X.Key.Student_Name,
                            Year_Name = X.Key.Year_Name,
                            Obtained = (int)X.Sum(u => u.SR.User_Score),
                            Total = (int)X.Sum(v => v.SSM.Max_Score),
                            Percentage = (double)((X.Sum(u => u.SR.User_Score)) * 100.0 / X.Sum(v => v.SSM.Max_Score))
                        };

            var Result = query.ToList();
            //var json = new JavaScriptSerializer().Serialize(Result);
            return View("StudentPercentage", Result);
        }


        //Students Percentage overall
        public ActionResult OverallPercentage()
        {
            var query = from SR in db.tbl_Student_Report
                        join Student in db.tbl_Student_Master on SR.Student_Id equals Student.Student_Id
                        join SSM in db.tbl_Semister_Subject_Map on SR.Sem_Subject_Id equals SSM.Sem_Subject_Id
                        group new { SR, SSM } by new
                        {
                            Student.Student_Name                           
                        } into X
                        select new StudentReportViewModel
                        {
                            Student_Name = X.Key.Student_Name,                            
                            Percentage = (double)((X.Sum(u => u.SR.User_Score)) * 100.0 / X.Sum(v => v.SSM.Max_Score))
                        };

            var Result = query.OrderByDescending(p=>p.Percentage);
            //var json = new JavaScriptSerializer().Serialize(Result);
            return View("OverallPercentage", Result);
        }

        //Using Distinct 111
        public string DistinctNames()
        {
            var query = (from SR in db.tbl_Student_Report
                        join Student in db.tbl_Student_Master on SR.Student_Id equals Student.Student_Id
                        select new
                        {
                            SR.Student_Id,
                            Student.Student_Name
                        }).Distinct();

            var Result = query.ToList();
            var json = new JavaScriptSerializer().Serialize(Result);
            return json;
        }

        //Using Distinct
        public string DistinctName()
        {
            var query = (from SR in db.tbl_Student_Report
                         join Student in db.tbl_Student_Master on SR.Student_Id equals Student.Student_Id
                         select new
                         {
                             SR.Student_Id,
                             Student.Student_Name
                         }).Distinct();

            var Result = query.ToList();
            var json = new JavaScriptSerializer().Serialize(Result);
            return json;
        }


        //Skip
        public string SkipRecords()
        {
            var query = (from SR in db.tbl_Student_Report
                         join Student in db.tbl_Student_Master on SR.Student_Id equals Student.Student_Id
                         orderby SR.Report_Id
                         select new
                         {
                             SR.Report_Id,
                             Student.Student_Name
                         }).Skip(10);

            var Result = query.ToList();
            var json = new JavaScriptSerializer().Serialize(Result);
            return json;
        }


        //Subject wise Failed Student
        public ActionResult FailedStudent()
        {
            var query = from SR in db.tbl_Student_Report
                        join SSM in db.tbl_Semister_Subject_Map on SR.Sem_Subject_Id equals SSM.Sem_Subject_Id
                        join Sub in db.tbl_Subject_Master on SSM.Subject_Id equals Sub.Subject_Id
                        where SR.User_Score < 32
                        group SR by Sub.Subject_Name into Sub
                        select new StudentReportViewModel
                        {
                            Subject_Name = Sub.Key,
                            Count = Sub.Count()
                        };
            var Result = query.ToList();
            //var json = new JavaScriptSerializer().Serialize(Result);
            return View("FailedStudent", Result);
        }


        //Lowest scoring students Semester wise
        public ActionResult LowestScorerSemwise()
        {
            var query = (from SR in db.tbl_Student_Report
                         join Student in db.tbl_Student_Master on SR.Student_Id equals Student.Student_Id
                         join SSM in db.tbl_Semister_Subject_Map on SR.Sem_Subject_Id equals SSM.Sem_Subject_Id
                         join Sub in db.tbl_Subject_Master on SSM.Subject_Id equals Sub.Subject_Id
                         join Sem in db.tbl_Semister_Master on SSM.Semister_Id equals Sem.Semister_Id
                         join Year in db.tbl_Year_Master on Sem.Year_Id equals Year.Year_Id
                         select new
                         {
                             SR.Student_Id,
                             Student.Student_Name,
                             Sem.Semister_Name,
                             Sub.Subject_Name,
                             SR.User_Score,
                             Year.Year_Name
                         })
                       .ToList()
                       .GroupBy(s => s.Semister_Name)
                       .SelectMany(s => s.OrderBy(u => u.User_Score)
                       .Select((g, Index) =>
                       new StudentReportViewModel
                       {
                           Student_Id=g.Student_Id,
                           Student_Name=g.Student_Name,
                           Semister_Name=g.Semister_Name,
                           Subject_Name=g.Subject_Name,
                           User_Score=(int)g.User_Score,
                           Year_Name=g.Year_Name,
                           Sem = Index + 1
                       }
                       ))
                       .Where(s => s.Sem == 1);

            var Result = query.ToList();
            //var json = new JavaScriptSerializer().Serialize(Result);
            return View("LowestScorerSemwise", Result);

        }


        
        //Age Calculation 
        public ActionResult AgeCalculation()
        {
            var query = from Student in db.tbl_Student_Master
                        select new
                        {
                            StudentName = Student.Student_Name,
                            StudentDOB = Student.Student_DOB,
                        };
            var result = query.ToList();
            var newResult = result.Select(r =>
                                    new StudentReportViewModel
                                    {
                                        Student_Name=r.StudentName,
                                        Age = CalculateAge(r.StudentDOB.Value)
                                    });
            //var json = new JavaScriptSerializer().Serialize(newResult);
            return View("AgeCalculation", newResult);
        }
        private int CalculateAge(DateTime studentDOB)
        {
            DateTime today = DateTime.Today;
            int age = today.Year - studentDOB.Year;
            if (studentDOB > today.AddYears(-age))
                age--;
            return age;
        }




        //Lowest Scoring  Excluding Failed Students
        public ActionResult LowestScorerswithoutFailed()
        {
            var query = (from SR in db.tbl_Student_Report
                         join Student in db.tbl_Student_Master on SR.Student_Id equals Student.Student_Id
                         join SSM in db.tbl_Semister_Subject_Map on SR.Sem_Subject_Id equals SSM.Sem_Subject_Id
                         join Sub in db.tbl_Subject_Master on SSM.Subject_Id equals Sub.Subject_Id
                         join Sem in db.tbl_Semister_Master on SSM.Semister_Id equals Sem.Semister_Id
                         join Year in db.tbl_Year_Master on Sem.Year_Id equals Year.Year_Id
                         where SR.User_Score > 32
                         select new
                         {
                             Student.Student_Name,
                             Sem.Semister_Name,
                             Sub.Subject_Name,
                             SR.User_Score,
                             Year.Year_Name
                         })
                        .ToList()
                        .GroupBy(s => s.Semister_Name)
                        .SelectMany(s => s.OrderBy(u => u.User_Score)
                        .Select((g, index) =>
                        new StudentReportViewModel
                        {
                            Student_Name=g.Student_Name,
                            Semister_Name=g.Semister_Name,
                            Subject_Name=g.Subject_Name,
                            User_Score=(int)g.User_Score,
                            Year_Name=g.Year_Name,
                            Sem = index + 1
                        }
                        ))
                        .Where(s => s.Sem == 1);

            var Result = query.ToList();
            //var json = new JavaScriptSerializer().Serialize(Result);
            return View("LowestScorerswithoutFailed", Result);

        }


        //Top Subject of Student with more selects
        public ActionResult StudentTopSubYear(string StudentName, string YearName)
        {
            var query = from SR in db.tbl_Student_Report
                        join Student in db.tbl_Student_Master on SR.Student_Id equals Student.Student_Id
                        join SSM in db.tbl_Semister_Subject_Map on SR.Sem_Subject_Id equals SSM.Sem_Subject_Id
                        join Sub in db.tbl_Subject_Master on SSM.Subject_Id equals Sub.Subject_Id
                        join SM in db.tbl_Semister_Master on SSM.Semister_Id equals SM.Semister_Id
                        join YM in db.tbl_Year_Master on SM.Year_Id equals YM.Year_Id
                        where Student.Student_Name == StudentName && YM.Year_Name == YearName
                        group SR by Student.Student_Name into StudentGroup
                        select new StudentReportViewModel
                        {
                            Student_Name = StudentGroup.Key,
                            Year= YearName,
                            Student_Mobile= StudentGroup.FirstOrDefault().tbl_Student_Master.Student_Mobile,
                            Subject_Name = StudentGroup.FirstOrDefault().tbl_Semister_Subject_Map.tbl_Subject_Master.Subject_Name,
                            User_Score = (int)(StudentGroup.Max(x => x.User_Score))
                        };
            var Result = query.ToList();
            //var json = new JavaScriptSerializer().Serialize(Result); 
            return View("StudentTopSubYear", Result);
        }


        //Reports Data
        public string ReportsData()
        {
            db.Configuration.ProxyCreationEnabled = false;
            var query = from SR in db.tbl_Student_Report
                        join Student in db.tbl_Student_Master on SR.Student_Id equals Student.Student_Id
                        select new
                        {
                            SR.Report_Id,
                            Student
                            //here DOB comes in inix timestamp
                        };
            var Result = query.ToList();
            var json = new JavaScriptSerializer().Serialize(Result);
            return json;
        }
        


        //Reports data with Corrected DOB
        public ActionResult ReportsDatawithDOB()
        {
            db.Configuration.ProxyCreationEnabled = false;
            var query = from SR in db.tbl_Student_Report
                        join Student in db.tbl_Student_Master on SR.Student_Id equals Student.Student_Id
                        group new { SR, Student} by Student.Student_Id into StudentGroup
                        select new
                        {
                            Report_Id= StudentGroup.Select(u=>u.SR.Report_Id).FirstOrDefault(),
                            Student_Name= StudentGroup.Select(u=>u.Student.Student_Name).FirstOrDefault(),
                            Student_DOB= StudentGroup.Select(u=>u.Student.Student_DOB).FirstOrDefault()
                            
                        };
            var Result = query.ToList();

            var NewResult = Result.Select(r =>
                                            new StudentReportViewModel
                                            {
                                                Report_Id=r.Report_Id,
                                                Student_Name=r.Student_Name,
                                                //DOB= r.Student_DOB.HasValue ? DateTimeOffset.FromUnixTimeMilliseconds(r.Student_DOB.Value.Ticks/TimeSpan.TicksPerMillisecond).Date.ToString("yyyy-MM-dd") : string.Empty
                                                DOB = r.Student_DOB != null ? r.Student_DOB.Value.ToString("yyyy-MM-dd") : string.Empty
                                            });

            //var json = new JavaScriptSerializer().Serialize(NewResult);
            return View("ReportsDatawithDOB", NewResult);
        }


        //Studentwise Report Count
        public ActionResult ReportCount()
        {
            var query = from SR in db.tbl_Student_Report
                        join Student in db.tbl_Student_Master on SR.Student_Id equals Student.Student_Id
                        group SR by Student.Student_Name into Stud
                        select new StudentReportViewModel
                        {
                            Student_Name = Stud.Key,
                            Count = Stud.Count()
                        };
            var Result = query.ToList();
            //var json = new JavaScriptSerializer().Serialize(Result);
            return View("ReportCount", Result);
        }


        //Top subjects with more selects
        public ActionResult TopSubwithMoreSelect(string StudentName, string YearName)
        {
            var query = from SR in db.tbl_Student_Report
                        join Student in db.tbl_Student_Master on SR.Student_Id equals Student.Student_Id
                        join SSM in db.tbl_Semister_Subject_Map on SR.Sem_Subject_Id equals SSM.Sem_Subject_Id
                        join SM in db.tbl_Semister_Master on SSM.Semister_Id equals SM.Semister_Id
                        join YM in db.tbl_Year_Master on SM.Year_Id equals YM.Year_Id
                        join Sub in db.tbl_Subject_Master on SSM.Subject_Id equals Sub.Subject_Id
                        where Student.Student_Name == StudentName && YM.Year_Name == YearName
                        group SR by Student.Student_Name into Stud
                        select new StudentReportViewModel
                        {
                            Student_Name = Stud.Key,
                            //StudentEmail = Stud.FirstOrDefault().tbl_Student_Master.Student_Email,
                            Subject_Name = Stud.FirstOrDefault().tbl_Semister_Subject_Map.tbl_Subject_Master.Subject_Name,
                            User_Score = (int)Stud.Max(u => u.User_Score),
                            Total = (int)Stud.FirstOrDefault().tbl_Semister_Subject_Map.Max_Score
                        };
            var Result = query.ToList();
            //var json = new JavaScriptSerializer().Serialize(Result);
            return View("TopSubwithMoreSelect", Result);

        }


        //Semister wise data sorting
        public ActionResult SemwiseDataSort()
        {
            var query = from S in db.tbl_Student_Report
                        join Stud in db.tbl_Student_Master on S.Student_Id equals Stud.Student_Id
                        join SSM in db.tbl_Semister_Subject_Map on S.Sem_Subject_Id equals SSM.Sem_Subject_Id
                        join Sem in db.tbl_Semister_Master on SSM.Semister_Id equals Sem.Semister_Id
                        join Sub in db.tbl_Subject_Master on SSM.Subject_Id equals Sub.Subject_Id
                        join Year in db.tbl_Year_Master on Sem.Year_Id equals Year.Year_Id
                        //group S by Sub.Subject_Name into Subjects
                        orderby Sem.Semister_Name, Sub.Subject_Id, Sub.Subject_Name
                        select new StudentReportViewModel
                        {
                            Student_Name = Stud.Student_Name,
                            Semister_Name = Sem.Semister_Name,
                            Subject_Name = Sub.Subject_Name,
                            User_Score = (int)S.User_Score,
                            Max_Score = (int)SSM.Max_Score
                        };
            var Result = query.ToList();
            //var json = new JavaScriptSerializer().Serialize(Result);
            return View("SemwiseDataSort", Result);

        }

        //Trial
        //public string Trial()
        //{
        //    bool query = (from S in db.tbl_Student_Master
        //                 where S.Student_Name == "Darshan"
        //                 select new
        //                 {
        //                     S
        //                 }).All();

        //}



        //top 3 from Particular Subject
        public ActionResult TopThree(string SubjectName)
        {
            var query = (from SR in db.tbl_Student_Report
                         join Stud in db.tbl_Student_Master on SR.Student_Id equals Stud.Student_Id
                         join SSM in db.tbl_Semister_Subject_Map on SR.Sem_Subject_Id equals SSM.Sem_Subject_Id
                         join Sub in db.tbl_Subject_Master on SSM.Subject_Id equals Sub.Subject_Id
                         join Sem in db.tbl_Semister_Master on SSM.Semister_Id equals Sem.Semister_Id
                         join Year in db.tbl_Year_Master on Sem.Year_Id equals Year.Year_Id
                         where Sub.Subject_Name == SubjectName
                         orderby SR.User_Score descending
                         select new StudentReportViewModel
                         {
                             Report_Id=SR.Report_Id,
                             Student_Name=Stud.Student_Name,
                             User_Score=(int)SR.User_Score
                         }).Take(3);

            var Result = query.ToList();
            //var json = new JavaScriptSerializer().Serialize(Result);
            return View("TopThree", Result);

        }

        //Particular Students top Subjects
        public ActionResult StudTopSub(string StudentName)
        {
            var query = (from SR in db.tbl_Student_Report
                         join Stud in db.tbl_Student_Master on SR.Student_Id equals Stud.Student_Id
                         join SSM in db.tbl_Semister_Subject_Map on SR.Sem_Subject_Id equals SSM.Sem_Subject_Id
                         join Sub in db.tbl_Subject_Master on SSM.Subject_Id equals Sub.Subject_Id
                         join Sem in db.tbl_Semister_Master on SSM.Semister_Id equals Sem.Semister_Id
                         join Year in db.tbl_Year_Master on Sem.Year_Id equals Year.Year_Id
                         where Stud.Student_Name == StudentName
                         group SR by Sem.Semister_Name into S
                         select new StudentReportViewModel
                         {
                             Student_Name = S.FirstOrDefault().tbl_Student_Master.Student_Name,
                             Semister_Name = S.Key,
                             Subject_Name= S.FirstOrDefault().tbl_Semister_Subject_Map.tbl_Subject_Master.Subject_Name,
                             User_Score = (int)S.Max(m => m.User_Score)
                         });

            var Result = query.ToList();
            //var json = new JavaScriptSerializer().Serialize(Result);
            return View("StudTopSub", Result);


        }


        //All Students Top subject Semister wise
        public ActionResult AllStudTopSub()
        {
            var query = (from SR in db.tbl_Student_Report
                         join Stud in db.tbl_Student_Master on SR.Student_Id equals Stud.Student_Id
                         join SSM in db.tbl_Semister_Subject_Map on SR.Sem_Subject_Id equals SSM.Sem_Subject_Id
                         join Sub in db.tbl_Subject_Master on SSM.Subject_Id equals Sub.Subject_Id
                         join Sem in db.tbl_Semister_Master on SSM.Semister_Id equals Sem.Semister_Id
                         join Year in db.tbl_Year_Master on Sem.Year_Id equals Year.Year_Id
                         select new
                         {
                             Stud.Student_Name,
                             Sem.Semister_Name,
                             Sub.Subject_Name,
                             SR.User_Score
                         })
                         .ToList()
                         .GroupBy(s => new { s.Student_Name, s.Semister_Name })
                         .SelectMany(s => s.OrderByDescending(u => u.User_Score)
                                             .Select((x, index) =>
                                            new StudentReportViewModel
                                            {
                                                Student_Name=x.Student_Name,
                                                Semister_Name=x.Semister_Name,
                                                Subject_Name = x.Subject_Name,
                                                User_Score=(int)x.User_Score,
                                                Sem = index + 1
                                            }))
                            .Where(x => x.Sem == 1);

            var Result = query.ToList();
            //var json = new JavaScriptSerializer().Serialize(Result);
            return View("AllStudTopSub", Result);


        }



        //All() Function
        public string AllFunction(string StudentName)
        {
            var query = (from SR in db.tbl_Student_Report
                         join Stud in db.tbl_Student_Master on SR.Student_Id equals Stud.Student_Id
                         join SSM in db.tbl_Semister_Subject_Map on SR.Sem_Subject_Id equals SSM.Sem_Subject_Id
                         join Sub in db.tbl_Subject_Master on SSM.Subject_Id equals Sub.Subject_Id
                         join Sem in db.tbl_Semister_Master on SSM.Semister_Id equals Sem.Semister_Id
                         join Year in db.tbl_Year_Master on Sem.Year_Id equals Year.Year_Id
                         select new
                         {
                             Stud.Student_Id,
                             Stud.Student_Name
                         }).All(u => u.Student_Name == StudentName);

            var Result = query;
            var json = new JavaScriptSerializer().Serialize(Result);
            return json;
        }


        //Particular Subject Result Passed or Failed
        public ActionResult SubjectResult(int? StudentId)
        {
            var query = from SR in db.tbl_Student_Report
                        join Stud in db.tbl_Student_Master on SR.Student_Id equals Stud.Student_Id
                        join SSM in db.tbl_Semister_Subject_Map on SR.Sem_Subject_Id equals SSM.Sem_Subject_Id
                        join Sub in db.tbl_Subject_Master on SSM.Subject_Id equals Sub.Subject_Id
                        join Sem in db.tbl_Semister_Master on SSM.Semister_Id equals Sem.Semister_Id
                        join Year in db.tbl_Year_Master on Sem.Year_Id equals Year.Year_Id
                        where Stud.Student_Id == StudentId
                        //&& Year.Year_Name=="First Year"
                        select new StudentReportViewModel
                        {
                            Subject_Name=Sub.Subject_Name,
                            User_Score=(int)SR.User_Score,
                            Max_Score=(int)SSM.Max_Score,
                            Percentage = (int)(SR.User_Score * 100.0 / SSM.Max_Score),
                            Result = (SR.User_Score * 100.0 / SSM.Max_Score) > 40 ? "Passed" : "Failed"
                        };

            var Result = query;
            //var json = new JavaScriptSerializer().Serialize(Result);
            return View("SubjectResult", Result);
        }


        //Result Passed or Failed by Semister
        public ActionResult SemisterResult(string StudentName)
        {
            var query = from SR in db.tbl_Student_Report
                        join Student in db.tbl_Student_Master on SR.Student_Id equals Student.Student_Id
                        join SSM in db.tbl_Semister_Subject_Map on SR.Sem_Subject_Id equals SSM.Sem_Subject_Id
                        join Sem in db.tbl_Semister_Master on SSM.Semister_Id equals Sem.Semister_Id
                        where Student.Student_Name == StudentName
                        group new { SR, SSM } by new
                        {
                            Student.Student_Name,
                            Sem.Semister_Name
                        } into X
                        select new StudentReportViewModel
                        {
                            Student_Name = X.Key.Student_Name,
                            Semister_Name = X.Key.Semister_Name,
                            Obtained = (int)X.Sum(u => u.SR.User_Score),
                            Total = (int)X.Sum(v => v.SSM.Max_Score),
                            Percentage = (double)((X.Sum(u => u.SR.User_Score)) * 100.0 / X.Sum(v => v.SSM.Max_Score)),
                            Result = ((X.Sum(u => u.SR.User_Score)) * 100.0 / X.Sum(v => v.SSM.Max_Score)) > 40 ? "Passed" : "Failed"
                        };

            var Result = query;
            //var json = new JavaScriptSerializer().Serialize(Result);
            return View("SemisterResult", Result);
        }


        //Grouping by User Score
        public ActionResult UserScoreGroup()
        {
            var query = from SR in db.tbl_Student_Report
                        join Stud in db.tbl_Student_Master on SR.Student_Id equals Stud.Student_Id
                        join SSM in db.tbl_Semister_Subject_Map on SR.Sem_Subject_Id equals SSM.Sem_Subject_Id
                        join Sub in db.tbl_Subject_Master on SSM.Subject_Id equals Sub.Subject_Id
                        join Sem in db.tbl_Semister_Master on SSM.Semister_Id equals Sem.Semister_Id
                        join Year in db.tbl_Year_Master on Sem.Year_Id equals Year.Year_Id
                        group new
                        {
                            Stud.Student_Name,
                            Sem.Semister_Name,
                            Sub.Subject_Name,
                            score = SR.User_Score,
                        }
                        by SR.User_Score into Score
                        orderby Score.Key
                        select Score;

            var Result = query;
            //var json = new JavaScriptSerializer().Serialize(Result);
            return View(Result);
        }


        //Nested Queries
        public string NestedQuery()
        {
            //var query = from Stud in db.tbl_Student_Master
            //            group Stud by Stud.Student_Name into Student
            //            from Student2 in 
            //            (
            //                from Stud in Student
            //                group Stud by Stud.Student_Id
            //            )
            //            group Student2.Key by Student.Key;



            var query = from SR in db.tbl_Student_Report
                        join Stud in db.tbl_Student_Master on SR.Student_Id equals Stud.Student_Id
                        group SR by Stud.Student_Name into Student
                        from Student2 in (
                       from Stud in Student
                       group Stud by Stud.Student_Id
                       )
                        group Student2.Key by Student.Key;

            var Result = query.ToList();
            var json = new JavaScriptSerializer().Serialize(Result);
            return json;

        }


        //Subquery inside Group
        public string SubQueryinGroupforMin()
        {
            var query = from SR in db.tbl_Student_Report
                        group SR by SR.Student_Id into Report
                        select new
                        {
                            Id = Report.Key,
                            Score = (
                                    from Re in Report
                                    select Re.User_Score
                                ).Min()
                        };

            var Result = query.ToList();
            var json = new JavaScriptSerializer().Serialize(Result);
            return json;
        }



        //ordering by scores
        public ActionResult OrderingbyScore()
        {
            var query = from SR in db.tbl_Student_Report
                        join Student in db.tbl_Student_Master on SR.Student_Id equals Student.Student_Id
                        join SSM in db.tbl_Semister_Subject_Map on SR.Sem_Subject_Id equals SSM.Sem_Subject_Id
                        join Sem in db.tbl_Semister_Master on SSM.Semister_Id equals Sem.Semister_Id
                        join Sub in db.tbl_Subject_Master on SSM.Subject_Id equals Sub.Subject_Id
                        join Year in db.tbl_Year_Master on Sem.Year_Id equals Year.Year_Id
                        orderby Sub.Subject_Name, Student.Student_Name
                        where SR.User_Score > 32 && SR.User_Score < 60
                        select new StudentReportViewModel
                        {
                            Student_Name=Student.Student_Name,
                            Subject_Name=Sub.Subject_Name,
                            User_Score=(int)SR.User_Score
                        };
            var Result = query.ToList();
            //var json = new JavaScriptSerializer().Serialize(Result);
            return View("OrderingbyScore", Result);

        }

        //Counting Number of Reports in Semister
        public ActionResult CountingReports()
        {
            var query = from SR in db.tbl_Student_Report
                        join Student in db.tbl_Student_Master on SR.Student_Id equals Student.Student_Id
                        join SSM in db.tbl_Semister_Subject_Map on SR.Sem_Subject_Id equals SSM.Sem_Subject_Id
                        join Sem in db.tbl_Semister_Master on SSM.Semister_Id equals Sem.Semister_Id
                        join Sub in db.tbl_Subject_Master on SSM.Subject_Id equals Sub.Subject_Id
                        join Year in db.tbl_Year_Master on Sem.Year_Id equals Year.Year_Id
                        group SR by Sem.Semister_Name into Semister
                        orderby Semister.Key
                        select new StudentReportViewModel
                        {
                            Semister_Name = Semister.Key,
                            Count = Semister.Count()
                        };
            var Result = query.ToList();
            //var json = new JavaScriptSerializer().Serialize(Result);
            return View("CountingReports", Result);

        }

        //Couning passed students in subject
        public ActionResult PassedCount()

        {
            var query = from SR in db.tbl_Student_Report
                        join Stud in db.tbl_Student_Master on SR.Student_Id equals Stud.Student_Id
                        join SSM in db.tbl_Semister_Subject_Map on SR.Sem_Subject_Id equals SSM.Sem_Subject_Id
                        join Sub in db.tbl_Subject_Master on SSM.Subject_Id equals Sub.Subject_Id
                        join Sem in db.tbl_Semister_Master on SSM.Semister_Id equals Sem.Semister_Id
                        where SR.User_Score >= 0.4 * SR.User_Score
                        orderby Sem.Semister_Name
                        group SR by Sub.Subject_Name into S
                        select new StudentReportViewModel
                        {
                            Semister_Name = S.FirstOrDefault().tbl_Semister_Subject_Map.tbl_Semister_Master.Semister_Name,
                            Subject_Name = S.Key,
                            Count = S.Count()
                        };
            var Result = query.ToList();
            //var json = new JavaScriptSerializer().Serialize(Result);
            return View("PassedCount", Result);


        }


        //By using Let keyword
        public string ScorebyLet(string StudentName)
        {
            var query = from SR in db.tbl_Student_Report
                        join Student in db.tbl_Student_Master on SR.Student_Id equals Student.Student_Id
                        join SSM in db.tbl_Semister_Subject_Map on SR.Sem_Subject_Id equals SSM.Sem_Subject_Id
                        join Sem in db.tbl_Semister_Master on SSM.Semister_Id equals Sem.Semister_Id
                        join Sub in db.tbl_Subject_Master on SSM.Subject_Id equals Sub.Subject_Id
                        join Year in db.tbl_Year_Master on Sem.Year_Id equals Year.Year_Id
                        let Per = SR.User_Score * 100 / SSM.Max_Score
                        let Status = (SR.User_Score * 100 / SSM.Max_Score) > 40 ? "Passed" : "Failed"
                        where Student.Student_Name == StudentName
                        select new
                        {
                            Student.Student_Name,
                            Marks = Per,
                            Result = Status
                        };

            var Result = query.ToList();
            var json = new JavaScriptSerializer().Serialize(Result);
            return json;

        }


        //Subquery inside group for max
        public string SubqueryInsideGroupforMax()
        {
            var query = from SR in db.tbl_Student_Report
                        group SR by SR.Student_Id into Stud
                        select new
                        {
                            Id = Stud.Key,
                            Name = Stud.Where(u => u.User_Score == Stud.Max(s => s.User_Score))
                                        .Select(s => s.tbl_Student_Master.Student_Name)
                                        .FirstOrDefault(),
                            Sub = Stud.Where(u => u.User_Score == Stud.Max(s => s.User_Score))
                                        .Select(s => s.tbl_Semister_Subject_Map.tbl_Subject_Master.Subject_Name)
                                        .FirstOrDefault(),

                            Score = (
                                    from S in Stud
                                    select S.User_Score
                                    ).Max()
                        };
            var Result = query.ToList();
            var json = new JavaScriptSerializer().Serialize(Result);
            return json;
        }


        //MinandMaxScore
        public ActionResult MinandMaxScore()
        {
            var query = from SR in db.tbl_Student_Report
                        join Student in db.tbl_Student_Master on SR.Student_Id equals Student.Student_Id
                        join SSM in db.tbl_Semister_Subject_Map on SR.Sem_Subject_Id equals SSM.Sem_Subject_Id
                        join Sem in db.tbl_Semister_Master on SSM.Semister_Id equals Sem.Semister_Id
                        join Year in db.tbl_Year_Master on Sem.Year_Id equals Year.Year_Id
                        group SR by Student.Student_Name into Stud
                        select new StudentReportViewModel
                        {
                            Student_Name = Stud.Key,

                            Maximum_Score = (int)Stud.Max(u => u.User_Score),
                            YearMaxScore = Stud.Where(u => u.User_Score == Stud.Max(s => s.User_Score))
                                            .Select(s => s.tbl_Semister_Subject_Map.tbl_Semister_Master.tbl_Year_Master.Year_Name)
                                            .FirstOrDefault(),
                            MaxScoreSubject= Stud.Where(u=>u.User_Score==Stud.Max(s=>s.User_Score))
                                            .Select(s=>s.tbl_Semister_Subject_Map.tbl_Subject_Master.Subject_Name)
                                            .FirstOrDefault(),

                            Minimum_Score = (int)Stud.Min(u => u.User_Score),
                            YearMinScore = Stud.Where(u => u.User_Score == Stud.Min(s => s.User_Score))
                                            .Select(s => s.tbl_Semister_Subject_Map.tbl_Semister_Master.tbl_Year_Master.Year_Name)
                                            .FirstOrDefault(),
                            MinScoreSubject= Stud.Where(u => u.User_Score == Stud.Min(s => s.User_Score))
                                            .Select(s => s.tbl_Semister_Subject_Map.tbl_Subject_Master.Subject_Name)
                                            .FirstOrDefault(),
                        };
            var Result = query.ToList();
            //var json = new JavaScriptSerializer().Serialize(Result);
            return View("MinandMaxScore", Result);
        }


        //PassedandfailedCount
        public string PassedFailedCount()
        {
            var query = from SR in db.tbl_Student_Report
                        join Student in db.tbl_Student_Master on SR.Student_Id equals Student.Student_Id
                        join SSM in db.tbl_Semister_Subject_Map on SR.Sem_Subject_Id equals SSM.Sem_Subject_Id
                        join Sem in db.tbl_Semister_Master on SSM.Semister_Id equals Sem.Semister_Id
                        join Sub in db.tbl_Subject_Master on SSM.Subject_Id equals Sub.Subject_Id
                        group SR by Sub.Subject_Name into Subject
                        select new
                        {
                            Name = Subject.Key,
                            FailedCount = Subject.Count(u => u.User_Score < 32),
                            PassedCount = Subject.Count(u => u.User_Score > 32)
                        };
            var Result = query.ToList();
            var json = new JavaScriptSerializer().Serialize(Result);
            return json;
        }


        //Let keyword
        public string DatabyLet()
        {
            var query = from SR in db.tbl_Student_Report
                        join Student in db.tbl_Student_Master on SR.Student_Id equals Student.Student_Id
                        join SSM in db.tbl_Semister_Subject_Map on SR.Sem_Subject_Id equals SSM.Sem_Subject_Id
                        join Sub in db.tbl_Subject_Master on SSM.Subject_Id equals Sub.Subject_Id
                        join Sem in db.tbl_Semister_Master on SSM.Semister_Id equals Sem.Semister_Id
                        let Diff = SSM.Max_Score - SR.User_Score
                        select new
                        {
                            Student.Student_Name,
                            Sub.Subject_Name,
                            SSM.Max_Score,
                            SR.User_Score,
                            Diff,
                            PerDiff = Diff * 100 / SSM.Max_Score
                        };
            var Result = query.ToList();
            var json = new JavaScriptSerializer().Serialize(Result);
            return json;
        }


        //Subjectwise Lowest Scorer
        public string SubLowestScorer()
        {
            var query = (from SR in db.tbl_Student_Report
                         join Student in db.tbl_Student_Master on SR.Student_Id equals Student.Student_Id
                         join SSM in db.tbl_Semister_Subject_Map on SR.Sem_Subject_Id equals SSM.Sem_Subject_Id
                         join Subject in db.tbl_Subject_Master on SSM.Subject_Id equals Subject.Subject_Id
                         join Semister in db.tbl_Semister_Master on SSM.Semister_Id equals Semister.Semister_Id
                         join Year in db.tbl_Year_Master on Semister.Year_Id equals Year.Year_Id
                         select new
                         {
                             SR.Report_Id,
                             Student.Student_Name,
                             Subject.Subject_Name,
                             SR.User_Score
                         })
                        .ToList()
                        .GroupBy(s => s.Subject_Name)
                        .SelectMany(u => u.OrderBy(s => s.User_Score)
                                            .Select((g, index) =>
                                            new
                                            {
                                                g.Report_Id,
                                                g.Student_Name,
                                                g.Subject_Name,
                                                g.User_Score,
                                                Sub = index + 1
                                            }))
                        .Where(u => u.Sub == 1);

            var Result = query.ToList();
            var json = new JavaScriptSerializer().Serialize(Result);
            return json;

        }

        //Trial
        public string Trial()
        {
            var query =
            /*from Stud in db.tbl_Student_Master
            where Stud.Student_Name.Contains("Darshan")
            select Stud;*/


            /*from S in db.tbl_Student_Master
            where S.Student_Name.Length != 6
            select S;*/


            /*from SR in db.tbl_Student_Report
            join Student in db.tbl_Student_Master on SR.Student_Id equals Student.Student_Id into StuR
            join SSM in db.tbl_Semister_Subject_Map on SR.Sem_Subject_Id equals SSM.Sem_Subject_Id
            join Sem in db.tbl_Semister_Master on SSM.Semister_Id equals Sem.Semister_Id
            join Sub in db.tbl_Subject_Master on SSM.Subject_Id equals Sub.Subject_Id
            join Year in db.tbl_Year_Master on Sem.Year_Id equals Year.Year_Id
            select new
            {
                StudentName = SR.Student_Id,
                Results = StuR
            };*/


            /*from SR in tbl_Student_Report
            join Student in tbl_Student_Master on SR.Student_Id equals Student.Student_Id into StuR
            from Res in StuR.DefaultIfEmpty()
            select new
            {
                SR.Report_Id,
                Result = Res?.User_Score ?? string.Empty
            }*/


            /*(from SR in db.tbl_Student_Report
             join Student in db.tbl_Student_Master on SR.Student_Id equals Student.Student_Id
             join SSM in db.tbl_Semister_Subject_Map on SR.Sem_Subject_Id equals SSM.Sem_Subject_Id
             join Subject in db.tbl_Subject_Master on SSM.Subject_Id equals Subject.Subject_Id
             join Semister in db.tbl_Semister_Master on SSM.Semister_Id equals Semister.Semister_Id
             join Year in db.tbl_Year_Master on Semister.Year_Id equals Year.Year_Id
             select new
             {
                 Student.Student_Name,
                 Year.Year_Name,
                 SR.User_Score,
                 Subject.Subject_Name,
                 Semister.Semister_Name
             })
            .ToList()
            .GroupBy(s => s.Year_Name)
            .SelectMany(u => u.OrderBy(s => s.User_Score)
                .Select((g, index) =>
                new
                {
                    g.Student_Name,
                    g.Year_Name,
                    g.User_Score,
                    g.Subject_Name,
                    Y = index + 1
                }
                ))
            .Where(u => u.Y == 1);*/

            (from SR in db.tbl_Student_Report
             join Stud in db.tbl_Student_Master on SR.Student_Id equals Stud.Student_Id into StudGroup
             from SG in StudGroup.DefaultIfEmpty()
             select new
             {
                 Name = StudGroup.FirstOrDefault().Student_Name,
                 Score = StudGroup.FirstOrDefault().tbl_Student_Report.FirstOrDefault().User_Score
             });


            var Result = query.ToList();
            var json = new JavaScriptSerializer().Serialize(Result);
            return json;

        }

    }

}