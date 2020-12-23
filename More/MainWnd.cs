/*
 * MainWnd.cs
 * 
 * Playground for testing controls.
 *
 * MIT License (see: LICENSE)
 * Copyright (c) 2020 Tomaz Stih
 * 
 * 20.12.2020   tstih
 * 
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using More.Windows.Forms;

namespace More
{
    public partial class MainWnd : Form
    {
        public MainWnd()
        {
            InitializeComponent();
        }

        private void MainWnd_Load(object sender, EventArgs e)
        {
            // First the title.
            _frame.Title = "Hello!";
            _frame.TitleAlignment = StringAlignment.Center;
            _frame.TitleHeight = 16;
            _frame.TitleBackColor = BackColor;
            _frame.TitleForeColor = ForeColor;

            // Outer border.
            _frame.OuterBorderDarkColor = Color.FromKnownColor(KnownColor.ControlDark);
            _frame.OuterBorderLightColor = Color.FromKnownColor(KnownColor.ControlLight);
            _frame.OuterBorderThickness = 1;

            // Inner border (replace dark and light).
            _frame.InnerBorderDarkColor = Color.FromKnownColor(KnownColor.ControlLight);
            _frame.InnerBorderLightColor = Color.FromKnownColor(KnownColor.ControlDark);
            _frame.InnerBorderThickness = 1;

            // Pixels between inner and outer color.
            _frame.BorderThickness = 2;
        }
    }

        
}