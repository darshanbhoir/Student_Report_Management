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



//from SR in tbl_Student_Report
//join Stud in tbl_Student_Master on SR.Student_Id equals Stud.Student_Id
//join SSM in tbl_Semister_Subject_Map on SR.Sem_Subject_Id equals SSM.Sem_Subject_Id
//join Sub in tbl_Subject_Master on SSM.Subject_Id equals Sub.Subject_Id
//join Sem in tbl_Semister_Master on SSM.Semister_Id equals Sem.Semister_Id
//join Year in tbl_Year_Master on Sem.Year_Id equals Year.Year_Id
//orderby Stud.Student_Name, Sem.Semister_Name
//where SR.User_Score >= 32 && SR.User_Score <=60
//select new 
//	{
//	Stud.Student_Name,
//	Sem.Semister_Name,
//	Sub.Subject_Name,
//	SR.User_Score
//	}



//from SR in tbl_Student_Report
//group SR by SR.Student_Id into Stud
//select new
//	{
//	Id= Stud.Key,
//	Name= Stud.Where(u=>u.User_Score==Stud.Max(s=>s.User_Score))
//				.Select(s=>s.tbl_Student_Master.Student_Name)
//				.FirstOrDefault(),
//	Sub= Stud.Where(u=>u.User_Score==Stud.Max(s=>s.User_Score))
//				.Select(s=>s.tbl_Semister_Subject_Map.tbl_Subject_Master.Subject_Name)
//				.FirstOrDefault(),
//	Score= (
//			from S in Stud
//			select S.User_Score
//			).Max()
//	}


//from SR in tbl_Student_Report
//group SR by SR.Student_Id into Stud
//select new
//	{
//	Id= Stud.Key,
//	Name= Stud.Where(u=>u.User_Score==Stud.Max(s=>s.User_Score))
//				.Select(s=>s.tbl_Student_Master.Student_Name)
//				.FirstOrDefault(),
//	Semister= Stud.Where(u=>u.User_Score==Stud.Max(s=>s.User_Score))
//				.Select(s=>s.tbl_Semister_Subject_Map.tbl_Semister_Master.Semister_Name)
//				.FirstOrDefault(),
//	Sub= Stud.Where(u=>u.User_Score==Stud.Max(s=>s.User_Score))
//				.Select(s=>s.tbl_Semister_Subject_Map.tbl_Subject_Master.Subject_Name)
//				.FirstOrDefault(),
//	Score= (
//			from S in Stud
//			select S.User_Score
//			).Min()
//	}



//from SR in tbl_Student_Report
//join Student in tbl_Student_Master on SR.Student_Id equals Student.Student_Id
//join SSM in tbl_Semister_Subject_Map on SR.Sem_Subject_Id equals SSM.Sem_Subject_Id
//join Sem in tbl_Semister_Master on SSM.Semister_Id equals Sem.Semister_Id
//group SR by Student.Student_Id into Stud
//select new 
//	{
//	Name= Stud.Key,
//	Semister= Stud.FirstOrDefault().tbl_Semister_Subject_Map.tbl_Semister_Master.Semister_Name,
//	Score= Stud.Count()
//	}


//(from SR in tbl_Student_Report
//join Student in tbl_Student_Master on SR.Student_Id equals Student.Student_Id
//join SSM in tbl_Semister_Subject_Map on SR.Sem_Subject_Id equals SSM.Sem_Subject_Id
//join SM in tbl_Semister_Master on SSM.Semister_Id equals SM.Semister_Id
//join YM in tbl_Year_Master on SM.Year_Id equals YM.Year_Id
//orderby SM	.Semister_Name
//where Student.Student_Name=="Darshan"
//select new
//{	
//	SR.Student_Id,
//	Student.Student_Name,
//	SM.Semister_Name,
//	YM.Year_Name,
//	SR.User_Score
//})
//.ToList()
//.GroupBy(g=>g.Semister_Name)
//.SelectMany(s=>s.OrderByDescending(g=>g.User_Score)
//				.Select((g, index)=>
//				new
//				{
//					g.Student_Id,
//					g.Student_Name,
//					g.Semister_Name,
//					g.Year_Name,
//					g.User_Score,
//					Semester= index+1
//				}))
//.Where(x=>x.Semester==1)
//




//(from SR in tbl_Student_Report
//join Student in tbl_Student_Master on SR.Student_Id equals Student.Student_Id
//join SSM in tbl_Semister_Subject_Map on SR.Sem_Subject_Id equals SSM.Sem_Subject_Id
//join Sub in tbl_Subject_Master on SSM.Subject_Id equals Sub.Subject_Id
//join SM in tbl_Semister_Master on SSM.Semister_Id equals SM.Semister_Id
//join YM in tbl_Year_Master on SM.Year_Id equals YM.Year_Id
//orderby SM.Semister_Name
//select new
//{
//	SR.Student_Id,
//	Student.Student_Name,
//	Sub.Subject_Name,
//	SR.User_Score,
//	SM.Semister_Name
//})
//.ToList()
//.GroupBy(s=>s.Semister_Name)
//.SelectMany(s=>s.OrderBy(u=>u.User_Score)
//					.Select((u, index)=>
//					new
//					{
//						u.Student_Id,
//						u.Student_Name,
//						u.Subject_Name,
//						u.User_Score,
//						u.Semister_Name,
//						Sem=index+1
//					}))
//.Where(s=>s.Sem==1)



//(from SR in tbl_Student_Report
//join Student in tbl_Student_Master on SR.Student_Id equals Student.Student_Id
//join SSM in tbl_Semister_Subject_Map on SR.Sem_Subject_Id equals SSM.Sem_Subject_Id
//join Sub in tbl_Subject_Master on SSM.Subject_Id equals Sub.Subject_Id
//join Sem in tbl_Semister_Master on SSM.Semister_Id equals Sem.Semister_Id
//join Year in tbl_Year_Master on Sem.Year_Id equals Year.Year_Id
//orderby Sem.Semister_Name
//select new
//	{
//		SR.Student_Id,
//		Student.Student_Name,
//		Sub.Subject_Name,
//		Year.Year_Name,
//		Sem.Semister_Name,
//		SR.User_Score,
//		SSM.Max_Score
//	})
//	.ToList()
//	.GroupBy(s=>s.Semister_Name)
//	.SelectMany(s=>s.OrderByDescending(u=>u.User_Score)
//					.Select((u, index)=>
//					new
//					{
//						u.Student_Id,
//						u.Student_Name,
//						u.Subject_Name,
//						u.Semister_Name,
//						u.Year_Name,
//						u.User_Score,
//						//u.Max_Score,
//						Sem=index+1
//					}
//					))
//	.Where(s=>s.Sem==1)



//from SR in tbl_Student_Report
//join Student in tbl_Student_Master on SR.Student_Id equals Student.Student_Id
//join SSM in tbl_Semister_Subject_Map on SR.Sem_Subject_Id equals SSM.Sem_Subject_Id
//join Sem in tbl_Semister_Master on SSM.Semister_Id equals Sem.Semister_Id
//join Year in tbl_Year_Master on Sem.Year_Id equals Year.Year_Id
//where Year.Year_Name=="First Year"
//orderby Student.Student_Name, Sem.Semister_Name
//select new
//{
//	Student.Student_Name,
//	Sem.Semister_Name,
//	Year.Year_Name,
//	SR.User_Score,
//	SSM.Max_Score
//}



//from SR in tbl_Student_Report
//join Student in tbl_Student_Master on SR.Student_Id equals Student.Student_Id
//join SSM in tbl_Semister_Subject_Map on SR.Sem_Subject_Id equals SSM.Sem_Subject_Id
//join Sem in tbl_Semister_Master on SSM.Semister_Id equals Sem.Semister_Id
//join Year in tbl_Year_Master on Sem.Year_Id equals Year.Year_Id
//group SR by Student.Student_Name into Stud
//select new
//{
//	Name= Stud.Key,
//	ScoreMax= Stud.Max(u=>u.User_Score),
//	YearofMaxScore= Stud.Where(u=>u.User_Score== Stud.Max(s=>s.User_Score))
//					.Select(s=>s.tbl_Semister_Subject_Map.tbl_Semister_Master.tbl_Year_Master.Year_Name)
//					.FirstOrDefault(),
//	ScoreMin= Stud.Min(u=>u.User_Score),
//	YearofMinScore=Stud.Where(u=>u.User_Score== Stud.Min(s=>s.User_Score))
//					.Select(s=>s.tbl_Semister_Subject_Map.tbl_Semister_Master.tbl_Year_Master.Year_Name)
//					.FirstOrDefault(),
//
//}


//from SR in tbl_Student_Report
//join Student in tbl_Student_Master on SR.Student_Id equals Student.Student_Id
//join SSM in tbl_Semister_Subject_Map on SR.Sem_Subject_Id equals SSM.Sem_Subject_Id
//join Sem in tbl_Semister_Master on SSM.Semister_Id equals Sem.Semister_Id
//join Sub in tbl_Subject_Master on SSM.Subject_Id equals Sub.Subject_Id
//group SR by Sub.Subject_Name into Subject
//select new
//{
//	Name= Subject.Key,
//	FailedCount= Subject.Count(u=>u.User_Score < 32),
//	PassedCount= Subject.Count(u=>u.User_Score>32)
//}




//(from SR in tbl_Student_Report
//select new
//{
//	SR.Student_Id
//}).Distinct()


//from SR in tbl_Student_Report
//join Student in tbl_Student_Master on SR.Student_Id equals Student.Student_Id
//join SSM in tbl_Semister_Subject_Map on SR.Sem_Subject_Id equals SSM.Sem_Subject_Id
//join Sem in tbl_Semister_Master on SSM.Semister_Id equals Sem.Semister_Id
//join Sub in tbl_Subject_Master on SSM.Subject_Id equals Sub.Subject_Id
//group SR by Student.Student_Name into Stud
//where Stud.Average(u=>u.User_Score)<60
//select new
//{
//	Name= Stud.Key,
//	Subject= Stud.Count(),
//	Avg= Stud.Average(u=>u.User_Score)
//}



//from SR in tbl_Student_Report
//join Student in tbl_Student_Master on SR.Student_Id equals Student.Student_Id
//join SSM in tbl_Semister_Subject_Map on SR.Sem_Subject_Id equals SSM.Sem_Subject_Id
//join Sem in tbl_Semister_Master on SSM.Semister_Id equals Sem.Semister_Id
//join Sub in tbl_Subject_Master on SSM.Subject_Id equals Sub.Subject_Id 
//where Sub.Subject_Name=="TFPS" || Sub.Subject_Name=="Ed"
//select new
//{
//	Student.Student_Name,
//	Sub.Subject_Name,
//	SR.User_Score
//}



(from SR in tbl_Student_Report
join Student in tbl_Student_Master on SR.Student_Id equals Student.Student_Id
join SSM in tbl_Semister_Subject_Map on SR.Sem_Subject_Id equals SSM.Sem_Subject_Id
join Sem in tbl_Semister_Master on SSM.Semister_Id equals Sem.Semister_Id
join Sub in tbl_Subject_Master on SSM.Subject_Id equals Sub.Subject_Id 
join Year in tbl_Year_Master on Sem.Year_Id equals Year.Year_Id
select new
{
	Student.Student_Name,
	Sub.Subject_Name,
	Sem.Semister_Name,
	SR.User_Score
})
.ToList()
.GroupBy(s=>s.Semister_Name)
.SelectMany(s=>s.OrderBy(u=>u.User_Score)
				.Select((g, index)=>
				new
				{
					g.Student_Name,
					g.Subject_Name,
					g.Semister_Name,
					g.User_Score,
					Sem=index+1
				}
				))
				.Where(s=>s.Sem==1)
				
