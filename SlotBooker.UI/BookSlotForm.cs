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
        public BookSlotForm()
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
