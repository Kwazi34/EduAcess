using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using EduAcess.Models;

namespace EduAcess.Controllers
{
	public class ProgrammesController : Controller
	{
		// GET: /Programmes
		public async Task<ActionResult> Index(string fieldOfStudy, string province, string institution, string qualificationType)
		{
			using (var context = new ApplicationDbContext())
			{
				var query = context.Programmes.Include(p => p.Institution).AsQueryable();

				if (!string.IsNullOrEmpty(province))
					query = query.Where(p => p.Institution.Province == province);
				if (!string.IsNullOrEmpty(institution))
					query = query.Where(p => p.Institution.Name == institution);
				if (!string.IsNullOrEmpty(fieldOfStudy))
					query = query.Where(p => p.FieldOfStudy == fieldOfStudy);
				if (!string.IsNullOrEmpty(qualificationType))
					query = query.Where(p => p.QualificationType == qualificationType);

				var programmes = await query.OrderBy(p => p.Institution.Name).ThenBy(p => p.Name).ToListAsync();
				var allInstitutions = await context.Institutions.AsNoTracking().ToListAsync();

				// If logged in with marks, calculate APS so we can show eligibility badges
				int learnerAps = 0;
				bool hasMarks = false;
				if (User.Identity.IsAuthenticated)
				{
					var userId = User.Identity.GetUserId();
					var profile = await context.LearnerProfiles.Include(p => p.Subjects).FirstOrDefaultAsync(p => p.UserId == userId);
					if (profile != null && profile.Subjects.Any(s => s.Mark > 0 && !s.IsLifeOrientation))
					{
						hasMarks = true;
						learnerAps = profile.Subjects
							.Where(s => !s.IsLifeOrientation)
							.Select(s => LevelPoints(s.Mark))
							.OrderByDescending(p => p)
							.Take(6)
							.Sum();
					}
				}

				ViewBag.LearnerAps = learnerAps;
				ViewBag.HasMarks = hasMarks;
				ViewBag.Province = province;
				ViewBag.Institution = institution;
				ViewBag.FieldOfStudy = fieldOfStudy;
				ViewBag.QualificationType = qualificationType;

				ViewBag.ProvinceOptions = new[] {
					"Eastern Cape", "Free State", "Gauteng", "KwaZulu-Natal",
					"Limpopo", "Mpumalanga", "North West", "Northern Cape", "Western Cape"
				};
				ViewBag.InstitutionOptions = allInstitutions.Select(i => i.Name).Distinct().OrderBy(n => n).ToList();
				ViewBag.FieldOptions = programmes.Select(p => p.FieldOfStudy).Distinct().OrderBy(f => f).ToList();
				ViewBag.QualificationOptions = programmes.Select(p => p.QualificationType).Distinct().OrderBy(q => q).ToList();

				return View(programmes);
			}
		}
		// GET: /Programmes/Details/{id}
		public async Task<ActionResult> Details(int id)
		{
			using (var context = new ApplicationDbContext())
			{
				var programme = await context.Programmes
											 .Include(p => p.Institution)
											 .FirstOrDefaultAsync(p => p.Id == id);

				if (programme == null)
				{
					return HttpNotFound();
				}

				// Calculate learner eligibility if logged in
				int learnerAps = 0;
				bool hasMarks = false;
				var matchedSubjects = new System.Collections.Generic.List<string>();
				var missingSubjects = new System.Collections.Generic.List<string>();

				if (User.Identity.IsAuthenticated)
				{
					var userId = User.Identity.GetUserId();
					var profile = await context.LearnerProfiles
											   .Include(p => p.Subjects)
											   .FirstOrDefaultAsync(p => p.UserId == userId);

					if (profile != null && profile.Subjects.Any(s => s.Mark > 0 && !s.IsLifeOrientation))
					{
						hasMarks = true;
						learnerAps = profile.Subjects
							.Where(s => !s.IsLifeOrientation)
							.Select(s => LevelPoints(s.Mark))
							.OrderByDescending(p => p)
							.Take(6)
							.Sum();

						// Check individual subject requirements
						var learnerSubjects = profile.Subjects.Select(s => s.SubjectName.ToLower()).ToList();
						var required = programme.RequiredSubjects?.Split(',') ?? new string[0];
						foreach (var req in required)
						{
							var name = req.Trim().Split(' ')[0].ToLower();
							if (learnerSubjects.Any(s => s.Contains(name)))
								matchedSubjects.Add(req.Trim());
							else
								missingSubjects.Add(req.Trim());
						}
					}
				}

				ViewBag.LearnerAps = learnerAps;
				ViewBag.HasMarks = hasMarks;
				ViewBag.MatchedSubjects = matchedSubjects;
				ViewBag.MissingSubjects = missingSubjects;

				return View(programme);
			}
		}
		private static int LevelPoints(int percent)
		{
			if (percent >= 80) return 7;
			if (percent >= 70) return 6;
			if (percent >= 60) return 5;
			if (percent >= 50) return 4;
			if (percent >= 40) return 3;
			if (percent >= 30) return 2;
			return 1;
		}
	}
}