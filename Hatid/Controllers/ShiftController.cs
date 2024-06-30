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
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Microsoft.AspNetCore.Http;
using Hatid.Models;

namespace Hatid.Controllers
{
    [Authorize(Roles = "Manager,Sales")]
    public class ShiftController : BaseController
    {
        private readonly ApplicationDbContext _context;
        public ShiftController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
        {
            _context = context;

        }

        #region Dashboard

        public async Task<IActionResult> Dashboard()
        {
            try
            {
                var shifts = await _context.Shift.Where(x => x.TimeIn.Date >= DateTime.Now.Date && x.TimeIn.Date <= DateTime.Now.Date && x.UserId == CurrentUserId).ToListAsync();
                var user = await _context.User.Where(x => x.Id == CurrentUserId).FirstOrDefaultAsync();
                shifts.ForEach(r =>
                {
                    r.UserName = user.FirstName + ' ' + user.LastName;
                });
                return View(shifts);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> SaveShift([FromForm] Shift shift)
        {
            try
            {
                shift.UserId = CurrentUserId;
                var entry = _context.Shift.FirstOrDefault(e => e.Id == shift.Id);
                if (entry == null)
                {
                    shift.IsCompleted = false;
                    _context.Add(shift);
                }
                else
                {
                    shift.IsCompleted = true;
                    _context.Entry(entry).CurrentValues.SetValues(shift);
                }
                await _context.SaveChangesAsync();
                var user = await _context.User.Where(x => x.Id==shift.UserId).FirstOrDefaultAsync();
                shift.UserName=user.FirstName+' '+user.LastName;
                return Ok(shift);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        #endregion

        #region My Shifts

        public async Task<IActionResult> MyShifts()
        {
            return View();
        }

        public async Task<IActionResult> GetShiftsDeposits(DateTime startDate, DateTime endDate)
        {
            var user = await _context.User.Where(x => x.RoleId==3 && x.Id==CurrentUserId).FirstOrDefaultAsync();

            var depositDict = await _context.Deposit.Where(d => !d.IsDeleted && d.DepositDate.Date >= startDate.Date && d.DepositDate.Date <= endDate.Date && d.UserId==CurrentUserId)
                .GroupBy(d => d.UserId).ToDictionaryAsync(x => x.Key, x => x.ToList());

            var shiftsDict = await _context.Shift.Where(s => !s.IsDeleted && s.TimeIn.Date >= startDate.Date && s.TimeIn.Date <= endDate.Date && s.UserId==CurrentUserId)
              .GroupBy(s => s.UserId).ToDictionaryAsync(x => x.Key, x => x.ToList());

            List<ShiftDeposit> list = new();
            ShiftDeposit shiftDeposit = new();
            shiftDeposit.UserName = user.FirstName+ " " + user.LastName;
            shiftDeposit.UserId = user.Id;
            if (depositDict.ContainsKey(user.Id))
            {
                shiftDeposit.DepositAmount = depositDict[user.Id].Sum(x => x.Amount);
            }
            if (shiftsDict.ContainsKey(user.Id))
            {
                shiftDeposit.ShiftDetail.ShiftCount = shiftsDict[user.Id].Count();
                var totalSeconds = shiftsDict[user.Id].Where(x => x.IsCompleted).Sum(x => (x.TimeOut - x.TimeIn).TotalSeconds);
                shiftDeposit.ShiftDetail.Hours = Convert.ToInt32(totalSeconds / 3600);
                shiftDeposit.ShiftDetail.Minutes = Convert.ToInt32(totalSeconds % 3600 / 60);
            }
            list.Add(shiftDeposit);

            return Ok(list);
        }

        public async Task<IActionResult> GetDepositsByUser(DateTime startDate, DateTime endDate)
        {
            var deposits = await _context.Deposit.Where(x => x.DepositDate.Date >= startDate.Date && x.DepositDate.Date <= endDate.Date && x.UserId==CurrentUserId).ToListAsync();
            var user = await _context.User.Where(x => x.Id == CurrentUserId).FirstOrDefaultAsync();
            deposits.ForEach(r =>
            {
                r.UserName = user.FirstName+' '+user.LastName;
            });
            return Ok(deposits);
        }

        public async Task<IActionResult> GetShiftsByUser(DateTime startDate, DateTime endDate)
        {
            var shifts = await _context.Shift.Where(x => x.TimeIn.Date >= startDate.Date && x.TimeIn.Date <= endDate.Date && x.UserId==CurrentUserId).ToListAsync();
            var user = await _context.User.Where(x => x.Id == CurrentUserId).FirstOrDefaultAsync();
            shifts.ForEach(r =>
            {
                r.UserName = user.FirstName+' '+user.LastName;
            });
            return Ok(shifts);
        }
        #endregion
    }
}
