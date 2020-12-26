using SlotBooker.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SlotBooker.UI
{
    public partial class BookSlotForm : Form
    {
        DateTime selectedDate;

        public BookSlotForm()
        {
            InitializeComponent();

            // ensure selected date is consistent with displayed date
            var today = DateTime.Today;
            monthCalendar.SetDate(today);
            selectedDate = today;       
        }

        private void findSlotButton_Click(object sender, EventArgs e)
        {
            SlotFinder slotFinder = new SlotFinder();      
            slotFinder.FindSlot(selectedDate);
        }

        private void monthCalendar_DateSelected(object sender, DateRangeEventArgs e)
        {
            selectedDate = e.Start;
        }
    }
}
