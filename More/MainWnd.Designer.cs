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
            this._line = new  System.Windows.Forms.More.Line();
            this.line1 = new  System.Windows.Forms.More.Line();
            this.line2 = new  System.Windows.Forms.More.Line();
            this.line3 = new  System.Windows.Forms.More.Line();
            this.line4 = new  System.Windows.Forms.More.Line();
            this.line5 = new  System.Windows.Forms.More.Line();
            this.SuspendLayout();
            // 
            // _line
            // 
            this._line.DashValues = new float[] {
        1F,
        1F};
            this._line.LineColor = System.Drawing.SystemColors.WindowText;
            this._line.Location = new System.Drawing.Point(39, 24);
            this._line.Name = "_line";
            this._line.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this._line.Size = new System.Drawing.Size(238, 26);
            this._line.TabIndex = 0;
            this._line.Text = "Text";
            this._line.TextAlignment = System.Drawing.StringAlignment.Near;
            this._line.TextOffset = 8;
            this._line.Thickness = 1;
            // 
            // line1
            // 
            this.line1.DashValues = new float[] {
        1F,
        1F};
            this.line1.LineColor = System.Drawing.SystemColors.WindowText;
            this.line1.Location = new System.Drawing.Point(39, 56);
            this.line1.Name = "line1";
            this.line1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.line1.Size = new System.Drawing.Size(238, 26);
            this.line1.TabIndex = 0;
            this.line1.Text = "Text";
            this.line1.TextAlignment = System.Drawing.StringAlignment.Center;
            this.line1.TextOffset = 8;
            this.line1.Thickness = 1;
            // 
            // line2
            // 
            this.line2.DashValues = new float[] {
        1F,
        1F};
            this.line2.LineColor = System.Drawing.SystemColors.WindowText;
            this.line2.Location = new System.Drawing.Point(39, 88);
            this.line2.Name = "line2";
            this.line2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.line2.Size = new System.Drawing.Size(238, 26);
            this.line2.TabIndex = 0;
            this.line2.Text = "Text";
            this.line2.TextAlignment = System.Drawing.StringAlignment.Far;
            this.line2.TextOffset = 8;
            this.line2.Thickness = 1;
            // 
            // line3
            // 
            this.line3.DashValues = new float[] {
        1F,
        1F};
            this.line3.LineColor = System.Drawing.SystemColors.WindowText;
            this.line3.Location = new System.Drawing.Point(283, 33);
            this.line3.Name = "line3";
            this.line3.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.line3.Size = new System.Drawing.Size(32, 125);
            this.line3.TabIndex = 0;
            this.line3.Text = "Text";
            this.line3.TextAlignment = System.Drawing.StringAlignment.Near;
            this.line3.TextOffset = 8;
            this.line3.Thickness = 1;
            // 
            // line4
            // 
            this.line4.DashValues = new float[] {
        1F,
        1F};
            this.line4.LineColor = System.Drawing.SystemColors.WindowText;
            this.line4.Location = new System.Drawing.Point(321, 33);
            this.line4.Name = "line4";
            this.line4.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.line4.Size = new System.Drawing.Size(32, 125);
            this.line4.TabIndex = 0;
            this.line4.Text = "Text";
            this.line4.TextAlignment = System.Drawing.StringAlignment.Center;
            this.line4.TextOffset = 8;
            this.line4.Thickness = 1;
            // 
            // line5
            // 
            this.line5.DashValues = new float[] {
        1F,
        1F};
            this.line5.LineColor = System.Drawing.SystemColors.WindowText;
            this.line5.Location = new System.Drawing.Point(359, 33);
            this.line5.Name = "line5";
            this.line5.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.line5.Size = new System.Drawing.Size(32, 125);
            this.line5.TabIndex = 0;
            this.line5.Text = "Text";
            this.line5.TextAlignment = System.Drawing.StringAlignment.Far;
            this.line5.TextOffset = 8;
            this.line5.Thickness = 1;
            // 
            // MainWnd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(674, 316);
            this.Controls.Add(this.line5);
            this.Controls.Add(this.line4);
            this.Controls.Add(this.line3);
            this.Controls.Add(this.line2);
            this.Controls.Add(this.line1);
            this.Controls.Add(this._line);
            this.Name = "MainWnd";
            this.Text = "More (Win Forms)";
            this.ResumeLayout(false);

        }

        #endregion

        private  System.Windows.Forms.More.Line _line;
        private  System.Windows.Forms.More.Line line1;
        private  System.Windows.Forms.More.Line line2;
        private  System.Windows.Forms.More.Line line3;
        private  System.Windows.Forms.More.Line line4;
        private  System.Windows.Forms.More.Line line5;
    }
}

