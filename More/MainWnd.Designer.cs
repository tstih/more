namespace More
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
            this._lneVertical = new System.Windows.Forms.More.Line();
            this._lneHorizontal = new System.Windows.Forms.More.Line();
            this.line1 = new System.Windows.Forms.More.Line();
            this.line2 = new System.Windows.Forms.More.Line();
            this.SuspendLayout();
            // 
            // _lneVertical
            // 
            this._lneVertical.DashValues = new float[] {
        1F,
        0F};
            this._lneVertical.Dock = System.Windows.Forms.DockStyle.Left;
            this._lneVertical.Font = new System.Drawing.Font("Courier New", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this._lneVertical.LineColor = System.Drawing.SystemColors.WindowText;
            this._lneVertical.Location = new System.Drawing.Point(20, 20);
            this._lneVertical.Name = "_lneVertical";
            this._lneVertical.Orientation = System.Windows.Forms.Orientation.Vertical;
            this._lneVertical.Size = new System.Drawing.Size(26, 321);
            this._lneVertical.TabIndex = 0;
            this._lneVertical.Text = "Vertical Line";
            this._lneVertical.TextAlignment = System.Drawing.StringAlignment.Center;
            this._lneVertical.TextOffset = 8;
            this._lneVertical.Thickness = 1;
            // 
            // _lneHorizontal
            // 
            this._lneHorizontal.DashValues = new float[] {
        1F,
        1F};
            this._lneHorizontal.Dock = System.Windows.Forms.DockStyle.Top;
            this._lneHorizontal.LineColor = System.Drawing.Color.Red;
            this._lneHorizontal.Location = new System.Drawing.Point(46, 20);
            this._lneHorizontal.Name = "_lneHorizontal";
            this._lneHorizontal.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this._lneHorizontal.Size = new System.Drawing.Size(558, 26);
            this._lneHorizontal.TabIndex = 1;
            this._lneHorizontal.Text = "Horizontal Line";
            this._lneHorizontal.TextAlignment = System.Drawing.StringAlignment.Near;
            this._lneHorizontal.TextOffset = 8;
            this._lneHorizontal.Thickness = 1;
            // 
            // line1
            // 
            this.line1.DashValues = new float[] {
        1F,
        0F};
            this.line1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.line1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.line1.ForeColor = System.Drawing.Color.Red;
            this.line1.LineColor = System.Drawing.SystemColors.WindowText;
            this.line1.Location = new System.Drawing.Point(46, 315);
            this.line1.Name = "line1";
            this.line1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.line1.Size = new System.Drawing.Size(558, 26);
            this.line1.TabIndex = 2;
            this.line1.Text = "Horizontal Line";
            this.line1.TextAlignment = System.Drawing.StringAlignment.Far;
            this.line1.TextOffset = 8;
            this.line1.Thickness = 1;
            // 
            // line2
            // 
            this.line2.DashValues = new float[] {
        3F,
        1F,
        1F,
        1F};
            this.line2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.line2.LineColor = System.Drawing.SystemColors.WindowText;
            this.line2.Location = new System.Drawing.Point(46, 46);
            this.line2.Name = "line2";
            this.line2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.line2.Size = new System.Drawing.Size(558, 269);
            this.line2.TabIndex = 3;
            this.line2.TextAlignment = System.Drawing.StringAlignment.Near;
            this.line2.TextOffset = 8;
            this.line2.Thickness = 4;
            // 
            // MainWnd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 361);
            this.ControlBox = false;
            this.Controls.Add(this.line2);
            this.Controls.Add(this.line1);
            this.Controls.Add(this._lneHorizontal);
            this.Controls.Add(this._lneVertical);
            this.Name = "MainWnd";
            this.Padding = new System.Windows.Forms.Padding(20);
            this.Text = "Line";
            this.Load += new System.EventHandler(this.MainWnd_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.More.Line _lneVertical;
        private System.Windows.Forms.More.Line _lneHorizontal;
        private System.Windows.Forms.More.Line line1;
        private System.Windows.Forms.More.Line line2;
    }
}

