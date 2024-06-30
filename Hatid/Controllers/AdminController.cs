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
using Hatid.Migrations;
using Hatid.Models;

namespace Hatid.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : BaseController
    {
        private readonly ApplicationDbContext _context;
       
        public AdminController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
        {
            _context = context;
        }

        #region Shifts
        [HttpPost]
        public async Task<IActionResult> SaveShift([FromForm] Shift shift)
        {
            try
            {
                var entry = _context.Shift.FirstOrDefault(e => e.Id == shift.Id);
                if (entry == null)
                {
                    _context.Add(shift);
                }
                else
                {
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

        [HttpPost]
        public async Task<IActionResult> DeleteShift([FromForm] int shiftId)
        {
            try
            {
                var entity = await _context.Shift.FindAsync(shiftId);
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
        #endregion

        #region Deposits
        [HttpPost]
        public async Task<IActionResult> SaveDeposit([FromForm] Deposit deposit)
        {
            try
            {
                var entry = _context.Deposit.FirstOrDefault(e => e.Id == deposit.Id);
                if (entry == null)
                {
                    _context.Add(deposit);
                }
                else
                {
                    _context.Entry(entry).CurrentValues.SetValues(deposit);
                }
                await _context.SaveChangesAsync();
                var user = await _context.User.Where(x => x.Id == deposit.UserId).FirstOrDefaultAsync();
                deposit.UserName = user.FirstName + ' ' + user.LastName;
                return Ok(deposit);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteDeposit([FromForm] int depositId)
        {
            try
            {
                var entity = await _context.Deposit.FindAsync(depositId);
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
        #endregion

        #region Dashboard
        public async Task<IActionResult> Dashboard()
        {
            var users = await _context.User.Where(x => !x.IsDeleted).ToListAsync();
            return View(users);
        }

        public async Task<IActionResult> GetShiftsDeposits(DateTime startDate, DateTime endDate)
        {
            var users = await _context.User.Where(x=>x.RoleId==3).ToListAsync();
           
            var depositDict = await _context.Deposit.Where(d => !d.IsDeleted && d.DepositDate.Date >= startDate.Date && d.DepositDate.Date <= endDate.Date)
                .GroupBy(d => d.UserId).ToDictionaryAsync(x => x.Key, x => x.ToList());

            var shiftsDict = await _context.Shift.Where(s => !s.IsDeleted && s.TimeIn.Date >= startDate.Date && s.TimeIn.Date <= endDate.Date)
              .GroupBy(s => s.UserId).ToDictionaryAsync(x => x.Key, x => x.ToList()); 
           
            List<ShiftDeposit> list = new();
            users.ForEach(u =>
            {
                ShiftDeposit shiftDeposit = new();
                shiftDeposit.UserName = u.FirstName+ " " + u.LastName;
                shiftDeposit.UserId = u.Id;
                if (depositDict.ContainsKey(u.Id))
                {
                    shiftDeposit.DepositAmount = depositDict[u.Id].Sum(x => x.Amount);
                }
                if (shiftsDict.ContainsKey(u.Id))
                {
                    shiftDeposit.ShiftDetail.ShiftCount = shiftsDict[u.Id].Count();
                    var totalSeconds = shiftsDict[u.Id].Where(x => x.IsCompleted).Sum(x => (x.TimeOut - x.TimeIn).TotalSeconds);
                    shiftDeposit.ShiftDetail.Hours = Convert.ToInt32(totalSeconds / 3600);
                    shiftDeposit.ShiftDetail.Minutes = Convert.ToInt32(totalSeconds % 3600 / 60);
                }
                list.Add(shiftDeposit);
            });

            return Ok(list);
        }
    
        public async Task<IActionResult> GetDepositsByUser(DateTime startDate, DateTime endDate,int userId)
        {
            var deposits = await _context.Deposit.Where(x => x.DepositDate.Date >= startDate.Date && x.DepositDate.Date <= endDate.Date && x.UserId==userId).ToListAsync();
            var user = await _context.User.Where(x => x.Id == userId).FirstOrDefaultAsync();
            deposits.ForEach(r =>
            {
                r.UserName = user.FirstName+' '+user.LastName;
            });
            return Ok(deposits);
        }

        public async Task<IActionResult> GetShiftsByUser(DateTime startDate, DateTime endDate, int userId)
        {
            var shifts = await _context.Shift.Where(x => x.TimeIn.Date >= startDate.Date && x.TimeIn.Date <= endDate.Date && x.UserId==userId).ToListAsync();
            var user = await _context.User.Where(x => x.Id == userId).FirstOrDefaultAsync();
            shifts.ForEach(r =>
            {
                r.UserName = user.FirstName+' '+user.LastName;
            });
            return Ok(shifts);
        }

        #endregion

    }
}
