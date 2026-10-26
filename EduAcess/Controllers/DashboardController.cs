using EduAcess.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;

namespace EduAcess.Controllers
{
	[Authorize(Roles = "Learner,Admin")]
	public class DashboardController : Controller
	{
		private static int LevelPoints(int percent)
		{
			if (percent >= 80) return 7;
			if (percent >= 70) return 6;
			if (percent >= 60) return 5;
			if (percent >= 50) return 4;
			if (percent >= 40) return 3;
			if (percent >= 30) return 2;
			if (percent > 0) return 1;
			return 0;
		}

		private static int CalculateAps(List<LearnerSubject> subjects, string institutionType = "University")
		{
			var valid = subjects.Where(s => !s.IsLifeOrientation && s.Mark > 0).ToList();
			if (valid.Count == 0) return 0;
			int topCount = institutionType == "TVET" ? 4 : 6;
			return valid.Select(s => LevelPoints(s.Mark)).OrderByDescending(p => p).Take(topCount).Sum();
		}

		// GET: /Dashboard 
		public async Task<ActionResult> Index(string fieldOfStudy, string province, string institution, string qualificationType)
		{
			var userId = User.Identity.GetUserId();

			using (var context = new ApplicationDbContext())
			{
				var profile = await context.LearnerProfiles
										   .Include(p => p.Subjects)
										   .FirstOrDefaultAsync(p => p.UserId == userId);

				if (profile == null || profile.Subjects == null || !profile.Subjects.Any(s => s.Mark > 0))
				{
					return RedirectToAction("Onboarding");
				}

				var user = await UserManager.FindByIdAsync(userId);
				string displayName = user != null && !string.IsNullOrEmpty(user.FirstName)
									 ? user.FirstName
									 : (user?.Email ?? "Learner");

				int aps = CalculateAps(profile.Subjects.ToList(), "University");
				profile.APS = aps;
				await context.SaveChangesAsync();

				var allInstitutions = await context.Institutions.AsNoTracking().ToListAsync();
				var allProgrammes = await context.Programmes.Include(p => p.Institution).AsNoTracking().ToListAsync();

				var userInterests = string.IsNullOrEmpty(profile.Interests)
					? new List<string>()
					: profile.Interests.Split(',').Select(i => i.Trim().ToLower()).ToList();

				var directMatches = new List<MatchViewModel>();
				var recommendedMatches = new List<MatchViewModel>(); // Renamed from PlanB to Recommendations

				foreach (var prog in allProgrammes)
				{
					if (prog.Institution == null) continue;
					if (!string.IsNullOrEmpty(province) && prog.Institution.Province != province) continue;
					if (!string.IsNullOrEmpty(institution) && prog.Institution.Name != institution) continue;
					if (!string.IsNullOrEmpty(fieldOfStudy) && prog.FieldOfStudy != fieldOfStudy) continue;
					if (!string.IsNullOrEmpty(qualificationType) && prog.QualificationType != qualificationType) continue;

					int instAps = CalculateAps(profile.Subjects.ToList(), prog.Institution.Type);
					bool eligible = instAps >= prog.RequiredAps;
					int score = Math.Min(100, (instAps * 100) / Math.Max(prog.RequiredAps, 1));

					var match = new MatchViewModel
					{
						Id = prog.Id,
						Name = prog.Name,
						Institution = prog.Institution.Name,
						QualificationType = prog.QualificationType,
						FieldOfStudy = prog.FieldOfStudy,
						Years = prog.DurationYears,
						Score = score,
						Eligible = eligible,
						NSFAS = eligible && profile.HouseholdIncome == "Under R350k",
						Bursaries = eligible ? 2 : 0,
						APShort = eligible ? 0 : (prog.RequiredAps - instAps),
						RequiredSubjects = prog.RequiredSubjects
					};

					bool isInterestMatch = userInterests.Count == 0 ||
										   userInterests.Any(ui => prog.FieldOfStudy.ToLower().Contains(ui) ||
																	prog.Name.ToLower().Contains(ui) ||
																	ui.Contains(prog.FieldOfStudy.ToLower()));

					if (eligible && isInterestMatch)
						directMatches.Add(match);
					else if (eligible && !isInterestMatch)
						recommendedMatches.Add(match);
					// Note: If !eligible && isInterestMatch, we don't show it on the main dashboard (they see it in Plan B)
				}

				directMatches = directMatches.OrderByDescending(m => m.Score).Take(20).ToList();
				recommendedMatches = recommendedMatches.OrderByDescending(m => m.Score).Take(10).ToList();

				var viewModel = new DashboardViewModel
				{
					UserName = displayName,
					APS = aps,
					HouseholdIncome = profile.HouseholdIncome ?? "Not set",
					Interests = profile.Interests ?? "None selected",
					Subjects = profile.Subjects.OrderByDescending(s => s.Mark).ToList(),
					Matches = directMatches,
					RecommendedMatches = recommendedMatches,
					TotalProgrammes = allProgrammes.Count,

					FieldOfStudy = fieldOfStudy,
					Province = province,
					Institution = institution,
					QualificationType = qualificationType,

					FieldOptions = allProgrammes.Select(p => p.FieldOfStudy).Distinct().OrderBy(f => f).Select(f => new SelectListItem { Text = f, Value = f }),
					ProvinceOptions = allInstitutions.Select(i => i.Province).Distinct().OrderBy(p => p).Select(p => new SelectListItem { Text = p, Value = p }),
					InstitutionOptions = allInstitutions.Select(i => i.Name).Distinct().OrderBy(n => n).Select(n => new SelectListItem { Text = n, Value = n }),
					QualificationOptions = allProgrammes.Select(p => p.QualificationType).Distinct().OrderBy(q => q).Select(q => new SelectListItem { Text = q, Value = q })
				};

				return View(viewModel);
			}
		}

		// GET: /Dashboard/ExplorePlanB
		public async Task<ActionResult> ExplorePlanB()
		{
			var userId = User.Identity.GetUserId();

			using (var context = new ApplicationDbContext())
			{
				var profile = await context.LearnerProfiles
										   .Include(p => p.Subjects)
										   .FirstOrDefaultAsync(p => p.UserId == userId);

				if (profile == null) return RedirectToAction("Onboarding");

				var user = await UserManager.FindByIdAsync(userId);
				string displayName = user?.FirstName ?? "Learner";

				int aps = CalculateAps(profile.Subjects.ToList(), "University");
				var allInstitutions = await context.Institutions.AsNoTracking().ToListAsync();
				var allProgrammes = await context.Programmes.Include(p => p.Institution).AsNoTracking().ToListAsync();

				var userInterests = string.IsNullOrEmpty(profile.Interests)
					? new List<string>()
					: profile.Interests.Split(',').Select(i => i.Trim().ToLower()).ToList();

				var planBMatches = new List<MatchViewModel>();

				foreach (var prog in allProgrammes)
				{
					if (prog.Institution == null) continue;

					int instAps = CalculateAps(profile.Subjects.ToList(), prog.Institution.Type);
					bool eligible = instAps >= prog.RequiredAps;

					bool isInterestMatch = userInterests.Count == 0 ||
										   userInterests.Any(ui => prog.FieldOfStudy.ToLower().Contains(ui) ||
																	prog.Name.ToLower().Contains(ui) ||
																	ui.Contains(prog.FieldOfStudy.ToLower()));

					// Plan B = You qualify (eligible) BUT you didn't choose this interest.
					if (eligible && !isInterestMatch)
					{
						planBMatches.Add(new MatchViewModel
						{
							Id = prog.Id,
							Name = prog.Name,
							Institution = prog.Institution.Name,
							QualificationType = prog.QualificationType,
							FieldOfStudy = prog.FieldOfStudy,
							Years = prog.DurationYears,
							Eligible = true,
							APShort = 0,
							RequiredSubjects = prog.RequiredSubjects
						});
					}
				}

				planBMatches = planBMatches.OrderBy(m => m.Name).ToList();

				var viewModel = new DashboardViewModel
				{
					UserName = displayName,
					APS = aps,
					Interests = profile.Interests ?? "None",
					PlanBMatches = planBMatches
				};

				return View(viewModel);
			}
		}
		[HttpGet]
		public async Task<JsonResult> GetPassportData()
		{
			var userId = User.Identity.GetUserId();
			using (var context = new ApplicationDbContext())
			{
				var profile = await context.LearnerProfiles
										   .Include(p => p.Subjects)
										   .FirstOrDefaultAsync(p => p.UserId == userId);

				if (profile == null) return Json(new { success = false }, JsonRequestBehavior.AllowGet);

				return Json(new
				{
					success = true,
					income = profile.HouseholdIncome,
					interests = profile.Interests,
					gender = profile.Gender,
					language = profile.Language, // NEW
					province = profile.Province, // NEW
					ageGroup = profile.AgeGroup, // ADD THIS LINE
					subjects = profile.Subjects.Select(s => new { name = s.SubjectName, mark = s.Mark }).ToList()
				}, JsonRequestBehavior.AllowGet);
			}
		}
		// GET: /Dashboard/SharePassport
		[HttpGet]
		public async Task<ActionResult> SharePassport()
		{
			var userId = User.Identity.GetUserId();
			using (var context = new ApplicationDbContext())
			{
				var profile = await context.LearnerProfiles
										   .Include(p => p.Subjects)
										   .FirstOrDefaultAsync(p => p.UserId == userId);

				if (profile == null) return RedirectToAction("Onboarding");

				var user = await UserManager.FindByIdAsync(userId);
				ViewBag.Name = user?.FirstName ?? "Learner";
				ViewBag.APS = profile.APS;
				ViewBag.Interests = profile.Interests;
				ViewBag.Subjects = profile.Subjects.OrderByDescending(s => s.Mark).ToList();

				// Generate a shareable link (for demo, use the current URL + token)
				// In production, save a GUID to a ShareTokens table
				ViewBag.ShareLink = Request.Url.GetLeftPart(UriPartial.Authority) + "/Home/Index";

				return View();
			}
		}
		// GET: /Dashboard/ProgrammeDetails/5
		public async Task<ActionResult> ProgrammeDetails(int id)
		{
			using (var context = new ApplicationDbContext())
			{
				var programme = await context.Programmes
											 .Include(p => p.Institution)
											 .FirstOrDefaultAsync(p => p.Id == id);

				if (programme == null) return HttpNotFound();

				return View(programme);
			}
		}

		public ActionResult Onboarding()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> SavePassportForm(string Income, string SubjectsJson, string Interests, string Gender, string Language, string Province, string AgeGroup)
		{
			try
			{
				var userId = User.Identity.GetUserId();

				using (var context = new ApplicationDbContext())
				{
					var profile = await context.LearnerProfiles
											   .Include(p => p.Subjects)
											   .FirstOrDefaultAsync(p => p.UserId == userId);

					if (profile == null)
					{
						profile = new LearnerProfile { UserId = userId };
						context.LearnerProfiles.Add(profile);
						await context.SaveChangesAsync();
					}

					if (profile.Subjects != null && profile.Subjects.Any())
					{
						context.LearnerSubjects.RemoveRange(profile.Subjects);
					}

					if (!string.IsNullOrEmpty(SubjectsJson))
					{
						var subjects = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PassportSubject>>(SubjectsJson);
						if (subjects != null)
						{
							foreach (var s in subjects)
							{
								context.LearnerSubjects.Add(new LearnerSubject
								{
									LearnerProfileId = profile.Id,
									SubjectName = s.Name,
									Mark = s.Mark,
									IsLifeOrientation = s.Name.ToLower().Contains("life orientation")
								});
							}
						}
					}

					// Save all fields
					if (!string.IsNullOrEmpty(Income)) profile.HouseholdIncome = Income;
					if (!string.IsNullOrEmpty(Interests)) profile.Interests = Interests;
					if (!string.IsNullOrEmpty(Gender)) profile.Gender = Gender;
					if (!string.IsNullOrEmpty(Language)) profile.Language = Language; // NEW
					if (!string.IsNullOrEmpty(Province)) profile.Province = Province; // NEW
					if (!string.IsNullOrEmpty(AgeGroup)) profile.AgeGroup = AgeGroup;
					await context.SaveChangesAsync();
				}

				return RedirectToAction("Index", "Dashboard");
			}
			catch (Exception ex)
			{
				TempData["Error"] = "Could not save data: " + ex.Message;
				return RedirectToAction("Onboarding");
			}
		}
		private ApplicationUserManager _userManager;
		public ApplicationUserManager UserManager
		{
			get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
			private set { _userManager = value; }
		}

		public class PassportViewModel
		{
			public string Income { get; set; }
			public List<PassportSubject> Subjects { get; set; }
		}

		public class PassportSubject
		{
			public string Name { get; set; }
			public int Mark { get; set; }
		}
	}
}