using System;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using EduAcess.Models;

namespace EduAcess
{
	public class MvcApplication : HttpApplication
	{
		protected void Application_Start()
		{
			// 1. Database initialization (Optional - forces database to update schema)
			Database.SetInitializer(new CreateDatabaseIfNotExists<ApplicationDbContext>());

			AreaRegistration.RegisterAllAreas();
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);

			// 2. Seed Roles and Default Admin
			SeedIdentity();

			// 3. Seed Institutions & Bursaries (via public SeedAll method)
			using (var context = new ApplicationDbContext())
			{
				SeedData.SeedAll(context);
			}
		}

		private void SeedIdentity()
		{
			var context = new ApplicationDbContext();
			var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
			var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));

			// --- A. Create Roles ---
			if (!roleManager.RoleExists("Admin"))
			{
				roleManager.Create(new IdentityRole("Admin"));
			}

			if (!roleManager.RoleExists("Learner"))
			{
				roleManager.Create(new IdentityRole("Learner"));
			}

			// --- B. Create Default Admin Account ---
			var adminEmail = "admin@eduaccess.co.za";
			var adminUser = userManager.FindByEmail(adminEmail);

			if (adminUser == null)
			{
				var admin = new ApplicationUser
				{
					UserName = adminEmail,
					Email = adminEmail,
					EmailConfirmed = true,
					MustChangePassword = true
				};

				var result = userManager.Create(admin, "Admin@12345");

				if (result.Succeeded)
				{
					userManager.AddToRole(admin.Id, "Admin");
				}
			}
		}
	}
}