using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduAcess.Models
{
	public class Programme
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public string Name { get; set; }

		[Required]
		public string QualificationType { get; set; }

		[Required]
		public string FieldOfStudy { get; set; }

		public int RequiredAps { get; set; }

		public int DurationYears { get; set; }

		public string RequiredSubjects { get; set; }

		public int InstitutionId { get; set; }

		[ForeignKey("InstitutionId")]
		public virtual Institution Institution { get; set; }
	}
}