using System;

namespace SlotBooker.Services.Models
{
    public class FindSlotResult
    {
        public bool Succeeded { get; set; }
        public DateTime? DateBooked { get; set; }
    }
}
