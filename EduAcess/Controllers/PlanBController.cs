using System.Web.Mvc;

namespace EduAcess.Controllers
{
	// PUBLIC — no [Authorize]
	public class PlanBController : Controller
	{
		public ActionResult Index()
		{
			return View();
		}

		// GET: /PlanB/Details/{route}
		public ActionResult Details(string route)
		{
			// Simple validation
			if (string.IsNullOrEmpty(route))
				return RedirectToAction("Index");

			ViewBag.Route = route;
			return View();
		}
	}
}