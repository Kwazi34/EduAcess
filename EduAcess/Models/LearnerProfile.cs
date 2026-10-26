using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduAcess.Models
{
	public class LearnerProfile
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public string UserId { get; set; }

		public string HouseholdIncome { get; set; }

		public int APS { get; set; }
		public string Language { get; set; }
		public string Province { get; set; }
		public string AgeGroup { get; set; }
		public DateTime LastUpdated { get; set; } = DateTime.Now;

		// ADD THESE TWO LINES
		public string Gender { get; set; }
		public string Interests { get; set; }

		public virtual ICollection<LearnerSubject> Subjects { get; set; } = new List<LearnerSubject>();
	}

	public class LearnerSubject
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public int LearnerProfileId { get; set; }

		[ForeignKey("LearnerProfileId")]
		public virtual LearnerProfile LearnerProfile { get; set; }

		[Required]
		[Display(Name = "Subject")]
		public string SubjectName { get; set; }

		[Required]
		[Range(0, 100)]
		[Display(Name = "Mark (%)")]
		public int Mark { get; set; }

		public bool IsLifeOrientation { get; set; } = false;
	}
}