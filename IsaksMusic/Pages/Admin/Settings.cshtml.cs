using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using IsaksMusic.Data;
using IsaksMusic.Pages.Account.Manage;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace IsaksMusic.Pages.Admin
{
    public class SettingsModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<ChangePasswordModel> _logger;

        public SettingsModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ILogger<ChangePasswordModel> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        [TempData]
        public string Message { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        /* Password */
        [BindProperty]
        public PasswordModel Password { get; set; }

        /* Email */
        [BindProperty]
        public EmailModel Email { get; set; }

        /* Username */
        [BindProperty]
        public UsernameModel Username { get; set; }

        public void OnGet()
        {
            ErrorMessage = null;
        }

        /// <summary>
        /// Change password
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnPostPasswordAsync()
        {
            ModelState.Remove("Username");

            ModelState.Remove("Email");

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, Password.OldPassword, Password.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                foreach (var error in changePasswordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                var errors = ModelState.Select(x => x.Value.Errors)
                           .Where(y => y.Count > 0)
                           .ToList();

                ErrorMessage = errors.FirstOrDefault().FirstOrDefault().ErrorMessage;

                return Page();
            }

            await _signInManager.SignInAsync(user, isPersistent: false);
            _logger.LogInformation("User changed their password successfully.");
            Message = "Your password has been changed.";
            ErrorMessage = null;

            return RedirectToPage();
        }

        /// <summary>
        /// Change email
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnPostEmailAsync()
        {
            ModelState.Remove("Username");

            ModelState.Remove("OldPassword");
            ModelState.Remove("NewPassword");
            ModelState.Remove("ConfirmPassword");

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var setEmailResult = await _userManager.SetEmailAsync(user, Email.Email);
            if (!setEmailResult.Succeeded)
            {
                foreach (var error in setEmailResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                var errors = ModelState.Select(x => x.Value.Errors)
                           .Where(y => y.Count > 0)
                           .ToList();

                ErrorMessage = errors.FirstOrDefault().FirstOrDefault().ErrorMessage;

                return Page();
            }

            _logger.LogInformation("User changed their email successfully.");
            Message = "Your email has been changed.";
            ErrorMessage = null;

            return RedirectToPage();
        }

        /// <summary>
        /// Change username
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnPostUsernameAsync()
        {
            ModelState.Remove("Email");

            ModelState.Remove("OldPassword");
            ModelState.Remove("NewPassword");
            ModelState.Remove("ConfirmPassword");

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var setUsernameResult = await _userManager.SetUserNameAsync(user, Username.Username);

            if (!setUsernameResult.Succeeded)
            {
                foreach (var error in setUsernameResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                var errors = ModelState.Select(x => x.Value.Errors)
                           .Where(y => y.Count > 0)
                           .ToList();

                ErrorMessage = errors.FirstOrDefault().FirstOrDefault().ErrorMessage;

                return Page();
            }

            _logger.LogInformation("User changed their username successfully.");
            Message = "Your username has been changed.";
            ErrorMessage = null;

            return RedirectToPage();
        }

        public class PasswordModel
        {
            [Required(ErrorMessage = "This field is required.")]
            [DataType(DataType.Password)]
            [Display(Name = "Current password")]
            public string OldPassword { get; set; }

            [Required (ErrorMessage = "This field is required.")]
            [StringLength(100, ErrorMessage = "The password must be between {2} and {1} characters long.", MinimumLength = 8)]
            [DataType(DataType.Password)]
            [Display(Name = "New password")]
            public string NewPassword { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm new password")]
            [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }

        public class EmailModel
        {
            [Required(ErrorMessage = "This field is required.")]
            [DataType(DataType.EmailAddress, ErrorMessage = "Not a valid email address.")]
            public string Email { get; set; }
        }

        public class UsernameModel
        {
            [Required(ErrorMessage = "This field is required.")]
            [StringLength(20, ErrorMessage = "The username must be between {2} and {1} characters long.", MinimumLength = 4)]
            public string Username { get; set; }
        }
    }
}