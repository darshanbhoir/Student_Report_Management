using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Student_Report_Management.Models.ViewModels
{
    public class StudentReportModel
    {
        //Student Report
        [Key]
        public int Report_Id { get; set; }
        public string Student_Name { get; set; }
        public int Sem_Subject_Id { get; set; }
        public string Semister_Name { get; set; }
        public int Max_Score { get; set; }
        public string Subject_Name { get; set; }
        public int User_Score { get; set; }




    }
}