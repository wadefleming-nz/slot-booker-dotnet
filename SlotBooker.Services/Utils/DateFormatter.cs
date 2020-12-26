using System;
using System.Globalization;

namespace SlotBooker.Services.Utils
{
    public class DateFormatter
    {
        private readonly DateTime date;
        private const string english = "en-GB"; // ensure date naming matches target website

        public string MonthString => date.ToString("MMMM", CultureInfo.GetCultureInfo(english));
        public string DateString => date.ToString("MMMM dd, yyyy", CultureInfo.GetCultureInfo(english));

        public DateFormatter(DateTime date)
        {
            this.date = date;
        }
    }
}
