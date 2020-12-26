
namespace SlotBooker.UI
{
    partial class BookSlotForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.findSlotButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // findSlotButton
            // 
            this.findSlotButton.Location = new System.Drawing.Point(189, 148);
            this.findSlotButton.Name = "findSlotButton";
            this.findSlotButton.Size = new System.Drawing.Size(172, 89);
            this.findSlotButton.TabIndex = 0;
            this.findSlotButton.Text = "Find Slot";
            this.findSlotButton.UseVisualStyleBackColor = true;
            this.findSlotButton.Click += new System.EventHandler(this.findSlotButton_Click);
            // 
            // BookSlotForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(551, 392);
            this.Controls.Add(this.findSlotButton);
            this.Name = "BookSlotForm";
            this.Text = "BookSlotForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button findSlotButton;
    }
}