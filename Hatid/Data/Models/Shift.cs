using Hatid.Data.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Hatid.Data.Models
{
    public class Shift : BaseEntity
    {
        public DateTime TimeIn { get; set; }
        private int _timeinyear;
        public int TimeInYear
        {
            get { return _timeinyear; }
            set
            {
                _timeinyear = Convert.ToInt32(TimeIn.Year);
            }
        }
        private int _timeinmonth;
        public int TimeInMonth
        {
            get { return _timeinmonth; }
            set
            {
                _timeinmonth = Convert.ToInt32(TimeIn.Month);
            }
        }
        private int _timeinday;
        public int TimeInDay
        {
            get { return _timeinday; }
            set
            {
                _timeinday = Convert.ToInt32(TimeIn.Day);
            }
        }
        private int _timeinhour;
        public int TimeInHour
        {
            get { return _timeinhour; }
            set
            {
                _timeinhour = Convert.ToInt32(TimeIn.Hour);
            }
        }
        private int _timeinminute;
        public int TimeInMinute
        {
            get { return _timeinminute; }
            set
            {
                _timeinminute = Convert.ToInt32(TimeIn.Minute);
            }
        }
        private int _timeinsecond;
        public int TimeInSecond
        {
            get { return _timeinsecond; }
            set
            {
                _timeinsecond = Convert.ToInt32(TimeIn.Second);
            }
        }

        public DateTime TimeOut { get; set; }
        private int _timeoutyear;
        public int TimeOutYear
        {
            get { return _timeoutyear; }
            set
            {
                _timeoutyear = Convert.ToInt32(TimeOut.Year);
            }
        }
        private int _timeoutmonth;
        public int TimeOutMonth
        {
            get { return _timeoutmonth; }
            set
            {
                _timeoutmonth = Convert.ToInt32(TimeOut.Month);
            }
        }
        private int _timeoutday;
        public int TimeOutDay
        {
            get { return _timeoutday; }
            set
            {
                _timeoutday = Convert.ToInt32(TimeOut.Day);
            }
        }
        private int _timeouthour;
        public int TimeOutHour
        {
            get { return _timeouthour; }
            set
            {
                _timeouthour = Convert.ToInt32(TimeOut.Hour);
            }
        }
        private int _timeoutminute;
        public int TimeOutMinute
        {
            get { return _timeoutminute; }
            set
            {
                _timeoutminute = Convert.ToInt32(TimeOut.Minute);
            }
        }
        private int _timeoutsecond;
        public int TimeOutSecond
        {
            get { return _timeoutsecond; }
            set
            {
                _timeoutsecond = Convert.ToInt32(TimeOut.Second);
            }
        }
        public int UserId { get; set; }
        public bool IsCompleted { get; set; }
        [NotMapped]
        public string UserName { get; set; }
    }
}
