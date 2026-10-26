using System;
using System.ComponentModel.DataAnnotations;

namespace EduAcess.Models
{
	public class SiteVisit
	{
		[Key]
		public int Id { get; set; }

		public DateTime VisitDate { get; set; } = DateTime.Now;

		// Nullable, because they might not be logged in yet
		public string UserId { get; set; }
	}
}