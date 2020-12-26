using System;
using System.Collections.Generic;

namespace SlotBooker.Services.Models
{
    public class FindSlotParams
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public List<DateTime> Dates { get; set; }
    }
}
