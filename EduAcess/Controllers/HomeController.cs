using EduAcess.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using EduAcess.Services; // Make sure this is at the top

namespace EduAcess.Controllers
{
	public class HomeController : Controller
	{
		private ApplicationUserManager _userManager;

		public ApplicationUserManager UserManager
		{
			get
			{
				return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
			}
			private set
			{
				_userManager = value;
			}
		}

		public async Task<ActionResult> Index()
		{
			ViewBag.ApplicationDeadline = new DateTime(2026, 11, 30, 23, 59, 59);

			// --- Live Stats (Public-facing) ---
			ViewBag.TotalUsers = await UserManager.Users.CountAsync();
			ViewBag.ActiveUsers = await UserManager.Users.CountAsync(u => u.EmailConfirmed);
			ViewBag.PendingUsers = await UserManager.Users.CountAsync(u => !u.EmailConfirmed);

			// --- Real Institutions & Bursaries Count ---
			using (var context = new ApplicationDbContext())
			{
				ViewBag.TotalInstitutions = await context.Institutions.CountAsync();
				ViewBag.TotalBursaries = await context.Bursaries.CountAsync();
			}

			return View();
		}
		[HttpPost]
		[AllowAnonymous]
		public async Task<JsonResult> ChatBotResponse(string message)
		{
			if (string.IsNullOrWhiteSpace(message))
				return Json(new { reply = "Please ask me a question!" });

			// ⚠️ PASTE YOUR GROQ KEY HERE (starts with gsk_)
			var groqKey = "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";

			var service = new GroqService(groqKey);
			var reply = await service.GetChatResponseAsync(message);

			return Json(new { reply = reply });
		}
		public ActionResult About()
		{
			ViewBag.Message = "Your application description page.";
			return View();
		}

		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";
			return View();
		}
	}
}