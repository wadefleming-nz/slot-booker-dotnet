using System;
using System.Globalization;

namespace SlotBooker.Services.Utils
{
    public class DateFormatter
    {
        public readonly DateTime Date;
        private const string english = "en-GB"; // ensure date naming matches target website

        public string MonthString => Date.ToString("MMMM", CultureInfo.GetCultureInfo(english));
        public string DateString => Date.ToString("MMMM dd, yyyy", CultureInfo.GetCultureInfo(english));

        public DateFormatter(DateTime date)
        {
            this.Date = date;
        }
    }
}
