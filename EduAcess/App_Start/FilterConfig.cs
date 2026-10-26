using System.Web;
using System.Web.Mvc;
using EduAcess.Models;
using Microsoft.AspNet.Identity;

namespace EduAcess
{
	public class FilterConfig
	{
		public static void RegisterGlobalFilters(GlobalFilterCollection filters)
		{
			filters.Add(new HandleErrorAttribute());
			filters.Add(new TrackVisitAttribute()); // ADD THIS LINE
		}
	}

	// Create this class in the same file
	public class TrackVisitAttribute : ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			// Don't track API calls, static files, or admin panel views (optional)
			var controllerName = filterContext.RouteData.Values["controller"].ToString();
			if (controllerName == "AdminDashboard") return;

			var userId = filterContext.HttpContext.User?.Identity?.GetUserId();

			using (var context = new ApplicationDbContext())
			{
				context.SiteVisits.Add(new SiteVisit
				{
					UserId = userId,
					VisitDate = System.DateTime.Now
				});
				context.SaveChanges();
			}

			base.OnActionExecuting(filterContext);
		}
	}
}