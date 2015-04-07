using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace hazi.WEB.Logic
{
    public class DateYearMonth
    {
        int year;

        public int Year
        {
            get { return year; }
        }
        int month;

        public int Month
        {
            get { return month; }
        }

        public DateYearMonth(int year, int month)
        {
            this.year = year;
            this.month = month;
        }

        public override string ToString()
        {
            return year + "/" + month;
        }
    }
}