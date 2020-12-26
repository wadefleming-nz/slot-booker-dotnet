using System;

namespace SlotBooker.Services.Models
{
    public class FindSlotParams
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime Date { get; set; }
    }
}
