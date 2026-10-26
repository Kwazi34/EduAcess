using System;
using System.ComponentModel.DataAnnotations;

namespace EduAcess.Models
{
	public class Bursary
	{
		[Key]
		public int Id { get; set; }

		[Required]
		[Display(Name = "Bursary Name")]
		public string Name { get; set; }

		[Required]
		[Display(Name = "Provider")]
		public string Provider { get; set; }

		[Display(Name = "Field of Study")]
		public string FieldOfStudy { get; set; }

		[Display(Name = "Opening Date")]
		[DataType(DataType.Date)]
		[DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
		public DateTime? OpeningDate { get; set; }

		[Display(Name = "Closing Date")]
		[DataType(DataType.Date)]
		[DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
		public DateTime? ClosingDate { get; set; }

		[Display(Name = "Description")]
		public string Description { get; set; }

		[Display(Name = "Website")]
		public string Website { get; set; }

		[Display(Name = "Is Active")]
		public bool IsActive { get; set; } = true;
	}
}