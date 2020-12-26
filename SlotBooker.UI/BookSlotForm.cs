using SlotBooker.Services;
using SlotBooker.Services.Models;
using System;
using System.Windows.Forms;

namespace SlotBooker.UI
{
    public partial class BookSlotForm : Form
    {
        DateTime selectedDate;

        public BookSlotForm()
        {
            InitializeComponent();

            // defaults
            emailAddressTextBox.Text = "wadefleming@yahoo.com";
            passwordTextBox.Text = "Dembava12345";

            // ensure selected date is consistent with displayed date
            var today = DateTime.Today;
            monthCalendar.SetDate(today);
            selectedDate = today;       
        }

        private void findButton_Click(object sender, EventArgs e)
        {
            SlotFinder slotFinder = new SlotFinder();
            var findSlotParams = new FindSlotParams
            {
                Email = emailAddressTextBox.Text,
                Password = passwordTextBox.Text,
                Date = selectedDate
            };

            slotFinder.FindSlot(findSlotParams);
        }

        private void monthCalendar_DateSelected(object sender, DateRangeEventArgs e)
        {
            selectedDate = e.Start;
        }
    }
}
