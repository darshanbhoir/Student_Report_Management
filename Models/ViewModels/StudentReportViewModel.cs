﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Student_Report_Management.Models.ViewModels
{
    public class StudentReportViewModel
    {
        //Semister_Master
        [Display(Name ="Semister Id" )]
        public int Semister_Id { get; set; }
        [Display(Name = "Semister Name")]
        public string Semister_Name { get; set; }

        //Semister_Subject_Map
        [Display(Name = "Semister Subject Id")]
        public int Sem_Subject_Id { get; set; }
        [Display(Name = "Maximum Score")]
        public int Max_Score { get; set; }

        //Student_MAster
        [Display(Name = "Student Id")]
        public int Student_Id { get; set; }
        [Display(Name = "Student Name")]
        public string Student_Name { get; set; }
        [Display(Name = "Student E-mail")]
        public string Student_Email { get; set; }
        [Display(Name = "Student Mobile")]
        public string Student_Mobile { get; set; }
        [Display(Name = "Student Birth Date")]
        public DateTime Student_DOB { get; set; }

        //Student_Master
        [Display(Name = "Report Id")]
        public int Report_Id { get; set; }
        [Display(Name = "User Score")]
        public int User_Score { get; set; }
        [Display(Name = "Data Updated on")]
        public DateTime Updated_On { get; set; }


        //Subject_Master
        [Display(Name = "Subject Id")]
        public int Subject_Id { get; set; }
        [Display(Name = "Subject Name")]
        public string Subject_Name { get; set; }

        //Year_Master
        [Display(Name = "Year Id")]
        public int Year_Id { get; set; }
        [Display(Name = "Year Name")]
        public string Year_Name { get; set; }



        //Annomous
        public int Years { get; set; }
        public int SubjectCount { get; set; }
        public int Total { get; set; }
        public double Average { get; set; }
        public int Sem { get; set; }
    }
}