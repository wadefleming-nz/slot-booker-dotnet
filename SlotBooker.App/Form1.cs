using System;
using System.Windows.Forms;
using SlotBooker.Services;

namespace SlotBooker.App
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void findSlotButton_Click(object sender, EventArgs e)
        {
            SlotFinder slotFinder = new SlotFinder();
            slotFinder.FindSlot();
        }
    }
}
