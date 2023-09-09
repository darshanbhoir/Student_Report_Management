using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Student_Report_Management.Models.ViewModels
{
    public class StudentReportViewModel
    {
        //Semister_Master
        public int Semister_Id { get; set; }
        public string Semister_Name { get; set; }

        //Semister_Subject_Map
        public int Sem_Subject_Id { get; set; }
        public int Max_Score { get; set; }

        //STudent_MAster
        public int Student_Id { get; set; }
        public string Student_Name { get; set; }
        public string Student_Email { get; set; }
        public string Student_Mobile { get; set; }
        public DateTime Student_DOB { get; set; }

        //Student_Master
 
        public int Report_Id { get; set; }
        public int User_Score { get; set; }
        public DateTime Updated_On { get; set; }


        //Subject_Master
        public int Subject_Id { get; set; }
        public string Subject_Name { get; set; }

        //Year_Master
        public int Year_Id { get; set; }
        public string Year_Name { get; set; }
    }
}