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
            this._spriteGrid = new System.Windows.Forms.More.SpriteGrid();
            this.SuspendLayout();
            // 
            // _spriteGrid
            // 
            this._spriteGrid.AutoScroll = true;
            this._spriteGrid.AutoScrollMinSize = new System.Drawing.Size(85, 85);
            this._spriteGrid.BackColor = System.Drawing.SystemColors.Window;
            this._spriteGrid.BottomMargin = 0;
            this._spriteGrid.CellHeight = 8;
            this._spriteGrid.CellWidth = 8;
            this._spriteGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this._spriteGrid.Font = new System.Drawing.Font("Segoe UI", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this._spriteGrid.ForeColor = System.Drawing.SystemColors.WindowText;
            this._spriteGrid.GridEdgeLineColor = System.Drawing.SystemColors.WindowText;
            this._spriteGrid.GridEdgeLineDashPattern = new float[] {
        1F,
        0F};
            this._spriteGrid.GridTickLineColor = System.Drawing.SystemColors.ControlDark;
            this._spriteGrid.GridTickLineDashPattern = new float[] {
        1F,
        1F};
            this._spriteGrid.LeftMargin = 0;
            this._spriteGrid.Location = new System.Drawing.Point(0, 0);
            this._spriteGrid.MajorTickSize = 10;
            this._spriteGrid.MarginColor = System.Drawing.SystemColors.Window;
            this._spriteGrid.MarginLineThickness = 1;
            this._spriteGrid.MinorTickSize = 6;
            this._spriteGrid.MinorTicksPerMajorTick = 4;
            this._spriteGrid.Name = "_spriteGrid";
            this._spriteGrid.RightMargin = 0;
            this._spriteGrid.RulerBackgroundColor = System.Drawing.SystemColors.ControlLight;
            this._spriteGrid.RulerHeight = 20;
            this._spriteGrid.RulerWidth = 20;
            this._spriteGrid.ShowHorzRuler = true;
            this._spriteGrid.ShowVertRuler = true;
            this._spriteGrid.Size = new System.Drawing.Size(624, 361);
            this._spriteGrid.TabIndex = 0;
            this._spriteGrid.TopMargin = 0;
            this._spriteGrid.ZoomIn += new System.EventHandler<System.Windows.Forms.More.ZoomInArgs>(this._spriteGrid_ZoomIn);
            this._spriteGrid.ZoomOut += new System.EventHandler<System.Windows.Forms.More.ZoomOutArgs>(this._spriteGrid_ZoomOut);
            this._spriteGrid.CellMouseDown += new System.EventHandler<System.Windows.Forms.More.CellMouseButtonArgs>(this._spriteGrid_CellMouseDown);
            this._spriteGrid.CellMouseUp += new System.EventHandler<System.Windows.Forms.More.CellMouseButtonArgs>(this._spriteGrid_CellMouseUp);
            this._spriteGrid.CellMouseMove += new System.EventHandler<System.Windows.Forms.More.CellMousePosArgs>(this._spriteGrid_CellMouseMove);
            // 
            // MainWnd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 361);
            this.Controls.Add(this._spriteGrid);
            this.Name = "MainWnd";
            this.Text = "SpriteGrid";
            this.Load += new System.EventHandler(this.MainWnd_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.More.SpriteGrid _spriteGrid;
    }
}

