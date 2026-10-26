using System.ComponentModel.DataAnnotations;

namespace EduAcess.Models
{
	public class Institution
	{
		[Key]
		public int Id { get; set; }

		[Required]
		[Display(Name = "Institution Name")]
		public string Name { get; set; }

		[Display(Name = "Abbreviation")]
		public string ShortName { get; set; }

		[Required]
		[Display(Name = "Type")]
		public string Type { get; set; } // "University", "TVET", "Private"

		[Required]
		[Display(Name = "Province")]
		public string Province { get; set; }
	}
}