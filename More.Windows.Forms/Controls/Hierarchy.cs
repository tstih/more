/*
 * Hierarchy.cs
 * 
 * Draws a tree, delegates drawing nodes to you.
 *
 * MIT License (see: LICENSE)
 * Copyright (c) 2020 Tomaz Stih
 * 
 * 21.12.2020   tstih
 * 
 */
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace More.Windows.Forms
{
    public class Hierarchy : Control
    {
        #region Const(s)
        private const int DEFAULT_NODE_WIDTH = 96;
        private const int DEFAULT_NODE_HEIGHT = 26;
        #endregion // Const(s)

        #region Private(s)
        private IHierarchyFeed _feed;
        #endregion // Private(s)

        #region Ctor
        public Hierarchy()
        {
            DoubleBuffered = true;
            ResizeRedraw = true;

            // Set default property values.
            SetDefaults();
        }

        private void SetDefaults()
        {
            _nodeWidth = DEFAULT_NODE_WIDTH;
            _nodeHeight = DEFAULT_NODE_HEIGHT;
        }
        #endregion // Ctor

        #region Method(s)
        public void SetFeed(IHierarchyFeed feed)
        {
            // Nothing new.
            if (_feed == feed) return;
            // Store and repaint.
            _feed = feed; Invalidate();
        }
        #endregion // Method(s)

        #region Override(s)
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            e.Graphics.Clear(BackColor);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            
        }
        #endregion // Override(s)

        #region Properties
        int _nodeWidth;
        public int NodeWidth { get { return _nodeWidth; } set { _nodeWidth = value; Invalidate(); } }

        int _nodeHeight;
        public int NodeHeight{ get { return _nodeHeight; } set { _nodeHeight = value; Invalidate(); } }
        #endregion // Properties

        #region Helper(s)

        #endregion // Helper(s)
    }

    public interface IHierarchyFeed
    {
        IEnumerable<string> Query(string key=null);
    }
}
