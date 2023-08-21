using Student_Report_Management.Models;
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
        public string Index(int StudentId, string YearName)
        {
            var query = from StudentReport in db.tbl_Student_Report
                        join StudentMaster in db.tbl_Student_Master on StudentReport.Student_Id equals StudentMaster.Student_Id
                        join SemisterSubjectMap in db.tbl_Semister_Subject_Map on StudentReport.Sem_Subject_Id equals SemisterSubjectMap.Sem_Subject_Id
                        join SemesterMaster in db.tbl_Semister_Master on SemisterSubjectMap.Semister_Id equals SemesterMaster.Semister_Id
                        join YearMaster in db.tbl_Year_Master on SemesterMaster.Year_Id equals YearMaster.Year_Id
                        join SubjectMaster in db.tbl_Subject_Master on SemisterSubjectMap.Subject_Id equals SubjectMaster.Subject_Id
                        where StudentReport.Student_Id == StudentId && YearMaster.Year_Name == YearName
                        select new 
                        {
                            StudentMaster.Student_Name,
                            SemisterSubjectMap.Sem_Subject_Id,
                            SemesterMaster.Semister_Name,
                            SemisterSubjectMap.Max_Score,
                            SubjectMaster.Subject_Name,
                            StudentReport.User_Score
                        };

            var Result = query.ToList();
            var json = new JavaScriptSerializer().Serialize(Result);
            return json;
        }


        //Semester wise Particular Student Data
        public string SemesterwiseStudent(string StudentName)
        {
            var query = from StudentReport in db.tbl_Student_Report
                        join StudentMaster in db.tbl_Student_Master on StudentReport.Student_Id equals StudentMaster.Student_Id
                        join SemesterSubjectMap in db.tbl_Semister_Subject_Map on StudentReport.Sem_Subject_Id equals SemesterSubjectMap.Sem_Subject_Id
                        join SemesterMaster in db.tbl_Semister_Master on SemesterSubjectMap.Semister_Id equals SemesterMaster.Semister_Id
                        where StudentMaster.Student_Name == StudentName
                        orderby SemesterMaster.Semister_Name
                        select new 
                        {
                           StudentReport.Report_Id,
                            StudentReport.Student_Id,
                            StudentReport.User_Score,
                            StudentMaster.Student_Name,
                            StudentMaster.Student_Mobile,
                            SemesterSubjectMap.Semister_Id,
                            SemesterMaster.Semister_Name
                        };

            var Result = query.ToList();
            var json = new JavaScriptSerializer().Serialize(Result);
            return json;
        }



        //Student Score Yearwise
        public string StudentScoreYearwise(int StudentId)
        {
            var query = from StudentReport in db.tbl_Student_Report
                        join StudentMaster in db.tbl_Student_Master on StudentReport.Student_Id equals StudentMaster.Student_Id
                        join SemisterSubjectMap in db.tbl_Semister_Subject_Map on StudentReport.Sem_Subject_Id equals SemisterSubjectMap.Sem_Subject_Id
                        join SemesterMaster in db.tbl_Semister_Master on SemisterSubjectMap.Semister_Id equals SemesterMaster.Semister_Id
                        join YearMaster in db.tbl_Year_Master on SemesterMaster.Year_Id equals YearMaster.Year_Id
                        where StudentReport.Student_Id == StudentId
                        select new 
                        {
                            StudentReport.Student_Id,
                            StudentMaster.Student_Name,
                            StudentReport.Report_Id,
                            SemesterMaster.Semister_Name,
                            YearMaster.Year_Name,
                            StudentReport.User_Score
                        };

            var Result = query.ToList();
            var json = new JavaScriptSerializer().Serialize(Result);
            return json;
        }



        //Overall Score for year desc
        public string OverallScoreforYear(string YearName)
        {
            var query = from SR in db.tbl_Student_Report
                        join SSM in db.tbl_Semister_Subject_Map on SR.Sem_Subject_Id equals SSM.Sem_Subject_Id
                        join SM in db.tbl_Semister_Master on SSM.Semister_Id equals SM.Semister_Id
                        join YM in db.tbl_Year_Master on SM.Year_Id equals YM.Year_Id
                        join Student in db.tbl_Student_Master on SR.Student_Id equals Student.Student_Id
                        where YM.Year_Name == YearName
                        orderby SR.User_Score descending
                        select new
                        {
                            Student.Student_Name,
                            SR.User_Score,
                            SSM.Max_Score,
                            YM.Year_Name,
                            SM.Semister_Name
                        };
            var Result = query.ToList();
            var json = new JavaScriptSerializer().Serialize(Result);
            return json;
        }

        //Top subject Semister wise of a particular user for a particular year.
        public string StudentMaxScoreYear(string StudentName, string YearName)
        {
            var query = from SR in db.tbl_Student_Report
                        join Student in db.tbl_Student_Master on SR.Student_Id equals Student.Student_Id
                        join SSM in db.tbl_Semister_Subject_Map on SR.Sem_Subject_Id equals SSM.Sem_Subject_Id
                        join SM in db.tbl_Semister_Master on SSM.Semister_Id equals SM.Semister_Id
                        join YM in db.tbl_Year_Master on SM.Year_Id equals YM.Year_Id
                        where Student.Student_Name == StudentName && YM.Year_Name == YearName
                        group SR by Student.Student_Name into StudentGroup
                        select new
                        {
                            StudentName = StudentGroup.Key,
                            MaxScore = StudentGroup.Max(x => x.User_Score)
                        };
            var Result = query.ToList();
            var json = new JavaScriptSerializer().Serialize(Result);
            return json;
        }



        //Top Scorer from all Students with subject details.
        public string StudentTopSubjectfromSem(string StudentName, string YearName)
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
                                           new
                                           {
                                               x.Student_Name,
                                               x.Year_Name,
                                               x.Semister_Name,
                                               x.Subject_Name,
                                               x.User_Score,
                                               Sem_Name = index + 1
                                           }))
                        .Where(x => x.Sem_Name == 1);

            var Result = query.ToList();
            var json = new JavaScriptSerializer().Serialize(Result);
            return json;
        }


        //Top Scorer from all Students with subject details
        public string TopScorer()
        {
            var query = (from SR in db.tbl_Student_Report
                         join Student in db.tbl_Student_Master on SR.Student_Id equals Student.Student_Id
                         join SSM in db.tbl_Semister_Subject_Map on SR.Sem_Subject_Id equals SSM.Sem_Subject_Id
                         join Subject in db.tbl_Subject_Master on SSM.Subject_Id equals Subject.Subject_Id
                         join Semester in db.tbl_Semister_Master on SSM.Semister_Id equals Semester.Semister_Id
                         join Year in db.tbl_Year_Master on Semester.Year_Id equals Year.Year_Id
                         orderby SR.User_Score descending
                         select new
                         {
                             SR.Student_Id,
                             Student.Student_Name,
                             Subject.Subject_Name,
                             SR.User_Score,
                             Semester.Semister_Name,
                             Year.Year_Name,
                         }).Take(1);
            var Result = query.ToList();
            var json = new JavaScriptSerializer().Serialize(Result);
            return json;
        }



        //Top Scorer Year wise
        public string TopScorerYearwise()
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
                                         new
                                         {
                                             g.Student_Id,
                                             g.Student_Name,
                                             g.Subject_Name,
                                             g.User_Score,
                                             g.Semister_Name,
                                             g.Year_Name,
                                             Year = index + 1
                                         }))
                        .Where(x => x.Year == 1);

            var Result = query.ToList();
            var json = new JavaScriptSerializer().Serialize(Result);
            return json;


        }



        //Semister wise subject data with max score
        public string SemwiseSubjectData()
        {
            var query = from SSM in db.tbl_Semister_Subject_Map
                        join Semester in db.tbl_Semister_Master on SSM.Semister_Id equals Semester.Semister_Id
                        join Subject in db.tbl_Subject_Master on SSM.Subject_Id equals Subject.Subject_Id
                        select new
                        {
                            SSM.Sem_Subject_Id,
                            Subject.Subject_Name,
                            SSM.Max_Score,
                            Semester.Semister_Name,
                        };

            var Result = query.ToList();
            var json = new JavaScriptSerializer().Serialize(Result);
            return json;

        }


        //Particular Students top subject from each Semister
        public string StudentTopSubEachSem(string StudentName)
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
                                        new
                                        {
                                            g.Student_Id,
                                            g.Student_Name,
                                            g.Semister_Name,
                                            g.User_Score,
                                            g.Year_Name,
                                            Semester = index + 1
                                        }))
                       .Where(x => x.Semester == 1);

            var Result = query.ToList();
            var json = new JavaScriptSerializer().Serialize(Result);
            return json;
        }


        //Left Outer Join
        public string LeftOuterjoin()
        {
            var query = from SR in db.tbl_Student_Report
                        join SSM in db.tbl_Semister_Subject_Map on SR.Sem_Subject_Id equals SSM.Sem_Subject_Id
                                into SSMGroup
                        from SSM in SSMGroup.DefaultIfEmpty()
                        select SR;

            var Result = query.ToList();
            var json = new JavaScriptSerializer().Serialize(Result);
            return json;

        }

    }

}