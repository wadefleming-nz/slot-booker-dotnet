using SlotBooker.Services;
using SlotBooker.Services.Models;
using System;
using System.Windows.Forms;
using System.Linq;

namespace SlotBooker.UI
{
    public partial class BookSlotForm : Form
    {
        public BookSlotForm()
        {
            InitializeComponent();

            monthCalendar.MinDate = DateTime.Today;   
        }

        private void findButton_Click(object sender, EventArgs e)
        {
            if (this.ValidateChildren())
            {
                SlotFinder slotFinder = new SlotFinder();
                var findSlotParams = new FindSlotParams
                {
                    Email = emailAddressTextBox.Text,
                    Password = passwordTextBox.Text,
                    Dates = selectedDatesListBox.Items.Cast<DateTime>().ToList()
                };

                var result = slotFinder.FindSlot(findSlotParams);
                var resultText = result.Succeeded 
                    ? $"Slot was successfully booked for { result.DateBooked.Value.ToString("d") }" 
                    : "Slot was not booked";
                resultLabel.Text = resultText;
            }
        }

        private void monthCalendar_DateSelected(object sender, DateRangeEventArgs e)
        {
            selectedDatesListBox.Items.Add(e.Start);
        }

        private void emailAddressTextBox_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(string.IsNullOrEmpty(emailAddressTextBox.Text))
            {
                e.Cancel = true;
                errorProvider.SetError(emailAddressTextBox, "Email must be specified");
            }
        }

        private void passwordTextBox_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(passwordTextBox.Text))
            {
                e.Cancel = true;
                errorProvider.SetError(passwordTextBox, "Password must be specified");
            }
        }

        private void selectedDatesListBox_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(selectedDatesListBox.Items.Count == 0)
            {
                e.Cancel = true;
                errorProvider.SetError(selectedDatesListBox, "Select at least one date");
            }
        }
    }
}
