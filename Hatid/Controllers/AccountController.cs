using Hatid.Data;
using Hatid.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Hatid.Areas.Identity.Pages.Account;
using Microsoft.AspNetCore.WebUtilities;
using System.Text.Encodings.Web;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace Hatid.Controllers
{
    public class AccountController : BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IUserStore<User> _userStore;
        private readonly IUserEmailStore<User> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        public AccountController(ApplicationDbContext context, UserManager<User> userManager,
            IUserStore<User> userStore,
            SignInManager<User> signInManager,
        ILogger<RegisterModel> logger,
            IEmailSender emailSender, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
        {
            _context = context;
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        #region User

        private User CreateUser()
        {
            try
            {
                return Activator.CreateInstance<User>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(User)}'. " +
                    $"Ensure that '{nameof(User)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }
        private IUserEmailStore<User> GetEmailStore()
        {
            return (IUserEmailStore<User>)_userStore;
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Users()
        {
            try
            {
                var users = await _context.User.ToListAsync();
                return View(users);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> GetUserByUserId([FromForm] int userId)
        {
            try
            {
                var user = await _context.User.FindAsync(userId)?? new User();
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> SaveUser([FromForm] User user)
        {
            try
            {
                var entry = _context.User.FirstOrDefault(u => u.Id == user.Id);
                if (entry == null)
                {
                    await registerUser(user);
                }
                else
                {
                    _context.Entry(entry).CurrentValues.SetValues(user);
                }
                await _context.SaveChangesAsync();
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        private async Task<User> registerUser(User user)
        {
            var userData = CreateUser();
            await _userStore.SetUserNameAsync(userData, user.Email, CancellationToken.None);
            await _emailStore.SetEmailAsync(userData, user.Email, CancellationToken.None);

            userData.FirstName = user.FirstName;
            userData.LastName = user.LastName;
            userData.PhoneNumber = user.PhoneNumber;
            userData.RoleId = user.RoleId;
            userData.IsActive = true;
            userData.PhoneNumber = user.PhoneNumber;
            userData.TwoFactorEnabled=user.TwoFactorEnabled;
            userData.EmailConfirmed = true;
            if (user.IsSendEmailInvite)
            {
                user.Password="Pass@123";
            }
            var result = await _userManager.CreateAsync(userData, user.Password);

            if (result.Succeeded)
            {
                //assign user role
                await _userManager.AddToRoleAsync(userData, ((EnumRole)user.RoleId).ToString());
                if (user.IsSendEmailInvite)
                {
                    var code = await _userManager.GeneratePasswordResetTokenAsync(userData);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ResetPassword",
                        pageHandler: null,
                        values: new { area = "Identity", code },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(
                        userData.Email,
                        "Reset Password",
                        $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                }

                return userData;
            }
            return null;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> DeleteUser([FromForm] int userId)
        {
            try
            {
                var entity = await _context.User.FindAsync(userId);
                if (entity != null)
                {
                    _context.Remove(entity);
                }
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }

        [HttpPost]
        public async Task<IActionResult> IsValidEmail([FromForm] int id, [FromForm] string email)
        {
            try
            {

                var users = await IsValidEmail(email);
                return Ok(!(users.Where(x => x.Id != id).Any()));
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> IsValidPhoneNumber([FromForm] int id, [FromForm] string phoneNumber)
        {
            try
            {
                var users = await IsValidPhoneNumber(phoneNumber);
                return Ok(!(users.Where(x => x.Id != id).Any()));
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        private async Task<List<User>> IsValidEmail(string email)
        {
            if (string.IsNullOrEmpty(email)) { return null; }
            return await _context.User.Where(x => x.Email == email && !x.IsDeleted).ToListAsync();
        }

        private async Task<List<User>> IsValidPhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber)) { return null; }
            return await _context.User.Where(x => x.PhoneNumber == phoneNumber && !x.IsDeleted).ToListAsync();
        }
     
        #endregion

    }
}
