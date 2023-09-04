<Query Kind="Expression">
  <Connection>
    <ID>ea3f7c03-c61d-4dc4-aea8-01acd22e1a61</ID>
    <Persist>true</Persist>
    <Server>13.233.140.191,1433</Server>
    <SqlSecurity>true</SqlSecurity>
    <Database>db_Student_Report_Mgmt</Database>
    <NoPluralization>true</NoPluralization>
    <NoCapitalization>true</NoCapitalization>
    <UserName>sa</UserName>
    <Password>AQAAANCMnd8BFdERjHoAwE/Cl+sBAAAAG27VzC120EOqHH4iU/QWxgAAAAACAAAAAAAQZgAAAAEAACAAAADwbBfHdgkep/ljh0RE7H0+PSNdiitlZ3DiSyPEYnqGnwAAAAAOgAAAAAIAACAAAADEtKx/i/5zz5P7kZD8ZvukfbI7XiDfDeLP6igzZlwIRiAAAAAZFO4ULpZ1M4VFUGbgC+51BTNu8/SLT2X1R37iz+OthUAAAACNiJnr1rCitjpe54riknAbHAkpsQhlE5CF6xCD8pIUj+2Xh6ReM82HBC5LApW0xj+YT/M5JfE2B4P6nmGDThgB</Password>
  </Connection>
</Query>

//from SR in tbl_Student_Report
//join Student in tbl_Student_Master on SR.Student_Id equals Student.Student_Id 
//join SSM in tbl_Semister_Subject_Map on SR.Sem_Subject_Id equals SSM.Sem_Subject_Id
//join Sem in tbl_Semister_Master on SSM.Semister_Id equals Sem.Semister_Id
//join Sub in tbl_Subject_Master on SSM.Subject_Id equals Sub.Subject_Id
//join Year in tbl_Year_Master on Sem.Year_Id equals Year.Year_Id
//let Per= SR.User_Score*100/SSM.Max_Score
//let Status =(SR.User_Score*100/SSM.Max_Score)>40 ? "PAssed" : "Failed"
//where Student.Student_Name=="Darshan" 
//select new
//{
//	Student.Student_Name,
//	Sub.Subject_Name,
//	Marks= Per,
//	Result= Status
//}




//from SR in tbl_Student_Report
//join Student in tbl_Student_Master on SR.Student_Id equals Student.Student_Id into StuR
//select new
//{
//	StudentName= SR.Student_Id,
//	Result= StuR
//}



//(from SR in tbl_Student_Report
//join Student in tbl_Student_Master on SR.Student_Id equals Student.Student_Id 
//join SSM in tbl_Semister_Subject_Map on SR.Sem_Subject_Id equals SSM.Sem_Subject_Id
//join Sem in tbl_Semister_Master on SSM.Semister_Id equals Sem.Semister_Id
//join Sub in tbl_Subject_Master on SSM.Subject_Id equals Sub.Subject_Id
//join Year in tbl_Year_Master on Sem.Year_Id equals Year.Year_Id
//orderby Student.Student_Id, SR.User_Score descending, Sub.Subject_Name
//where Sem.Semister_Name=="Semester 1"
//select new
//	{
//		Student.Student_Id,
//		Student.Student_Name,
//		SR.User_Score,
//		Sem.Semister_Name,
//		Year.Year_Name,
//		Sub.Subject_Name
//	})



//from SR in tbl_Student_Report
//join Student in tbl_Student_Master on SR.Student_Id equals Student.Student_Id
//join SSM in tbl_Semister_Subject_Map on SR.Sem_Subject_Id equals SSM.Sem_Subject_Id
//join Sem in tbl_Semister_Master on SSM.Semister_Id equals Sem.Semister_Id
//join Year in tbl_Year_Master on Sem.Year_Id equals Year.Year_Id
//where SR.Student_Id == 2
//select new 
//{
//    SR.Student_Id,
//    Student.Student_Name,
//    Sem.Semister_Name,
//    Year.Year_Name,
//    SR.User_Score
//}



//(from SR in tbl_Student_Report
//join Student in tbl_Student_Master on SR.Student_Id equals Student.Student_Id
//join SSM in tbl_Semister_Subject_Map on SR.Sem_Subject_Id equals SSM.Sem_Subject_Id
//join Sem in tbl_Semister_Master on SSM.Semister_Id equals Sem.Semister_Id
//join Year in tbl_Year_Master on Sem.Year_Id equals Year.Year_Id
//join Sub in tbl_Subject_Master on SSM.Subject_Id equals Sub.Subject_Id
//select new
//{
// 	Student.Student_Name,
//	Year.Year_Name,
//	Sem.Semister_Name,
//	Sub.Subject_Name,
//	SR.User_Score
//})
//.ToList()
//.GroupBy(g => g.Semister_Name)
//                        .SelectMany(s => s.OrderByDescending(x => x.User_Score)
//                                           .Select((x, index) =>
//                                           new
//                                           {
//                                               x.Student_Name,
//                                               x.Year_Name,
//                                               x.Semister_Name,
//                                               x.Subject_Name,
//                                               x.User_Score,
//                                               Sem_Name = index + 1
//                                           }))
//                        .Where(x => x.Sem_Name == 1)



//(from SR in tbl_Student_Report
//join Student in tbl_Student_Master on SR.Student_Id equals Student.Student_Id
//join SSM in tbl_Semister_Subject_Map on SR.Sem_Subject_Id equals SSM.Sem_Subject_Id
//join Sem in tbl_Semister_Master on SSM.Semister_Id equals Sem.Semister_Id
//join Year in tbl_Year_Master on Sem.Year_Id equals Year.Year_Id
//join Sub in tbl_Subject_Master on SSM.Subject_Id equals Sub.Subject_Id
//orderby SR.User_Score descending
//where Year.Year_Name=="First Year"
//select new
//{
// 	Student.Student_Name,
//	Year.Year_Name,
//	Sem.Semister_Name,
//	Sub.Subject_Name,
//	SR.User_Score
//})
//.Take(5)



//from SR in tbl_Student_Report
//join Stud in tbl_Student_Master on SR.Student_Id equals Stud.Student_Id
//join SSM in tbl_Semister_Subject_Map on SR.Sem_Subject_Id equals SSM.Sem_Subject_Id
//join Sub in tbl_Subject_Master on SSM.Subject_Id equals Sub.Subject_Id
//join Sem in tbl_Semister_Master on SSM.Semister_Id equals Sem.Semister_Id
//where  SR.User_Score>= 0.4* SR.User_Score
//orderby Sem.Semister_Name
//group SR by Sub.Subject_Name into S
//select new
//{
//	Semister = S.FirstOrDefault().tbl_Semister_Subject_Map.tbl_Semister_Master.Semister_Name,
//	SubName= S.Key,
//	Count= S.Count()
//}


