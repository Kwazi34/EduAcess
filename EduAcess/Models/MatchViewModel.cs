using System.Collections.Generic;
using System.Web.Mvc;

namespace EduAcess.Models
{
	public class MatchViewModel
	{
		public int Id { get; set; } // ADD THIS
		public string Name { get; set; }
		public string Institution { get; set; }
		public string QualificationType { get; set; }
		public string FieldOfStudy { get; set; }
		public int Years { get; set; }
		public int Score { get; set; }
		public bool Eligible { get; set; }
		public bool NSFAS { get; set; }
		public int Bursaries { get; set; }
		public int APShort { get; set; }
		public string RequiredSubjects { get; set; }
	}

	public class DashboardViewModel
	{
		public int APS { get; set; }
		public string HouseholdIncome { get; set; }
		public List<EduAcess.Models.LearnerSubject> Subjects { get; set; }
		public List<MatchViewModel> Matches { get; set; }
		public List<EduAcess.Models.Bursary> Bursaries { get; set; }
		public int TotalInstitutions { get; set; }
		public int TotalBursaries { get; set; }
		public int TotalProgrammes { get; set; }

		public string FieldOfStudy { get; set; }
		public string Province { get; set; }
		public string Institution { get; set; }
		public string QualificationType { get; set; }
		public string Interests { get; set; }
		public string UserName { get; set; }
		public List<MatchViewModel> PlanBMatches { get; set; }
		public List<MatchViewModel> RecommendedMatches { get; set; }
		public IEnumerable<SelectListItem> FieldOptions { get; set; }
		public IEnumerable<SelectListItem> ProvinceOptions { get; set; }
		public IEnumerable<SelectListItem> InstitutionOptions { get; set; }
		public IEnumerable<SelectListItem> QualificationOptions { get; set; }
	}
	public class AdminDashboardViewModel
	{
		// Core Stats
		public int TotalUsers { get; set; }
		public int ActiveUsers { get; set; }
		public int PendingUsers { get; set; }
		public int TotalInstitutions { get; set; }
		public int TotalBursaries { get; set; }
		public int TotalPlanBMatches { get; set; } // Social Impact Metric

		// Chart Data
		public List<string> ApsLabels { get; set; }
		public List<int> ApsData { get; set; }

		public List<string> InterestLabels { get; set; }
		public List<int> InterestData { get; set; }

		public List<string> ProvinceLabels { get; set; }
		public List<int> ProvinceData { get; set; }

		// Live Feed
		public List<RecentActivity> RecentActivities { get; set; }
	}

	public class RecentActivity
	{
		public string UserName { get; set; }
		public string Action { get; set; }
		public System.DateTime Time { get; set; }
	}
}