using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using EduAcess.Models;

namespace EduAcess.Controllers
{
	// PUBLIC — no [Authorize]
	public class FundingController : Controller
	{
		public async Task<ActionResult> Index()
		{
			using (var context = new ApplicationDbContext())
			{
				var bursaries = await context.Bursaries
											 .Where(b => b.IsActive)
											 .OrderBy(b => b.ClosingDate)
											 .ToListAsync();

				string householdIncome = "Not set";
				bool nsfasLikely = false;

				if (User.Identity.IsAuthenticated)
				{
					var userId = User.Identity.GetUserId();
					var profile = await context.LearnerProfiles
											   .FirstOrDefaultAsync(p => p.UserId == userId);
					if (profile != null)
					{
						householdIncome = profile.HouseholdIncome ?? "Not set";
						nsfasLikely = profile.HouseholdIncome == "Under R350k";
					}
				}

				ViewBag.HouseholdIncome = householdIncome;
				ViewBag.NSFASLikely = nsfasLikely;

				return View(bursaries);
			}
		}

		// GET: /Funding/Details/{id}
		public async Task<ActionResult> Details(int id)
		{
			using (var context = new ApplicationDbContext())
			{
				var bursary = await context.Bursaries.FirstOrDefaultAsync(b => b.Id == id);

				if (bursary == null)
				{
					return HttpNotFound();
				}

				ViewBag.IsLoggedIn = User.Identity.IsAuthenticated;
				return View(bursary);
			}
		}
	}
}