using EduAcess.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace EduAcess.Controllers
{
	[Authorize(Roles = "Admin")]
	public class AdminDashboardController : Controller
	{
		private ApplicationUserManager _userManager;

		public AdminDashboardController()
		{
		}

		public AdminDashboardController(ApplicationUserManager userManager)
		{
			UserManager = userManager;
		}

		public ApplicationUserManager UserManager
		{
			get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
			private set { _userManager = value; }
		}

		// GET: /AdminDashboard/
		public async Task<ActionResult> Index()
		{
			var viewModel = new AdminDashboardViewModel();

			using (var context = new ApplicationDbContext())
			{
				// --- 1. Core Stats ---
				viewModel.TotalUsers = await UserManager.Users.CountAsync();
				viewModel.ActiveUsers = await UserManager.Users.CountAsync(u => u.EmailConfirmed);
				viewModel.PendingUsers = viewModel.TotalUsers - viewModel.ActiveUsers;
				viewModel.TotalInstitutions = await context.Institutions.CountAsync();
				viewModel.TotalBursaries = await context.Bursaries.CountAsync();

				// Social Impact Metric (Plan B Candidates - learners with APS between 1 and 29)
				viewModel.TotalPlanBMatches = await context.LearnerProfiles.CountAsync(p => p.APS > 0 && p.APS < 30);

				var profiles = await context.LearnerProfiles.ToListAsync();

				// --- 2. Chart: APS Distribution ---
				var apsGroups = profiles.GroupBy(p =>
					p.APS == 0 ? "No APS" :
					p.APS < 20 ? "< 20" :
					p.APS <= 25 ? "20-25" :
					p.APS <= 30 ? "26-30" :
					p.APS <= 35 ? "31-35" : "36+"
				).Select(g => new { Group = g.Key, Count = g.Count() }).ToList();

				viewModel.ApsLabels = apsGroups.Select(g => g.Group).ToList();
				viewModel.ApsData = apsGroups.Select(g => g.Count).ToList();

				// --- 3. Chart: Top Interests ---
				var allInterests = profiles.Where(p => !string.IsNullOrEmpty(p.Interests))
										   .SelectMany(p => p.Interests.Split(','))
										   .Select(i => i.Trim())
										   .GroupBy(i => i)
										   .Select(g => new { Interest = g.Key, Count = g.Count() })
										   .OrderByDescending(g => g.Count)
										   .Take(5)
										   .ToList();

				viewModel.InterestLabels = allInterests.Select(i => i.Interest).ToList();
				viewModel.InterestData = allInterests.Select(i => i.Count).ToList();

				// --- 4. Chart: Geographic Distribution (Provinces) ---
				var allProvinces = profiles.Where(p => !string.IsNullOrEmpty(p.Province))
										   .GroupBy(p => p.Province)
										   .Select(g => new { Province = g.Key, Count = g.Count() })
										   .OrderByDescending(g => g.Count)
										   .ToList();

				viewModel.ProvinceLabels = allProvinces.Select(p => p.Province).ToList();
				viewModel.ProvinceData = allProvinces.Select(p => p.Count).ToList();

				// --- 5. Live Activity Feed ---
				var recentUsers = await UserManager.Users
												   .OrderByDescending(u => u.Id)
												   .Take(5)
												   .ToListAsync();

				viewModel.RecentActivities = recentUsers.Select(u => new RecentActivity
				{
					UserName = !string.IsNullOrEmpty(u.FirstName) ? u.FirstName : u.Email.Split('@')[0],
					Action = "created a new account",
					Time = DateTime.Now.AddMinutes(-new Random().Next(1, 60))
				}).ToList();
			}

			return View(viewModel);
		}

		// GET: /AdminDashboard/ExportUsersToCsv
		public ActionResult ExportUsersToCsv()
		{
			using (var context = new ApplicationDbContext())
			{
				var data = (from u in context.Users
							join p in context.LearnerProfiles on u.Id equals p.UserId into gj
							from profile in gj.DefaultIfEmpty()
							select new
							{
								Email = u.Email,
								FirstName = u.FirstName,
								Phone = u.PhoneNumber,
								APS = profile != null ? profile.APS : 0,
								Interests = profile != null ? profile.Interests : "Not Set",
								Income = profile != null ? profile.HouseholdIncome : "Unknown",
								Province = profile != null ? profile.Province : "Unknown"
							}).ToList();

				var csv = new StringBuilder();
				csv.AppendLine("Email,FirstName,PhoneNumber,APS,Interests,HouseholdIncome,Province");

				foreach (var row in data)
				{
					csv.AppendLine($"\"{row.Email}\",\"{row.FirstName}\",\"{row.Phone}\",{row.APS},\"{row.Interests}\",\"{row.Income}\",\"{row.Province}\"");
				}

				return File(Encoding.UTF8.GetBytes(csv.ToString()), "text/csv", "EduAccess_User_Data.csv");
			}
		}

		// GET: /AdminDashboard/Users
		public async Task<ActionResult> Users()
		{
			var users = await UserManager.Users
										 .OrderByDescending(u => u.Id)
										 .ToListAsync();
			return View(users);
		}

		// GET: /AdminDashboard/Institutions
		public async Task<ActionResult> Institutions()
		{
			using (var context = new ApplicationDbContext())
			{
				var institutions = await context.Institutions
												.OrderBy(i => i.Type)
												.ThenBy(i => i.Province)
												.ThenBy(i => i.Name)
												.ToListAsync();
				return View(institutions);
			}
		}

		// GET: /AdminDashboard/Bursaries
		public async Task<ActionResult> Bursaries()
		{
			using (var context = new ApplicationDbContext())
			{
				var bursaries = await context.Bursaries
											 .OrderBy(b => b.ClosingDate == null)
											 .ThenBy(b => b.ClosingDate)
											 .ThenBy(b => b.Name)
											 .ToListAsync();
				return View(bursaries);
			}
		}
	}
}