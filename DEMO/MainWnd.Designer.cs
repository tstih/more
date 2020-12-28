
namespace DEMO
{
    partial class MainWnd
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.labelEx1 = new More.Windows.Forms.Controls.LabelEx();
            this.SuspendLayout();
            // 
            // labelEx1
            // 
            this.labelEx1.Angle = 45F;
            this.labelEx1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelEx1.Font = new System.Drawing.Font("Akayla Script PERSONAL USE", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.labelEx1.HorzAlignment = System.Drawing.StringAlignment.Center;
            this.labelEx1.Location = new System.Drawing.Point(0, 0);
            this.labelEx1.Name = "labelEx1";
            this.labelEx1.Opacity = 0;
            this.labelEx1.Size = new System.Drawing.Size(624, 361);
            this.labelEx1.TabIndex = 0;
            this.labelEx1.Text = "What a day!";
            this.labelEx1.VertAlignment = System.Drawing.StringAlignment.Center;
            // 
            // MainWnd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 361);
            this.ControlBox = false;
            this.Controls.Add(this.labelEx1);
            this.Name = "MainWnd";
            this.Text = "LabelEx";
            this.Load += new System.EventHandler(this.MainWnd_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private More.Windows.Forms.Controls.LabelEx labelEx1;
    }
}

