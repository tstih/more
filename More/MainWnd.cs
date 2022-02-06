/*
 * MainWnd.cs
 * 
 * More.Demos main window.
 *
 * MIT License (see: LICENSE)
 * Copyright (c) 2022 Tomaz Stih
 * 
 * 05.02.2022   tstih
 * 
 */
using System;
using System.Windows.Forms;

using More.Windows.Forms;
using More.Demos;

namespace More
{
    public partial class MainWnd : Form
    {
        public MainWnd()
        {
            // Init controls. 
            InitializeComponent();

            // Create navigator.
            _navigator.SetRoot(
                NavigatorBuilder.Fluent()
                    .Add("DocumentPreview")
                    .Add("Frame", () => Set(typeof(FrameDemo)))
                    .Add("Hierarchy")
                    .Add("LabelEx")
                    .Add("Line")
                    .Add("Listing", () => Set(typeof(ListingDemo)))
                    .Add("Monitors")
                    .Add("Navigator")
                    .Add("Prompt")
                    .Add("SecurityMatrix")
                    .Add("SpriteGrid")
                    .Begin("Navigate to...")
                        .Add("More github")
                    .End()
                    .GetRoot()
            );
        }

        private void Set(Type demo)
        {
            // Clear the workspace.
            _workspacePanel.Controls.Clear();
            // Create the control and dock it.
            var ctl=Activator.CreateInstance(demo) as UserControl;
            ctl.Dock = DockStyle.Fill;
            // Add it to the workspace.
            _workspacePanel.Controls.Add(ctl);
        }
    }
}
