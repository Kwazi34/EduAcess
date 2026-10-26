using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace EduAcess.Models
{
	// You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
	public class ApplicationUser : IdentityUser
	{
		// --- EduAccess custom properties ---

		// Display name for the user
		
		public string FirstName { get; set; }
		public string Surname { get; set; }
		public string FullName { get; set; }
		public string VerificationCode { get; set; }   // <-- ADD THIS
		public string FaceDescriptor { get; set; }

		// When true, the user is forced to change their password before using the app
		public bool MustChangePassword { get; set; }

		// -----------------------------------

		public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
		{
			// Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
			var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
			// Add custom user claims here
			return userIdentity;
		}
	}

	public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
	{
		public ApplicationDbContext()
			: base("DefaultConnection", throwIfV1Schema: false)
		{
		}
		public DbSet<Bursary>Bursaries { get; set; }
		public DbSet<Institution> Institutions { get; set; }
		public DbSet<Programme> Programmes { get; set; }
		public DbSet<LearnerProfile> LearnerProfiles { get; set; }
		public DbSet<LearnerSubject> LearnerSubjects { get; set; }
		public DbSet<SiteVisit> SiteVisits { get; set; }

		public static ApplicationDbContext Create()
		{
			return new ApplicationDbContext();
		}
	}
}