using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using EduAcess.Models;
using EduAcess.Services;
namespace EduAcess.Controllers
{
	[Authorize]
	public class AccountController : Controller
	{
		private ApplicationSignInManager _signInManager;
		private ApplicationUserManager _userManager;

		public AccountController()
		{
		}

		public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
		{
			UserManager = userManager;
			SignInManager = signInManager;
		}

		public ApplicationSignInManager SignInManager
		{
			get
			{
				return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
			}
			private set { _signInManager = value; }
		}

		public ApplicationUserManager UserManager
		{
			get
			{
				return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
			}
			private set { _userManager = value; }
		}

		// GET: /Account/Login
		[AllowAnonymous]
		public ActionResult Login(string returnUrl)
		{
			ViewBag.ReturnUrl = returnUrl;
			return View(new LoginViewModel());
		}

		// POST: /Account/Login
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			// 1. Block unverified users BEFORE password check
			var user = await UserManager.FindByEmailAsync(model.Email);
			if (user != null && !user.EmailConfirmed)
			{
				ModelState.AddModelError("", "You must confirm your email before logging in. Please check your inbox for the confirmation link.");
				return View(model);
			}

			var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
			switch (result)
			{
				case SignInStatus.Success:
					// 2. Force password change if flagged (e.g., seeded admin)
					if (user != null && user.MustChangePassword)
					{
						return RedirectToAction("ChangePassword", "Manage");
					}

					// 3. Redirect Admins straight to the Admin Dashboard
					if (user != null && UserManager.IsInRole(user.Id, "Admin"))
					{
						return RedirectToAction("Index", "AdminDashboard");
					}

					// 4. Regular learners go to their Learner Dashboard
					if (string.IsNullOrEmpty(returnUrl))
					{
						return RedirectToAction("Index", "Dashboard");
					}
					return RedirectToLocal(returnUrl);

				case SignInStatus.LockedOut:
					return View("Lockout");

				case SignInStatus.RequiresVerification:
					return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });

				case SignInStatus.Failure:
				default:
					ModelState.AddModelError("", "Invalid login attempt.");
					return View(model);
			}
		}

		// GET: /Account/VerifyCode
		[AllowAnonymous]
		public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
		{
			if (!await SignInManager.HasBeenVerifiedAsync())
			{
				return View("Error");
			}
			return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
		}

		// POST: /Account/VerifyCode
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
			switch (result)
			{
				case SignInStatus.Success:
					return RedirectToLocal(model.ReturnUrl);
				case SignInStatus.LockedOut:
					return View("Lockout");
				case SignInStatus.Failure:
				default:
					ModelState.AddModelError("", "Invalid code.");
					return View(model);
			}
		}

		// GET: /Account/Register
		[AllowAnonymous]
		public ActionResult Register()
		{
			return View(new RegisterViewModel());
		}


		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Register(RegisterViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			var user = new ApplicationUser
			{
				UserName = model.Email,
				Email = model.Email,
				EmailConfirmed = false,
				FirstName = model.FirstName,
				Surname = model.Surname,
				FullName = model.FirstName + " " + model.Surname,
				PhoneNumber = model.PhoneNumber
			};

			var result = await UserManager.CreateAsync(user, model.Password);

			if (!result.Succeeded)
			{
				AddErrors(result);
				return View(model);
			}

			// Assign the new user to the Learner role.
			var roleResult = await UserManager.AddToRoleAsync(
				user.Id, "Learner");

			if (!roleResult.Succeeded)
			{
				System.Diagnostics.Trace.TraceError(
					"Could not assign Learner role to user {0}: {1}",
					user.Id, string.Join("; ", roleResult.Errors));

				TempData["Error"] =
					"Your account was created, but setup was incomplete. Please contact support.";

				return RedirectToAction("Login");
			}

			// Generate a six-digit verification code.
			string code = GenerateVerificationCode();

			user.VerificationCode = code;

			var updateResult = await UserManager.UpdateAsync(user);

			if (!updateResult.Succeeded)
			{
				System.Diagnostics.Trace.TraceError(
					"Could not save verification code for user {0}: {1}",
					user.Id, string.Join("; ", updateResult.Errors));

				TempData["Error"] =
					"Your account was created, but verification setup failed. Please contact support.";

				return RedirectToAction("Login");
			}

			try
			{
				string firstName = System.Web.HttpUtility.HtmlEncode(
					model.FirstName ?? "Learner");

				string emailBody = $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1'>
    <title>Verify your EduAccess account</title>
</head>
<body style='margin:0;padding:0;background:#f4f7f6;
             font-family:Arial,sans-serif;color:#243b3a;'>
    <div style='max-width:600px;margin:30px auto;background:#ffffff;
                border-radius:12px;overflow:hidden;'>
        <div style='background:#0d7a6e;padding:28px;text-align:center;'>
            <h1 style='margin:0;color:#ffffff;'>EduAccess</h1>
            <p style='margin:8px 0 0;color:#e4f5f1;'>
                Your pathway to higher education
            </p>
        </div>

        <div style='padding:32px;'>
            <h2>Hello {firstName},</h2>

            <p>
                Thank you for creating your EduAccess account.
                Enter the verification code below to confirm your email address.
            </p>

            <div style='margin:28px 0;text-align:center;'>
                <div style='display:inline-block;background:#e8f5f2;
                            color:#0d7a6e;padding:18px 28px;
                            border-radius:8px;font-size:32px;
                            font-weight:bold;letter-spacing:8px;'>
                    {code}
                </div>
            </div>

            <p>
                If you did not create this account, you can ignore this email.
            </p>

            <p style='margin-top:28px;'>
                Regards,<br>
                <strong>The EduAccess Team</strong>
            </p>
        </div>
    </div>
</body>
</html>";

				// Send using YOUR EmailService.
				await EmailService.SendAsync(
					user.Email,
					"Verify your EduAccess account",
					emailBody);

				TempData["Success"] =
					"Your account was created. Check your inbox for your verification code.";
			}
			catch (Exception ex)
			{
				System.Diagnostics.Trace.TraceError(
					"Registration email failed for {0}: {1}",
					user.Email, ex);

				TempData["Error"] =
					"Your account was created, but the verification email could not be sent. Try resending the code.";
			}

			return RedirectToAction(
				"ConfirmEmailCode",
				"Account",
				new { email = user.Email });
		}

		private static string GenerateVerificationCode()
		{
			// Cryptographically generated six-digit code.
			using (var rng = System.Security.Cryptography.RandomNumberGenerator.Create())
			{
				byte[] bytes = new byte[4];
				rng.GetBytes(bytes);

				uint value = BitConverter.ToUInt32(bytes, 0);
				int code = (int)(value % 900000) + 100000;

				return code.ToString(
					System.Globalization.CultureInfo.InvariantCulture);
			}
		}



		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> ResendCode(string email)
		{
			if (string.IsNullOrWhiteSpace(email))
			{
				TempData["Error"] = "Please provide your email address.";
				return RedirectToAction("Login");
			}

			var user = await UserManager.FindByEmailAsync(email.Trim());

			if (user == null || user.EmailConfirmed)
			{
				// Avoid disclosing account details.
				TempData["Info"] =
					"If your account requires verification, follow the instructions provided.";
				return RedirectToAction("Login");
			}

			string code = GenerateVerificationCode();

			user.VerificationCode = code;

			var updateResult = await UserManager.UpdateAsync(user);

			if (!updateResult.Succeeded)
			{
				System.Diagnostics.Trace.TraceError(
					"Could not update verification code for {0}: {1}",
					user.Email, string.Join("; ", updateResult.Errors));

				TempData["Error"] =
					"We could not generate a new code. Please try again.";

				return RedirectToAction(
					"ConfirmEmailCode",
					new { email = user.Email });
			}

			try
			{
				string emailBody = $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'>
</head>
<body style='font-family:Arial,sans-serif;background:#f4f7f6;
             padding:24px;color:#243b3a;'>
    <div style='max-width:560px;margin:auto;background:#ffffff;
                padding:30px;border-radius:12px;'>
        <h2 style='color:#0d7a6e;'>Your new EduAccess verification code</h2>

        <p>Use the code below to verify your email address.</p>

        <div style='text-align:center;margin:28px 0;'>
            <span style='display:inline-block;background:#e8f5f2;
                         color:#0d7a6e;padding:18px 26px;
                         font-size:30px;font-weight:bold;
                         letter-spacing:8px;border-radius:8px;'>
                {code}
            </span>
        </div>

        <p>This code replaces your previous verification code.</p>
        <p>Regards,<br><strong>The EduAccess Team</strong></p>
    </div>
</body>
</html>";

				await EmailService.SendAsync(
					user.Email,
					"Your new EduAccess verification code",
					emailBody);

				TempData["Success"] =
					"A new verification code has been sent to your email.";
			}
			catch (Exception ex)
			{
				System.Diagnostics.Trace.TraceError(
					"Resend verification email failed for {0}: {1}",
					user.Email, ex);

				TempData["Error"] =
					"The email could not be sent. Please try again later.";
			}

			return RedirectToAction(
				"ConfirmEmailCode",
				new { email = user.Email });
		}

		// GET: /Account/ConfirmEmail
		[AllowAnonymous]
		public async Task<ActionResult> ConfirmEmail(string userId, string code)
		{
			if (userId == null || code == null)
			{
				return View("Error");
			}
			var result = await UserManager.ConfirmEmailAsync(userId, code);
			return View(result.Succeeded ? "ConfirmEmail" : "Error");
		}

		// GET: /Account/ForgotPassword
		[AllowAnonymous]
		public ActionResult ForgotPassword()
		{
			return View(new ForgotPasswordViewModel());
		}


		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> ForgotPassword(
			ForgotPasswordViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			var user = await UserManager.FindByNameAsync(model.Email);

			if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
			{
				// Keep the response the same whether or not the account exists.
				return RedirectToAction(
					"ForgotPasswordConfirmation",
					"Account");
			}

			string code = await UserManager.GeneratePasswordResetTokenAsync(
				user.Id);

			string callbackUrl = Url.Action(
				"ResetPassword",
				"Account",
				new { userId = user.Id, code = code },
				protocol: Request.Url.Scheme);

			string safeUrl = System.Web.HttpUtility.HtmlAttributeEncode(
				callbackUrl ?? string.Empty);

			string emailBody = $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'>
</head>
<body style='font-family:Arial,sans-serif;background:#f4f7f6;
             padding:24px;color:#243b3a;'>
    <div style='max-width:560px;margin:auto;background:#ffffff;
                padding:30px;border-radius:12px;'>
        <h2 style='color:#0d7a6e;'>Reset your EduAccess password</h2>

        <p>We received a request to reset your EduAccess password.</p>

        <p style='margin:28px 0;'>
            <a href='{safeUrl}'
               style='background:#0d7a6e;color:#ffffff;
                      padding:14px 22px;text-decoration:none;
                      border-radius:6px;display:inline-block;'>
                Reset my password
            </a>
        </p>

        <p>If you did not request a password reset, you can ignore this email.</p>
        <p>Regards,<br><strong>The EduAccess Team</strong></p>
    </div>
</body>
</html>";

			try
			{
				await EmailService.SendAsync(
					user.Email,
					"Reset your EduAccess password",
					emailBody);
			}
			catch (Exception ex)
			{
				System.Diagnostics.Trace.TraceError(
					"Password reset email failed for {0}: {1}",
					user.Email, ex);

				// Do not expose SMTP details or whether the account exists.
			}

			return RedirectToAction(
				"ForgotPasswordConfirmation",
				"Account");
		}


		// GET: /Account/ForgotPasswordConfirmation
		[AllowAnonymous]
		public ActionResult ForgotPasswordConfirmation()
		{
			return View();
		}

		// GET: /Account/ResetPassword
		[AllowAnonymous]
		public ActionResult ResetPassword(string code)
		{
			return code == null ? View("Error") : View(new ResetPasswordViewModel());
		}

		// POST: /Account/ResetPassword
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}
			var user = await UserManager.FindByNameAsync(model.Email);
			if (user == null)
			{
				return RedirectToAction("ResetPasswordConfirmation", "Account");
			}
			var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
			if (result.Succeeded)
			{
				return RedirectToAction("ResetPasswordConfirmation", "Account");
			}
			AddErrors(result);
			return View();
		}

		// GET: /Account/ResetPasswordConfirmation
		[AllowAnonymous]
		public ActionResult ResetPasswordConfirmation()
		{
			return View();
		}

		// POST: /Account/ExternalLogin
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public ActionResult ExternalLogin(string provider, string returnUrl)
		{
			return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
		}

		// GET: /Account/SendCode
		[AllowAnonymous]
		public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
		{
			var userId = await SignInManager.GetVerifiedUserIdAsync();
			if (userId == null)
			{
				return View("Error");
			}
			var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
			var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
			return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
		}

		// POST: /Account/SendCode
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> SendCode(SendCodeViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View();
			}

			if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
			{
				return View("Error");
			}
			return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
		}

		// GET: /Account/ExternalLoginCallback
		[AllowAnonymous]
		public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
		{
			var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
			if (loginInfo == null)
			{
				return RedirectToAction("Login");
			}

			var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
			switch (result)
			{
				case SignInStatus.Success:
					return RedirectToLocal(returnUrl);
				case SignInStatus.LockedOut:
					return View("Lockout");
				case SignInStatus.RequiresVerification:
					return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
				case SignInStatus.Failure:
				default:
					ViewBag.ReturnUrl = returnUrl;
					ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
					return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
			}
		}
		// GET: /Account/ConfirmEmailCode?email=...
		[AllowAnonymous]
		public ActionResult ConfirmEmailCode(string email)
		{
			if (string.IsNullOrEmpty(email))
			{
				return RedirectToAction("Login");
			}
			ViewBag.Email = email;
			return View();
		}

		// POST: /Account/ConfirmEmailCode
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> ConfirmEmailCode(string email, string code)
		{
			ViewBag.Email = email;

			if (string.IsNullOrEmpty(code) || code.Length != 6)
			{
				ModelState.AddModelError("", "Please enter the 6-digit code.");
				return View();
			}

			var user = await UserManager.FindByEmailAsync(email);
			if (user == null)
			{
				ModelState.AddModelError("", "User not found. Please register again.");
				return View();
			}

			if (user.VerificationCode != code)
			{
				ModelState.AddModelError("", "Invalid code. Please check the code in your console output.");
				return View();
			}

			// Mark email as confirmed
			user.EmailConfirmed = true;
			user.VerificationCode = null;
			await UserManager.UpdateAsync(user);

			// Sign them in
			await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

			// Go to Dashboard
			return RedirectToAction("Index", "Dashboard");
		}
		// POST: /Account/ExternalLoginConfirmation
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
		{
			if (User.Identity.IsAuthenticated)
			{
				return RedirectToAction("Index", "Manage");
			}

			if (ModelState.IsValid)
			{
				var info = await AuthenticationManager.GetExternalLoginInfoAsync();
				if (info == null)
				{
					return View("ExternalLoginFailure");
				}
				var user = new ApplicationUser
				{
					UserName = model.Email,
					Email = model.Email,
					EmailConfirmed = true // AUTO-CONFIRM
				};
				var result = await UserManager.CreateAsync(user);
				if (result.Succeeded)
				{
					await UserManager.AddToRoleAsync(user.Id, "Learner");

					result = await UserManager.AddLoginAsync(user.Id, info.Login);
					if (result.Succeeded)
					{
						await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
						return RedirectToAction("Index", "Dashboard");
					}
				}
				AddErrors(result);
			}

			ViewBag.ReturnUrl = returnUrl;
			return View(model);
		}
		// POST: /Account/FaceLogin
		[HttpPost]
		[AllowAnonymous]
		public async Task<ActionResult> FaceLogin(double[] currentDescriptor)
		{
			if (currentDescriptor == null || currentDescriptor.Length != 128)
				return Json(new { success = false, message = "Invalid face data" });

			// Get all users who have registered a face
			var usersWithFaces = UserManager.Users.Where(u => u.FaceDescriptor != null).ToList();

			ApplicationUser matchedUser = null;
			double bestDistance = double.MaxValue;

			foreach (var user in usersWithFaces)
			{
				// Convert string back to array
				var savedDescriptor = Newtonsoft.Json.JsonConvert.DeserializeObject<double[]>(user.FaceDescriptor);

				// Calculate Euclidean distance (lower is better, < 0.6 is usually a match)
				double distance = 0;
				for (int i = 0; i < 128; i++)
				{
					distance += Math.Pow(savedDescriptor[i] - currentDescriptor[i], 2);
				}
				distance = Math.Sqrt(distance);

				if (distance < 0.6 && distance < bestDistance) // 0.6 is the threshold
				{
					bestDistance = distance;
					matchedUser = user;
				}
			}

			if (matchedUser != null)
			{
				// Log the user in
				await SignInManager.SignInAsync(matchedUser, isPersistent: false, rememberBrowser: false);
				return Json(new { success = true, redirectUrl = Url.Action("Index", "Dashboard") });
			}

			return Json(new { success = false });
		}
		// POST: /Account/LogOff
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult LogOff()
		{
			AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
			return RedirectToAction("Index", "Home");
		}

		// GET: /Account/ExternalLoginFailure
		[AllowAnonymous]
		public ActionResult ExternalLoginFailure()
		{
			return View();
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (_userManager != null)
				{
					_userManager.Dispose();
					_userManager = null;
				}

				if (_signInManager != null)
				{
					_signInManager.Dispose();
					_signInManager = null;
				}
			}

			base.Dispose(disposing);
		}

		#region Helpers
		private const string XsrfKey = "XsrfId";

		private IAuthenticationManager AuthenticationManager
		{
			get
			{
				return HttpContext.GetOwinContext().Authentication;
			}
		}

		private void AddErrors(IdentityResult result)
		{
			foreach (var error in result.Errors)
			{
				ModelState.AddModelError("", error);
			}
		}

		private ActionResult RedirectToLocal(string returnUrl)
		{
			if (Url.IsLocalUrl(returnUrl))
			{
				return Redirect(returnUrl);
			}
			return RedirectToAction("Index", "Home");
		}

		internal class ChallengeResult : HttpUnauthorizedResult
		{
			public ChallengeResult(string provider, string redirectUri)
				: this(provider, redirectUri, null)
			{
			}

			public ChallengeResult(string provider, string redirectUri, string userId)
			{
				LoginProvider = provider;
				RedirectUri = redirectUri;
				UserId = userId;
			}

			public string LoginProvider { get; set; }
			public string RedirectUri { get; set; }
			public string UserId { get; set; }

			public override void ExecuteResult(ControllerContext context)
			{
				var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
				if (UserId != null)
				{
					properties.Dictionary[XsrfKey] = UserId;
				}
				context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
			}
		}
		#endregion
	}
}