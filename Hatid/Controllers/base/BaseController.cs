using Hatid.Data;
using Hatid.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Hatid.Controllers
{
    public class BaseController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public BaseController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;

        }

        public int CurrentUserId
        {
            get
            {
                int userId = 0;
                if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
                {
                    var uStr = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                    int.TryParse(uStr, out userId);
                }
                return userId;
            }
        }

        public async Task<List<User>> GetUserList()
        {
            var users = await _context.User.ToListAsync();
            return users;
        }
        public static User GetValidUserById(int userId, Dictionary<int, User> usersDict)
        {
            User ret = new();
            if (usersDict.ContainsKey(userId))
            {
                ret = usersDict[userId];
            }
            return ret;

        }
    }
}
