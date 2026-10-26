using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EduAcess.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
	}

  
		public class RegisterViewModel
		{
			[Required(ErrorMessage = "First name is required.")]
			[StringLength(50, ErrorMessage = "First name must be 50 characters or fewer.")]
			[Display(Name = "First Name")]
			public string FirstName { get; set; }

			[Required(ErrorMessage = "Surname is required.")]
			[StringLength(50, ErrorMessage = "Surname must be 50 characters or fewer.")]
			[Display(Name = "Surname")]
			public string Surname { get; set; }

			[Required(ErrorMessage = "Phone number is required.")]
			[Phone(ErrorMessage = "Please enter a valid phone number.")]
			[StringLength(15, ErrorMessage = "Phone number must be 15 characters or fewer.")]
			[Display(Name = "Phone Number")]
			public string PhoneNumber { get; set; }

			[Required(ErrorMessage = "Email is required.")]
			[EmailAddress(ErrorMessage = "Please enter a valid email address.")]
			[Display(Name = "Email")]
			public string Email { get; set; }

			[Required(ErrorMessage = "Password is required.")]
			[StringLength(100, ErrorMessage = "The password must be at least {2} characters long.", MinimumLength = 6)]
			[DataType(DataType.Password)]
			[Display(Name = "Password")]
			public string Password { get; set; }

			[DataType(DataType.Password)]
			[Display(Name = "Confirm password")]
			[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
			public string ConfirmPassword { get; set; }

			[Range(typeof(bool), "true", "true", ErrorMessage = "You must accept the Terms and Conditions to register.")]
			[Display(Name = "I agree to the Terms and Conditions")]
			public bool AcceptTerms { get; set; }
		}
	
	public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
