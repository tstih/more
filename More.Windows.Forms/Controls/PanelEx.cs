/*
 * PanelEx.cs
 * 
 * Panel that enables painted decorations.
 * Derive your panel from this class, set the Margin 
 * to offset client rectangle, and override the
 * Decorate function to paint decorations in the pseudo 
 * "non-client" area.
 * See: Frame (derived from PanelEx) for example of use.
 *
 * MIT License (see: LICENSE)
 * Copyright (c) 2020 Tomaz Stih
 * 
 * 03.02.2020   tstih
 * 
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace More.Windows.Forms
{
    public class PanelEx : Panel
    {
        #region Ctor(s)
        public PanelEx() : base() {}
        #endregion // Ctor(s)

        #region Protected(s)
        protected Rectangle _lt;
        protected Rectangle _rt;
        protected Rectangle _lb;
        protected Rectangle _rb;
        protected Rectangle _l;
        protected Rectangle _t;
        protected Rectangle _r;
        protected Rectangle _b;
        #endregion // Protected(s)

        #region Override(s)
        protected override void OnSizeChanged(EventArgs e)
        {
            // Layout rectangles.
            LayoutRectangles();

            // Relayout.
            Relayout(_lt, _rt, _lb, _rb, _l, _t, _r, _b);

            // Pass the call.
            base.OnSizeChanged(e);
        }

        public override Rectangle DisplayRectangle
        {
            get
            {
                Rectangle rect = new Rectangle(
                    base.DisplayRectangle.Left + Margin.Left,
                    base.DisplayRectangle.Top + Margin.Top,
                    base.DisplayRectangle.Width - Margin.Left - Margin.Right,
                    base.DisplayRectangle.Height - Margin.Top - Margin.Bottom);
                return rect;
            }
        }

        protected override void OnMarginChanged(EventArgs e)
        {
            // Relayout.
            PerformLayout();

            // And repaint.
            Invalidate();
        }

        // TODO: Rectangle calculation seems a bit complicated. Also check pixel math, look for +1/-1 pixel errors.
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            // Clear entire area.
            Graphics g = e.Graphics;
            g.Clear(BackColor);

            // Layout rectangles.
            LayoutRectangles();

            // And paint deco.
            Decorate(e.Graphics, _lt, _rt, _lb, _rb, _l, _t, _r, _b);
        }
        #endregion // Override(s)

        #region Overridable(s)
        // Override this.
        protected virtual void Decorate(Graphics g, Rectangle lt, Rectangle rt, Rectangle lb, Rectangle rb, Rectangle l, Rectangle t, Rectangle r, Rectangle b)
        { }

        protected virtual void Relayout(Rectangle lt, Rectangle rt, Rectangle lb, Rectangle rb, Rectangle l, Rectangle t, Rectangle r, Rectangle b)
        { }
        #endregion // Overridable(s)

        #region Helper(s)
        private void LayoutRectangles()
        {
            // Our target rectangle is client rectangle. We'll raise events to paint each part if margin is set.
            _lt = new Rectangle(ClientRectangle.Left, ClientRectangle.Top, Margin.Left, Margin.Top);
            _rt = new Rectangle(ClientRectangle.Right - Margin.Right, ClientRectangle.Top, Margin.Right, Margin.Top);
            _lb = new Rectangle(ClientRectangle.Left, ClientRectangle.Bottom - Margin.Bottom, Margin.Left, Margin.Bottom);
            _rb = new Rectangle(ClientRectangle.Right - Margin.Right, ClientRectangle.Bottom - Margin.Bottom, Margin.Right, Margin.Bottom);
            _l = new Rectangle(ClientRectangle.Left, ClientRectangle.Top + Margin.Top, Margin.Left, ClientRectangle.Height - Margin.Top - Margin.Bottom);
            _t = new Rectangle(ClientRectangle.Left + Margin.Left, ClientRectangle.Top, ClientRectangle.Width - Margin.Left - Margin.Right, Margin.Top);
            _r = new Rectangle(ClientRectangle.Right - Margin.Right, ClientRectangle.Top + Margin.Top, Margin.Right, ClientRectangle.Height - Margin.Top - Margin.Bottom);
            _b = new Rectangle(ClientRectangle.Left + Margin.Left, ClientRectangle.Bottom - Margin.Bottom, ClientRectangle.Width - Margin.Left - Margin.Right, Margin.Bottom);
        }
        #endregion // Helper(s)
    }
}