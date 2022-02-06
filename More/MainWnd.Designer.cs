
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
            More.Windows.Forms.Theme theme1 = new More.Windows.Forms.Theme();
            this._navigator = new More.Windows.Forms.Navigator();
            this._workspacePanel = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // _navigator
            // 
            this._navigator.ActiveCategories = false;
            this._navigator.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(16)))), ((int)(((byte)(64)))));
            this._navigator.CollapseWidth = 48;
            this._navigator.Dock = System.Windows.Forms.DockStyle.Left;
            this._navigator.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this._navigator.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(176)))), ((int)(((byte)(192)))));
            this._navigator.HoverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(32)))), ((int)(((byte)(96)))));
            this._navigator.HoverForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(176)))), ((int)(((byte)(192)))));
            this._navigator.ItemHeight = 32;
            this._navigator.ItemIndent = 8;
            this._navigator.Location = new System.Drawing.Point(0, 0);
            this._navigator.Margin = new System.Windows.Forms.Padding(0);
            this._navigator.Name = "_navigator";
            this._navigator.Padding = new System.Windows.Forms.Padding(8);
            this._navigator.SelectedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(100)))), ((int)(((byte)(164)))));
            this._navigator.SelectedForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this._navigator.Size = new System.Drawing.Size(173, 450);
            this._navigator.TabIndex = 0;
            this._navigator.Theme = theme1;
            this._navigator.TitleFont = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this._navigator.TitleForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this._navigator.TitleHeight = 48;
            this._navigator.TitleSeparatorColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(32)))), ((int)(((byte)(96)))));
            // 
            // _workspacePanel
            // 
            this._workspacePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._workspacePanel.Location = new System.Drawing.Point(173, 0);
            this._workspacePanel.Name = "_workspacePanel";
            this._workspacePanel.Size = new System.Drawing.Size(627, 450);
            this._workspacePanel.TabIndex = 1;
            // 
            // MainWnd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this._workspacePanel);
            this.Controls.Add(this._navigator);
            this.Name = "MainWnd";
            this.Text = "More Demos";
            this.ResumeLayout(false);

        }

        #endregion

        private Windows.Forms.Navigator _navigator;
        private System.Windows.Forms.Panel _workspacePanel;
    }
}

