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
            this._hierarchy = new More.Windows.Forms.Hierarchy();
            this.SuspendLayout();
            // 
            // _hierarchy
            // 
            this._hierarchy.Direction = More.Windows.Forms.Direction.Left2Right;
            this._hierarchy.Dock = System.Windows.Forms.DockStyle.Fill;
            this._hierarchy.Location = new System.Drawing.Point(20, 20);
            this._hierarchy.Name = "_hierarchy";
            this._hierarchy.NodeHeight = 16;
            this._hierarchy.NodeHorzSpacing = 16;
            this._hierarchy.NodeVertSpacing = 32;
            this._hierarchy.NodeWidth = 32;
            this._hierarchy.Size = new System.Drawing.Size(584, 321);
            this._hierarchy.TabIndex = 0;
            this._hierarchy.DrawNode += new System.EventHandler<More.Windows.Forms.DrawNodeEventArgs>(this._hierarchy_DrawNode);
            this._hierarchy.DrawEdge += new System.EventHandler<More.Windows.Forms.DrawEdgeEventArgs>(this._hierarchy_DrawEdge);
            this._hierarchy.MouseUp += new System.Windows.Forms.MouseEventHandler(this._hierarchy_MouseUp);
            // 
            // MainWnd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 361);
            this.ControlBox = false;
            this.Controls.Add(this._hierarchy);
            this.Name = "MainWnd";
            this.Padding = new System.Windows.Forms.Padding(20);
            this.Text = "Hierarchy";
            this.Load += new System.EventHandler(this.MainWnd_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Windows.Forms.Hierarchy _hierarchy;
        private Windows.Forms.Hierarchy hierarchy1;
    }
}

