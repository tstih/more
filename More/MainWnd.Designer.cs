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
            this.spriteGrid1 = new System.Windows.Forms.More.SpriteGrid();
            this.SuspendLayout();
            // 
            // spriteGrid1
            // 
            this.spriteGrid1.AutoScroll = true;
            this.spriteGrid1.AutoScrollMinSize = new System.Drawing.Size(277, 533);
            this.spriteGrid1.BottomMargin = 0;
            this.spriteGrid1.CellHeight = 8;
            this.spriteGrid1.CellWidth = 8;
            this.spriteGrid1.Columns = 32;
            this.spriteGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spriteGrid1.Font = new System.Drawing.Font("Segoe UI", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.spriteGrid1.ForeColor = System.Drawing.SystemColors.WindowText;
            this.spriteGrid1.GridEdgeLineColor = System.Drawing.SystemColors.WindowText;
            this.spriteGrid1.GridEdgeLineDashPattern = new float[] {
        1F,
        0F};
            this.spriteGrid1.GridSelection = null;
            this.spriteGrid1.GridTickLineColor = System.Drawing.SystemColors.ControlDark;
            this.spriteGrid1.GridTickLineDashPattern = new float[] {
        1F,
        1F};
            this.spriteGrid1.LeftMargin = 0;
            this.spriteGrid1.Location = new System.Drawing.Point(0, 0);
            this.spriteGrid1.MajorTickSize = 12;
            this.spriteGrid1.MarginLineThickness = 1;
            this.spriteGrid1.MinorTickSize = 4;
            this.spriteGrid1.MinorTicksPerMajorTick = 8;
            this.spriteGrid1.Name = "spriteGrid1";
            this.spriteGrid1.RightMargin = 0;
            this.spriteGrid1.Rows = 64;
            this.spriteGrid1.RulerBackgroundColor = System.Drawing.SystemColors.ControlLight;
            this.spriteGrid1.RulerHeight = 20;
            this.spriteGrid1.RulerWidth = 20;
            this.spriteGrid1.ShowHorzRuler = true;
            this.spriteGrid1.ShowVertRuler = true;
            this.spriteGrid1.Size = new System.Drawing.Size(624, 361);
            this.spriteGrid1.SourceImage = null;
            this.spriteGrid1.TabIndex = 0;
            this.spriteGrid1.Text = "_spriteGrid";
            this.spriteGrid1.TopMargin = 0;
            // 
            // MainWnd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 361);
            this.ControlBox = false;
            this.Controls.Add(this.spriteGrid1);
            this.Name = "MainWnd";
            this.Text = "SpriteGrid";
            this.Load += new System.EventHandler(this.MainWnd_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.More.SpriteGrid spriteGrid1;
    }
}

