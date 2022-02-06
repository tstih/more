/*
 * Program.cs
 * 
 * Demos application entry point.
 *
 * MIT License (see: LICENSE)
 * Copyright (c) 2022 Tomaz Stih
 * 
 * 05.02.2022   tstih
 * 
 */
using System;
using System.Windows.Forms;

namespace More.Demos
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainWnd());
        }
    }
}
